using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Xunit;

namespace ThisAssemblyTests;

/// <summary />
public class StringsCollisionTests
{
    /// <summary />
    [Theory]
    [InlineData("SomeResource", "SomeResource_Alt", false)]
    [InlineData("SomeResource", "SomeResource_Alt", true)]
    [InlineData("MSG_InstallationFailed", "MSG_InstallationFailed_Alt", false)]
    [InlineData("MSG_InstallationFailed", "MSG_InstallationFailed_Alt", true)]
    public void ReportsDiagnosticWhenResourceNameIsUsedAsBaseName(string name, string nestedName, bool nestedFirst)
    {
        var xml = nestedFirst ? Resx(nestedName, name) : Resx(name, nestedName);

        var area = ResourceFile.LoadText(xml, "Strings");
        var diagnostics = Report(area);

        var diagnostic = Assert.Single(diagnostics);
        var message = diagnostic.GetMessage();

        Assert.Contains("you cannot use a resource name as the base name for another", message, StringComparison.OrdinalIgnoreCase);
        Assert.Contains($"{name}_Title", message, StringComparison.Ordinal);
        Assert.Contains(name, message, StringComparison.Ordinal);
        Assert.Contains(nestedName, message, StringComparison.Ordinal);
    }

    /// <summary />
    [Fact]
    public void DoesNotReportDiagnosticForExistingResourcesResx()
    {
        var area = ResourceFile.Load("Resources.resx", "Strings");

        Assert.Empty(Report(area));
    }

    /// <summary />
    [Theory]
    [InlineData("Foo_Hey")]
    [InlineData("User_InvalidCredentials")]
    public void DoesNotReportDiagnosticForUnderscoreHierarchyWithoutBaseNameClash(string name)
    {
        var area = ResourceFile.LoadText(Resx(name), "Strings");

        Assert.Empty(Report(area));
    }

    static List<Diagnostic> Report(ResourceArea area)
    {
        var diagnostics = new List<Diagnostic>();
        ResourceFile.ReportDiagnostics(area, diagnostics.Add);
        return diagnostics;
    }

    static string Resx(params string[] names) =>
        "<root>" + string.Concat(names.Select(n => $"<data name=\"{n}\"><value>{n}</value></data>")) + "</root>";
}
