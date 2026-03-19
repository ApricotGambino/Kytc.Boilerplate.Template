using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;

namespace CodeGenerator
{
    [Generator]
    public class DomainEntityDtoGenerator : IIncrementalGenerator
    {
        public void Initialize(IncrementalGeneratorInitializationContext context)
        {
            var classesMarkedWithTheGeneratorAttribute =
                context.SyntaxProvider.ForAttributeWithMetadataName(
                $"CodeGenerator.{DomainEntityAttributeGenerator.DomainEntityAttributeName}",
                    predicate: static (node, _) => node is ClassDeclarationSyntax,
                    transform: static (ctx, _) => (ClassDeclarationSyntax)ctx.TargetNode
                ).Where(static c => c != null);

            var compilationAndEntities = context.CompilationProvider.Combine(classesMarkedWithTheGeneratorAttribute.Collect());
            context.RegisterSourceOutput(compilationAndEntities, GenerateDtos);
        }

        private static void GenerateDtos(SourceProductionContext context, (Compilation, ImmutableArray<ClassDeclarationSyntax>) tuple)
        {
            var (compilation, entities) = tuple;
            foreach (var entity in entities)
            {
                var semanticModel = compilation.GetSemanticModel(entity.SyntaxTree);
                var classSymbol = semanticModel.GetDeclaredSymbol(entity);
                if (classSymbol == null)
                    continue;

                var dtoSource = GenerateDtoClass((INamedTypeSymbol)classSymbol);
                context.AddSource($"{classSymbol.Name}GeneratedDto.g.cs", SourceText.From(dtoSource, Encoding.UTF8));
            }
        }

        private static string GenerateDtoClass(INamedTypeSymbol classSymbol)
        {
            var sb = new StringBuilder();
            string tab = "    ";
            var namespaceName = classSymbol.ContainingNamespace.ToDisplayString();
            var className = classSymbol.Name;
            var publicMembers = classSymbol.GetMembers().OfType<IPropertySymbol>().AsQueryable().Where(p => p.DeclaredAccessibility == Accessibility.Public).ToList();
            var fieldsInterface = classSymbol.Interfaces.Where(p => p.Name.EndsWith("Fields")).FirstOrDefault();

            var generatedClassName = $"{className}GeneratedDto";
            var mapInterface = $"IMap<{className}, {generatedClassName}>";

            var interfaces = $"{fieldsInterface.Name}, IPrimaryKey<int>, {mapInterface}, IDto";

            sb.AppendLine($"using Kernel.Data.Entities;");
            sb.AppendLine($"using Kernel.Data.Mapping;");
            sb.AppendLine($"namespace {namespaceName}");
            sb.AppendLine("{");
            sb.AppendLine($"{tab}public record {generatedClassName}() : {interfaces}");
            sb.AppendLine($"{tab}{{");
            sb.Append($"{GenerateDtoPropertiesString(publicMembers, tab)}");
            sb.AppendLine();
            sb.Append($"{GenerateMapFromString(publicMembers, generatedClassName, className, tab)}");
            sb.AppendLine($"{tab}}}");
            sb.AppendLine("}");
            return sb.ToString();
        }

        private static string GenerateDtoPropertiesString(List<IPropertySymbol> publicMembers, string tab)
        {
            var sb = new StringBuilder();
            sb.AppendLine($"{tab}{tab}public required int Id {{ get; set; }}");
            foreach (var property in publicMembers)
            {
                sb.AppendLine($"{tab}{tab}public required {property.Type.ToDisplayString()} {property.Name} {{ get; set; }}");
            }
            return sb.ToString();
        }

        private static string GenerateMapFromString(List<IPropertySymbol> publicMembers, string generatedClassName, string className, string tab)
        {
            var sb = new StringBuilder();
            sb.AppendLine($"{tab}{tab}public static {generatedClassName} MapFrom({className} entity)");
            sb.AppendLine($"{tab}{tab}{{");
            sb.AppendLine($"{tab}{tab}{tab}return new {generatedClassName}()");
            sb.AppendLine($"{tab}{tab}{tab}{{");
            sb.AppendLine($"{tab}{tab}{tab}{tab}Id = entity.Id,");
            foreach (var propertyName in publicMembers.Select(s => s.Name))
            {
                sb.AppendLine($"{tab}{tab}{tab}{tab}{propertyName} = entity.{propertyName},");
            }
            sb.AppendLine($"{tab}{tab}{tab}}};");
            sb.AppendLine($"{tab}{tab}}}");
            return sb.ToString();
        }
    }
}



