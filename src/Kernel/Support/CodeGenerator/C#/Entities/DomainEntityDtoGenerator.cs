// DomainEntityDtoGenerator.cs is part of the Boilerplate kernel, modify at your own risk.
// You can get updates from the BP repository.

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
            var (compilation, domainEntities) = tuple;

            foreach (var entity in domainEntities)
            {
                var semanticModel = compilation.GetSemanticModel(entity.SyntaxTree);
                var classSymbol = semanticModel.GetDeclaredSymbol(entity);
                if (classSymbol == null)
                    continue;

                var dtoPropertySymbols = new List<DtoPropertySymbol>();
                var entitySymbol = (INamedTypeSymbol)classSymbol;

                var publicProperties = entitySymbol.GetMembers().OfType<IPropertySymbol>().AsQueryable().Where(p => p.DeclaredAccessibility == Accessibility.Public).ToList();


                foreach (var property in publicProperties.Where(p => p != null).ToList())
                {
                    dtoPropertySymbols.Add(new DtoPropertySymbol()
                    {
                        PropertySymbol = property!,
                        IsPropertyADomainEntity = IsPropertyADomainEntity(property),
                        PropertyNamespace = property.Type.ContainingNamespace.ToDisplayString()
                    });
                }

                var responseDtoSource = GenerateResponseDtoClass(entitySymbol, dtoPropertySymbols);
                context.AddSource($"{classSymbol.Name}ResponseDto.g.cs", SourceText.From(responseDtoSource, Encoding.UTF8));

                var fullResponseDtoSource = GenerateFullResponseDtoClass(entitySymbol, dtoPropertySymbols);
                context.AddSource($"{classSymbol.Name}FullResponseDto.g.cs", SourceText.From(fullResponseDtoSource, Encoding.UTF8));

                var createRequestDtoSource = GenerateCreateRequestDtoClass(entitySymbol, dtoPropertySymbols);
                context.AddSource($"{classSymbol.Name}CreateRequestDto.g.cs", SourceText.From(createRequestDtoSource, Encoding.UTF8));

                var updateRequestDtoSource = GenerateUpdateRequestDtoClass(entitySymbol, dtoPropertySymbols);
                context.AddSource($"{classSymbol.Name}UpdateRequestDto.g.cs", SourceText.From(updateRequestDtoSource, Encoding.UTF8));
            }
        }

        private static string GenerateResponseDtoClass(INamedTypeSymbol entitySymbol, List<DtoPropertySymbol> dtoProperties)
        {
            var sb = new StringBuilder();
            var tab = TabSpacing;
            var entityName = entitySymbol.Name;
            var generatedClassName = GetResponseClassName(entityName);
            var mapInterface = $"IMap<{entityName}, {generatedClassName}>, IMap<{generatedClassName}, {entityName}>";
            var interfaces = $"IPrimaryKey<int>, {mapInterface}, IDto";

            sb.AppendLine(GenerateUsingString(entitySymbol, dtoProperties));
            sb.AppendLine($"namespace {Namespace}");
            sb.AppendLine("{");
            sb.AppendLine($"{tab}public record {generatedClassName}() : {interfaces}");
            sb.AppendLine($"{tab}{{");
            sb.Append($"{GenerateDtoPropertiesString(dtoProperties, includeId: true, includeDomainEntities: true)}");
            sb.AppendLine();
            sb.Append($"{GenerateMapFromEntityString(dtoProperties, generatedClassName, entityName, includeId: true, includeDomainEntities: true)}");
            sb.AppendLine();
            sb.Append($"{GenerateMapFromGeneratedDtoString(dtoProperties, generatedClassName, entityName, includeId: true, includeDomainEntities: true)}");
            sb.AppendLine($"{tab}}}");
            sb.AppendLine("}");
            return sb.ToString();
        }
        private static string GenerateFullResponseDtoClass(INamedTypeSymbol entitySymbol, List<DtoPropertySymbol> dtoProperties)
        {
            var entityName = entitySymbol.Name;
            var baseEntityAuditInterfaceName = "IBaseEntityAuditProperties";
            var baseEntityAuditInterface = entitySymbol.AllInterfaces.Where(p => p.Name.EndsWith(baseEntityAuditInterfaceName)).FirstOrDefault();
            var baseEntityAuditProperties = baseEntityAuditInterface.GetMembers().OfType<IPropertySymbol>().AsQueryable().Where(p => p.DeclaredAccessibility == Accessibility.Public).ToList();

            var dtoBaseEntityAuditProperties = new List<DtoPropertySymbol>();
            foreach (var property in baseEntityAuditProperties.Where(p => p != null).ToList())
            {
                dtoBaseEntityAuditProperties.Add(new DtoPropertySymbol()
                {
                    PropertySymbol = property!,
                    IsPropertyADomainEntity = IsPropertyADomainEntity(property),
                    PropertyNamespace = property.Type.ContainingNamespace.ToDisplayString()
                });
            }

            var allProperties = dtoProperties.ToList();
            allProperties.AddRange(dtoBaseEntityAuditProperties);
            var sb = new StringBuilder();

            var tab = TabSpacing;
            var mapInterface = $"IMap<{entityName}, {GetFullResponseClassName(entityName)}>, IMap<{GetFullResponseClassName(entityName)}, {entityName}>";

            sb.AppendLine(GenerateUsingString(entitySymbol, dtoProperties));
            sb.AppendLine($"namespace {Namespace}");
            sb.AppendLine("{");
            sb.AppendLine($"{tab}public record {GetFullResponseClassName(entityName)}() : {GetResponseClassName(entityName)}, {baseEntityAuditInterfaceName}, {mapInterface}");
            sb.AppendLine($"{tab}{{");
            sb.Append($"{GenerateDtoPropertiesString(dtoBaseEntityAuditProperties, includeId: false, includeDomainEntities: true)}");
            sb.AppendLine();
            sb.Append($"{GenerateMapFromEntityString(allProperties, GetFullResponseClassName(entityName), entityName, includeId: true, includeDomainEntities: true)}");
            sb.AppendLine();
            sb.Append($"{GenerateMapFromGeneratedDtoString(allProperties, GetFullResponseClassName(entityName), entityName, includeId: true, includeDomainEntities: true)}");
            sb.AppendLine($"{tab}}}");
            sb.AppendLine("}");
            return sb.ToString();
        }
        private static string GenerateCreateRequestDtoClass(INamedTypeSymbol entitySymbol, List<DtoPropertySymbol> dtoProperties)
        {
            var sb = new StringBuilder();
            var tab = TabSpacing;
            var entityName = entitySymbol.Name;
            var generatedClassName = GetCreateRequestClassName(entityName);

            var mapInterface = $"IMap<{entityName}, {generatedClassName}>, IMap<{generatedClassName}, {entityName}>";
            var interfaces = $"{mapInterface}, IDto";

            sb.AppendLine(GenerateUsingString(entitySymbol, dtoProperties));
            sb.AppendLine($"namespace {Namespace}");
            sb.AppendLine("{");
            sb.AppendLine($"{tab}public record {generatedClassName}() : {interfaces}");
            sb.AppendLine($"{tab}{{");
            sb.Append($"{GenerateDtoPropertiesString(dtoProperties, includeId: false, includeDomainEntities: false)}");
            sb.AppendLine();
            sb.Append($"{GenerateMapFromEntityString(dtoProperties, generatedClassName, entityName, includeId: false, includeDomainEntities: false)}");
            sb.AppendLine();
            sb.Append($"{GenerateMapFromGeneratedDtoString(dtoProperties, generatedClassName, entityName, includeId: false, includeDomainEntities: false)}");
            sb.AppendLine($"{tab}}}");
            sb.AppendLine("}");
            return sb.ToString();
        }
        private static string GenerateUpdateRequestDtoClass(INamedTypeSymbol entitySymbol, List<DtoPropertySymbol> dtoProperties)
        {
            var sb = new StringBuilder();
            var tab = TabSpacing;
            var entityName = entitySymbol.Name;
            var generatedClassName = GetUpdateRequestClassName(entityName);
            var mapInterface = $"IMap<{entityName}, {generatedClassName}>, IMap<{generatedClassName}, {entityName}>";

            //var interfaces = $"{fieldsInterface.Name}, {mapInterface}, IDto";
            var interfaces = $"{mapInterface}, IDto";

            sb.AppendLine(GenerateUsingString(entitySymbol, dtoProperties));
            sb.AppendLine($"namespace {Namespace}");
            sb.AppendLine("{");
            sb.AppendLine($"{tab}public record {generatedClassName}() : {interfaces}");
            sb.AppendLine($"{tab}{{");
            sb.Append($"{GenerateDtoPropertiesString(dtoProperties, includeId: true, includeDomainEntities: false)}");
            sb.AppendLine();
            sb.Append($"{GenerateMapFromEntityString(dtoProperties, generatedClassName, entityName, includeId: true, includeDomainEntities: false)}");
            sb.AppendLine();
            sb.Append($"{GenerateMapFromGeneratedDtoString(dtoProperties, generatedClassName, entityName, includeId: true, includeDomainEntities: false)}");
            sb.AppendLine($"{tab}}}");
            sb.AppendLine("}");
            return sb.ToString();
        }
        private static string GenerateDtoPropertiesString(List<DtoPropertySymbol> dtoProperties, bool includeId, bool includeDomainEntities)
        {
            var sb = new StringBuilder();
            var tab = TabSpacing;
            if (includeId)
            {
                sb.AppendLine($"{tab}{tab}public required int Id {{ get; set; }}");
            }
            foreach (var dtoProperty in dtoProperties)
            {
                if (dtoProperty.IsPropertyADomainEntity)
                {
                    if (includeDomainEntities)
                    {
                        sb.AppendLine($"{tab}{tab}public {GetResponseClassName(dtoProperty.PropertySymbol.Name)}? {dtoProperty.PropertySymbol.Name} {{ get; set; }}");
                    }
                }
                else
                {
                    sb.AppendLine($"{tab}{tab}public required {dtoProperty.PropertySymbol.Type.ToDisplayString()} {dtoProperty.PropertySymbol.Name} {{ get; set; }}");
                }
            }
            return sb.ToString();
        }
        private static string GenerateMapFromEntityString(List<DtoPropertySymbol> dtoProperties, string generatedClassName, string entityName, bool includeId, bool includeDomainEntities)
        {
            var sb = new StringBuilder();
            var tab = TabSpacing;
            sb.AppendLine($"{tab}{tab}public static {generatedClassName} MapFrom({entityName} entity)");
            sb.AppendLine($"{tab}{tab}{{");
            sb.AppendLine($"{tab}{tab}{tab}return new {generatedClassName}()");
            sb.AppendLine($"{tab}{tab}{tab}{{");

            if (includeId)
            {
                sb.AppendLine($"{tab}{tab}{tab}{tab}Id = entity.Id,");
            }
            foreach (var dtoProperty in dtoProperties)
            {
                if (dtoProperty.IsPropertyADomainEntity)
                {
                    if (includeDomainEntities)
                    {
                        sb.AppendLine($"{tab}{tab}{tab}{tab}{dtoProperty.PropertySymbol.Name} = entity.{dtoProperty.PropertySymbol.Name} == null ? null : {dtoProperty.PropertySymbol.Name}Response.MapFrom(entity.{dtoProperty.PropertySymbol.Name}),");
                    }
                }
                else
                {
                    sb.AppendLine($"{tab}{tab}{tab}{tab}{dtoProperty.PropertySymbol.Name} = entity.{dtoProperty.PropertySymbol.Name},");
                }

            }
            sb.AppendLine($"{tab}{tab}{tab}}};");
            sb.AppendLine($"{tab}{tab}}}");
            return sb.ToString();
        }
        private static string GenerateMapFromGeneratedDtoString(List<DtoPropertySymbol> dtoProperties, string generatedClassName, string entityName, bool includeId, bool includeDomainEntities)
        {
            var sb = new StringBuilder();
            var tab = TabSpacing;
            sb.AppendLine($"{tab}{tab}public static {entityName} MapFrom({generatedClassName} dto)");
            sb.AppendLine($"{tab}{tab}{{");
            sb.AppendLine($"{tab}{tab}{tab}return new {entityName}()");
            sb.AppendLine($"{tab}{tab}{tab}{{");

            if (includeId)
            {
                sb.AppendLine($"{tab}{tab}{tab}{tab}Id = dto.Id,");
            }

            foreach (var dtoProperty in dtoProperties)
            {
                if (dtoProperty.IsPropertyADomainEntity)
                {
                    if (includeDomainEntities)
                    {
                        sb.AppendLine($"{tab}{tab}{tab}{tab}{dtoProperty.PropertySymbol.Name} = dto.{dtoProperty.PropertySymbol.Name} == null ? null : {dtoProperty.PropertySymbol.Name}Response.MapFrom(dto.{dtoProperty.PropertySymbol.Name}),");
                    }
                }
                else
                {
                    sb.AppendLine($"{tab}{tab}{tab}{tab}{dtoProperty.PropertySymbol.Name} = dto.{dtoProperty.PropertySymbol.Name},");
                }

            }

            sb.AppendLine($"{tab}{tab}{tab}}};");
            sb.AppendLine($"{tab}{tab}}}");
            return sb.ToString();
        }
        private static string GenerateUsingString(INamedTypeSymbol entitySymbol, List<DtoPropertySymbol> dtoProperties)
        {
            var sb = new StringBuilder();
            sb.AppendLine($"using Kernel.Data.Mapping;"); //This is where our IMap<> interface lives.
            sb.AppendLine($"using Kernel.Data.Entities;"); //This is where our BaseEntity and base entity interfaces lives.
            sb.AppendLine($"using {entitySymbol.ContainingNamespace.ToDisplayString()};");
            foreach (var propertyNamespace in dtoProperties.Select(s => s.PropertyNamespace).Distinct())
            {
                sb.AppendLine($"using {propertyNamespace};");
            }
            return sb.ToString();
        }


        private static string TabSpacing => "    ";
        private static string Namespace => "GeneratedDtos";
        private static string GetResponseClassName(string entityName)
        {
            return $"{entityName}Response";
        }
        private static string GetFullResponseClassName(string entityName)
        {
            return $"{entityName}FullResponse";
        }
        private static string GetCreateRequestClassName(string entityName)
        {
            return $"{entityName}CreateRequest";
        }
        private static string GetUpdateRequestClassName(string entityName)
        {
            return $"{entityName}UpdateRequest";
        }
        private static bool IsPropertyADomainEntity(IPropertySymbol property)
        {
            var propertyTypeSymbol = property.Type;
            if (propertyTypeSymbol.IsReferenceType
                && propertyTypeSymbol.TypeKind == TypeKind.Class
                && propertyTypeSymbol.SpecialType == SpecialType.None)
            {
                var attributes = propertyTypeSymbol.GetAttributes();
                if (attributes != null
                    && attributes.Select(s => s.AttributeClass?.Name).Contains(DomainEntityAttributeGenerator.DomainEntityAttributeName))
                {
                    //Here we have some kind of object that need to map, and since that object we're mapping is at least a domainentity, we know we should have
                    //some response object so that we don't leak our entity.
                    return true;
                }
            }

            //Here we're dealing with a regular ol data type, like int, string, bool, etc.
            return false;
        }

    }

    sealed class DtoPropertySymbol()
    {
        public IPropertySymbol PropertySymbol { get; set; }
        public bool IsPropertyADomainEntity { get; set; }
        public string PropertyNamespace { get; set; }
    }
}



