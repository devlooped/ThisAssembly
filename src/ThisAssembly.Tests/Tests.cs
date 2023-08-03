using System.Diagnostics.CodeAnalysis;
using System.IO;
using Xunit;
using Xunit.Abstractions;

[assembly: SuppressMessage("SponsorLink", "SL04")]

namespace ThisAssemblyTests;

public record class Tests(ITestOutputHelper Output)
{
    [Fact]
    public void CanReadResourceFile()
        => Assert.NotNull(ResourceFile.Load("Resources.resx", "Strings"));

    [Fact]
    public void CanUseInfo()
        => Assert.Equal("ThisAssembly.Tests", ThisAssembly.Info.Title);

    [Fact]
    public void CanUseInfoDescription()
        => Assert.Equal(
            """
            A Description
                  with a newline and
                  * Some "things" with quotes
                  // Some comments too.
            """.ReplaceLineEndings(), ThisAssembly.Info.Description.ReplaceLineEndings());

    [Fact]
    public void CanUseConstants()
        => Assert.Equal("Baz", ThisAssembly.Constants.Foo.Bar);

    [Fact]
    public void CanUseFileConstants()
        => Assert.Equal(ThisAssembly.Constants.Content.Docs.License, Path.Combine("Content", "Docs", "License.md"));

    [Fact]
    public void CanUseFileConstantInvalidIdentifier()
        => Assert.Equal(ThisAssembly.Constants.Content.Docs._12._Readme_copy_, Path.Combine("Content", "Docs", "12. Readme (copy).txt"));

    [Fact]
    public void CanUseFileConstantLinkedFile()
        => Assert.Equal(ThisAssembly.Constants.Included.Readme, Path.Combine("Included", "Readme.txt"));

    [Fact]
    public void CanUseMetadata()
        => Assert.Equal("Bar", ThisAssembly.Metadata.Foo);

    [Fact]
    public void CanUseProjectProperty()
        => Assert.Equal("Bar", ThisAssembly.Project.Foo);

    [Fact]
    public void CanUseStringsNamedArguments()
        => Assert.NotNull(ThisAssembly.Strings.Named("hello", "world"));

    [Fact]
    public void CanUseStringsIndexedArguments()
        => Assert.NotNull(ThisAssembly.Strings.Indexed("hello", "world"));

    [Fact]
    public void CanUseStringResource()
        => Assert.Equal("Value", ThisAssembly.Strings.Foo.Bar.Baz);

    [Fact]
    public void CanUseTextResource()
        => Assert.NotNull(ThisAssembly.Resources.Content.Styles.Custom.Text);

    [Fact]
    public void CanUseByteResource()
        => Assert.NotNull(ThisAssembly.Resources.Content.Styles.Main.GetBytes());

    [Fact]
    public void CanUseSameNameDifferentExtensions()
        => Assert.NotNull(ThisAssembly.Resources.Content.Swagger.swagger_ui.css.GetBytes());

    [Fact]
    public void CanUseFileInvalidCharacters()
        => Assert.NotNull(ThisAssembly.Resources.webhook_data.Text);

    [Fact]
    public void CanUseGitConstants()
        => Assert.NotEmpty(ThisAssembly.Git.Commit);

    [Fact]
    public void CanUseGitBranchConstants()
    {
        Assert.NotEmpty(ThisAssembly.Git.Branch);
        Output.WriteLine(ThisAssembly.Git.Branch);
    }
}
