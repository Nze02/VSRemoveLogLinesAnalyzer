using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RemoveLogLinesAnalyzer
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class RemoveLogLines : DiagnosticAnalyzer
    {
        private static DiagnosticDescriptor Rule = new DiagnosticDescriptor(
            "RemoveLogLines",
            "Remove LogLines",
            "Remove LogLines",
            "LogLines",
            DiagnosticSeverity.Warning,
            isEnabledByDefault: true);

        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics { get { return ImmutableArray.Create(Rule); } }

        public override void Initialize(AnalysisContext context)
        {
            context.RegisterSyntaxNodeAction(AnalyzeNode, SyntaxKind.InvocationExpression);
        }

        private void AnalyzeNode(SyntaxNodeAnalysisContext context)
        {
            var invocation = (InvocationExpressionSyntax)context.Node;

            var symbolInfo = context.SemanticModel.GetSymbolInfo(invocation.Expression);

            if(symbolInfo.Symbol is IMethodSymbol methodSymbol)
            {
                if(methodSymbol.Name == "LogLine")
                {
                    var diagnostic = Diagnostic.Create(Rule, invocation.GetLocation());
                    context.ReportDiagnostic(diagnostic);
                }
            }
        }
    }
}
