using System;
using System.Globalization;
using Microsoft.CodeAnalysis;

namespace CodeGeneration;
class DiagnosticException : Exception
{
    Diagnostic _diagnostic;

    public DiagnosticException(Diagnostic diagnostic)
    {
        _diagnostic = diagnostic;
    }

    public override string Message => (_diagnostic as IFormattable).ToString(null, CultureInfo.InvariantCulture);

    public void Report(SourceProductionContext context) => context.ReportDiagnostic(_diagnostic);
}
