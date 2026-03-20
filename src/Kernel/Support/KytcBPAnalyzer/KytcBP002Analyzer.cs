// KytcBP002Analyzer.cs is part of the Boilerplate kernel, modify at your own risk.
// You can get updates from the BP repository.

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
    public class KytcBP002Analyzer : DiagnosticAnalyzer
    {
        private static readonly DiagnosticDescriptor Rule = KytcBPDiagnosticDescriptors.KYTCBP002;

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

            if (namedTypeSymbol.TypeKind == TypeKind.Class
                && namedTypeSymbol.BaseType != null
                && namedTypeSymbol.BaseType.Name == "BaseEntity")
            {
                var interfaces = namedTypeSymbol.Interfaces;
                var expectedInterfaceName = $"I{namedTypeSymbol.Name}Fields";
                if (interfaces.IsDefaultOrEmpty || !interfaces.Any(p => p.Name == expectedInterfaceName))
                {
                    var diagnostic = Diagnostic.Create(Rule, namedTypeSymbol.Locations[0], namedTypeSymbol.Name);

                    context.ReportDiagnostic(diagnostic);
                }
            }

        }
    }
}
