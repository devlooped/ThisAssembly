﻿extern alias Analyzer;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Analyzer::Devlooped.Sponsors;
using Devlooped.Sponsors;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.CodeAnalysis.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Xunit;

namespace Tests;

public class AnalyzerTests : IDisposable
{
    static readonly SponsorableManifest sponsorable = new(
        new Uri("https://sponsorlink.devlooped.com"),
        [new Uri("https://github.com/sponsors/devlooped"), new Uri("https://github.com/sponsors/kzu")],
        "a82350fb2bae407b3021",
        new JsonWebKey(ThisAssembly.Resources.keys.kzu_key.Text));

    public AnalyzerTests()
    {
        // Simulate being a VS IDE for analyzers to actually run.
        if (Environment.GetEnvironmentVariable("VSAPPIDNAME") == null)
            Environment.SetEnvironmentVariable("VSAPPIDNAME", "test");
    }

    void IDisposable.Dispose()
    {
        if (Environment.GetEnvironmentVariable("VSAPPIDNAME") == "test")
            Environment.SetEnvironmentVariable("VSAPPIDNAME", null);
    }

    [Fact]
    public async Task WhenNoAdditionalFiles_ThenReportsUnknown()
    {
        var compilation = CSharpCompilation.Create("test", [CSharpSyntaxTree.ParseText("//")])
            .WithAnalyzers([new SponsorLinkAnalyzer()]);

        var diagnostics = await compilation.GetAnalyzerDiagnosticsAsync();

        Assert.NotEmpty(diagnostics);

        var diagnostic = diagnostics.Single(x => x.Properties.TryGetValue(nameof(SponsorStatus), out var value));

        Assert.True(diagnostic.Properties.TryGetValue(nameof(SponsorStatus), out var value));
        var status = Enum.Parse<SponsorStatus>(value);

        Assert.Equal(SponsorStatus.Unknown, status);
    }

    [Fact]
    public async Task WhenUnknownAndGrace_ThenDoesNotReport()
    {
        // simulate an analyzer file with the right metadata, which is recent and therefore 
        // within the grace period
        var dll = Path.Combine(GetTempPath(), "FakeAnalyzer.dll");
        File.WriteAllText(dll, "");

        var compilation = CSharpCompilation.Create("test", [CSharpSyntaxTree.ParseText("//")])
            .WithAnalyzers([new SponsorLinkAnalyzer()], new AnalyzerOptions([new AdditionalTextFile(dll)], new TestAnalyzerConfigOptionsProvider(new())
            {
                { "build_metadata.Analyzer.ItemType", "Analyzer" },
                { "build_metadata.Analyzer.NuGetPackageId", "SponsorableLib" }
            }));

        var diagnostics = await compilation.GetAnalyzerDiagnosticsAsync();

        Assert.Empty(diagnostics);
    }

    [Fact]
    public async Task WhenUnknownAndNoGraceOption_ThenReportsUnknown()
    {
        // simulate an analyzer file with the right metadata, which is recent and therefore 
        // within the grace period
        var dll = Path.Combine(GetTempPath(), "FakeAnalyzer.dll");
        File.WriteAllText(dll, "");

        var compilation = CSharpCompilation.Create("test", [CSharpSyntaxTree.ParseText("//")])
            .WithAnalyzers([new SponsorLinkAnalyzer()], new AnalyzerOptions([new AdditionalTextFile(dll)], new TestAnalyzerConfigOptionsProvider(new())
            {
                { "build_property.SponsorLinkNoInstallGrace", "true" },
                { "build_metadata.Analyzer.ItemType", "Analyzer" },
                { "build_metadata.Analyzer.NuGetPackageId", "SponsorableLib" }
            }));

        var diagnostics = await compilation.GetAnalyzerDiagnosticsAsync();

        Assert.NotEmpty(diagnostics);

        var diagnostic = diagnostics.Single(x => x.Properties.TryGetValue(nameof(SponsorStatus), out var value));

        Assert.True(diagnostic.Properties.TryGetValue(nameof(SponsorStatus), out var value));
        var status = Enum.Parse<SponsorStatus>(value);

        Assert.Equal(SponsorStatus.Unknown, status);
    }

    [Fact]
    public async Task WhenUnknownAndGraceExpired_ThenReportsUnknown()
    {
        // simulate an analyzer file with the right metadata, which is recent and therefore 
        // within the grace period
        var dll = Path.Combine(GetTempPath(), "FakeAnalyzer.dll");
        File.WriteAllText(dll, "");
        File.SetLastWriteTimeUtc(dll, DateTime.UtcNow - TimeSpan.FromDays(30));

        var compilation = CSharpCompilation.Create("test", [CSharpSyntaxTree.ParseText("//")])
            .WithAnalyzers([new SponsorLinkAnalyzer()], new AnalyzerOptions([new AdditionalTextFile(dll)], new TestAnalyzerConfigOptionsProvider(new())
            {
                { "build_metadata.Analyzer.ItemType", "Analyzer" },
                { "build_metadata.Analyzer.NuGetPackageId", "SponsorableLib" }
            }));

        var diagnostics = await compilation.GetAnalyzerDiagnosticsAsync();

        Assert.NotEmpty(diagnostics);

        var diagnostic = diagnostics.Single(x => x.Properties.TryGetValue(nameof(SponsorStatus), out var value));

        Assert.True(diagnostic.Properties.TryGetValue(nameof(SponsorStatus), out var value));
        var status = Enum.Parse<SponsorStatus>(value);

        Assert.Equal(SponsorStatus.Unknown, status);
    }

