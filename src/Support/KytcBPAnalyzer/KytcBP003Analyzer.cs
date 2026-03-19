using System.Collections.Immutable;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;


//TODO: Put this note somewhere
//NOTE: to debug, set your startup as the CodeGenerator project, and hit f5
//TODO: Document all this.
namespace KytcBPAnalyzer
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class KytcBP003Analyzer : DiagnosticAnalyzer
    {
        private static readonly DiagnosticDescriptor Rule = KytcBPDiagnosticDescriptors.KYTCBP003;

        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics { get { return ImmutableArray.Create(Rule); } }

        public override void Initialize(AnalysisContext context)
        {
            context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
            context.EnableConcurrentExecution();
            context.RegisterSymbolAction(AnalyzeSymbol, SymbolKind.NamedType);
        }

        private static void AnalyzeSymbol(SymbolAnalysisContext context)
        {
            var namedTypeSymbol = (INamedTypeSymbol)context.Symbol;

            if (namedTypeSymbol.TypeKind == TypeKind.Interface
                && namedTypeSymbol.Name.EndsWith("Fields"))
            {
                var attributes = namedTypeSymbol.GetAttributes();
                if (attributes == null || !attributes.Any(p => p.AttributeClass.Name == "DomainEntityFieldsAttribute"))
                {
                    var diagnostic = Diagnostic.Create(Rule, namedTypeSymbol.Locations[0], namedTypeSymbol.Name);

                    context.ReportDiagnostic(diagnostic);
                }
            }
        }
    }
}