    [Fact]
    public async Task WhenSponsoring_ThenReportsSponsor()
    {
        var sponsor = sponsorable.Sign([], expiration: TimeSpan.FromMinutes(5));
        var jwt = Path.Combine(GetTempPath(), "kzu.jwt");
        File.WriteAllText(jwt, sponsor, Encoding.UTF8);

        var compilation = CSharpCompilation.Create("test", [CSharpSyntaxTree.ParseText("//")])
            .WithAnalyzers([new SponsorLinkAnalyzer()], new AnalyzerOptions([new AdditionalTextFile(jwt)], new TestAnalyzerConfigOptionsProvider(new())
            {
                { "build_metadata.SponsorManifest.ItemType", "SponsorManifest" }
            }));

        var diagnostics = await compilation.GetAnalyzerDiagnosticsAsync();

        Assert.NotEmpty(diagnostics);

        var diagnostic = diagnostics.Single(x => x.Properties.TryGetValue(nameof(SponsorStatus), out var value));

        Assert.True(diagnostic.Properties.TryGetValue(nameof(SponsorStatus), out var value));
        var status = Enum.Parse<SponsorStatus>(value);

        Assert.Equal(SponsorStatus.Sponsor, status);
    }

    [Fact]
    public async Task WhenMultipleAnalyzers_ThenReportsOnce()
    {
        var compilation = CSharpCompilation.Create("test", [CSharpSyntaxTree.ParseText("//")])
            .WithAnalyzers([new SponsorLinkAnalyzer(), new SponsorLinkAnalyzer()]);

        var diagnostics = await compilation.GetAnalyzerDiagnosticsAsync();

        Assert.NotEmpty(diagnostics);

        var diagnostic = diagnostics.Single(x => x.Properties.TryGetValue(nameof(SponsorStatus), out var value));

        Assert.True(diagnostic.Properties.TryGetValue(nameof(SponsorStatus), out var value));
        var status = Enum.Parse<SponsorStatus>(value);

        Assert.Equal(SponsorStatus.Unknown, status);
    }


    string GetTempPath([CallerMemberName] string? test = default)
    {
        var path = Path.Combine(Path.GetTempPath(), test ?? nameof(AnalyzerTests));
        Directory.CreateDirectory(path);
        return path;
    }

    class AdditionalTextFile(string path) : AdditionalText
    {
        public override string Path => path;
        public override SourceText GetText(CancellationToken cancellationToken) => SourceText.From(File.ReadAllText(Path), Encoding.UTF8);
    }

    class TestAnalyzerConfigOptionsProvider(Dictionary<string, string> options) : AnalyzerConfigOptionsProvider, IDictionary<string, string>
    {
        AnalyzerConfigOptions analyzerOptions = new TestAnalyzerConfigOptions(options);
        public override AnalyzerConfigOptions GetOptions(SyntaxTree tree) => analyzerOptions;

        public override AnalyzerConfigOptions GetOptions(AdditionalText textFile) => analyzerOptions;
        public void Add(string key, string value) => options.Add(key, value);
        public bool ContainsKey(string key) => options.ContainsKey(key);
        public bool Remove(string key) => options.Remove(key);
        public bool TryGetValue(string key, [MaybeNullWhen(false)] out string value) => options.TryGetValue(key, out value);
        public void Add(KeyValuePair<string, string> item) => ((ICollection<KeyValuePair<string, string>>)options).Add(item);
        public void Clear() => ((ICollection<KeyValuePair<string, string>>)options).Clear();
        public bool Contains(KeyValuePair<string, string> item) => ((ICollection<KeyValuePair<string, string>>)options).Contains(item);
        public void CopyTo(KeyValuePair<string, string>[] array, int arrayIndex) => ((ICollection<KeyValuePair<string, string>>)options).CopyTo(array, arrayIndex);
        public bool Remove(KeyValuePair<string, string> item) => ((ICollection<KeyValuePair<string, string>>)options).Remove(item);
        public IEnumerator<KeyValuePair<string, string>> GetEnumerator() => ((IEnumerable<KeyValuePair<string, string>>)options).GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable)options).GetEnumerator();
        public override AnalyzerConfigOptions GlobalOptions => analyzerOptions;
        public ICollection<string> Keys => options.Keys;
        public ICollection<string> Values => options.Values;
        public int Count => ((ICollection<KeyValuePair<string, string>>)options).Count;
        public bool IsReadOnly => ((ICollection<KeyValuePair<string, string>>)options).IsReadOnly;
        public string this[string key] { get => options[key]; set => options[key] = value; }

        class TestAnalyzerConfigOptions(Dictionary<string, string> options) : AnalyzerConfigOptions
        {
            public override bool TryGetValue(string key, out string value) => options.TryGetValue(key, out value);
        }
    }
}
