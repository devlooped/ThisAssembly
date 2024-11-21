using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using Xunit;
using Xunit.Abstractions;
//using ThisAssembly = ThisAssemblyTests

[assembly: SuppressMessage("SponsorLink", "SL04")]

namespace ThisAssemblyTests;

/// <summary />
public record class Tests(ITestOutputHelper Output)
{
    /// <summary />
    [Fact]
    public void CanReadResourceFile()
        => Assert.NotNull(ResourceFile.Load("Resources.resx", "Strings"));

    /// <summary />
    [Fact]
    public void CanUseInfo()
        => Assert.Equal("ThisAssembly.Tests", ThisAssembly.Info.Title);

    /// <summary />
    [Fact]
    public void CanUseInfoDescription()
        => Assert.Equal(
            """
            A Description
                  with a newline and
                  * Some "things" with quotes
                  // Some comments too.
            """.ReplaceLineEndings(), ThisAssembly.Info.Description.ReplaceLineEndings());

    /// <summary />
    [Fact]
    public void CanUseMultilineProjectProperty()
        => Assert.Equal(
            """
                  A Description
                  with a newline and
                  * Some "things" with quotes
                  // Some comments too.
            """.ReplaceLineEndings(), ThisAssembly.Project.Multiline.ReplaceLineEndings());

    /// <summary />
    [Fact]
    public void CanUseProjectFullFileContents()
    {
        Assert.NotEmpty(ThisAssembly.Project.ProjectFile);
        Assert.False(ThisAssembly.Project.ProjectFile.StartsWith("|"));
    }

    /// <summary />
    [Fact]
    public void CanUseProjectRepositoryUrl()
    {
        Assert.NotEmpty(ThisAssembly.Project.RepositoryUrl);
    }

    /// <summary />
    [Fact]
    public void CanUseConstants()
        => Assert.Equal("Baz", ThisAssembly.Constants.Foo.Bar);

    /// <summary />
    [Fact]
    public void CanUseTypedIntConstant()
        => Assert.Equal(123, ThisAssembly.Constants.TypedInt);

    /// <summary />
    [Fact]
    public void CanUseTypedInt64Constant()
        => Assert.Equal(123, ThisAssembly.Constants.TypedInt64);

    /// <summary />
    [Fact]
    public void CanUseTypedLongConstant()
        => Assert.Equal(123, ThisAssembly.Constants.TypedLong);

    /// <summary />
    [Fact]
    public void CanUseTypedBoolConstant()
        => Assert.True(ThisAssembly.Constants.TypedBoolean);

    /// <summary />
    [Fact]
    public void CanUseTypedDoubleConstant()
        => Assert.Equal(1.23, ThisAssembly.Constants.TypedDouble);

    /// <summary />
    [Fact]
    public void CanUseTypedTimeSpanStaticProp()
        => Assert.Equal(TimeSpan.FromSeconds(5), ThisAssembly.Constants.TypedTimeSpan);

    /// <summary />
    [Fact]
    public void CanUseFileConstants()
        => Assert.Equal(ThisAssembly.Constants.Content.Docs.License, Path.Combine("Content", "Docs", "License.md"));

    /// <summary />
    [Fact]
    public void CanUseFileConstantInvalidIdentifier()
        => Assert.Equal(ThisAssembly.Constants.Content.Docs._12._Readme_copy_, Path.Combine("Content", "Docs", "12. Readme (copy).txt"));

    /// <summary />
    [Fact]
    public void CanUseFileConstantLinkedFile()
        => Assert.Equal(ThisAssembly.Constants.Included.Readme, Path.Combine("Included", "Readme.txt"));

    /// <summary />
    [Fact]
    public void CanUseMetadata()
        => Assert.Equal("Bar", ThisAssembly.Metadata.Foo);

    /// <summary />
    [Fact]
    public void CanUseHierarchicalMetadata()
        => Assert.Equal("Baz", ThisAssembly.Metadata.Root.Foo.Bar);

    /// <summary />
    [Fact]
    public void CanUseProjectProperty()
        => Assert.Equal("Bar", ThisAssembly.Project.Foo);

    /// <summary />
    [Fact]
    public void CanUseStringsNamedArguments()
        => Assert.NotNull(ThisAssembly.Strings.Named("hello", "world"));

    /// <summary />
    [Fact]
    public void CanUseStringsIndexedArguments()
        => Assert.NotNull(ThisAssembly.Strings.Indexed("hello", "world"));

    /// <summary />
    [Fact]
    public void CanUseStringsNamedFormattedArguments()
        => Assert.Equal("Year 2020, Month 03", ThisAssembly.Strings.WithNamedFormat(new DateTime(2020, 3, 20)));

    /// <summary />
    [Fact]
    public void CanUseStringsIndexedFormattedArguments()
        => Assert.Equal("Year 2020, Month 03", ThisAssembly.Strings.WithIndexedFormat(new DateTime(2020, 3, 20)));

    /// <summary />
    [Fact]
    public void CanUseStringResource()
        => Assert.Equal("Value", ThisAssembly.Strings.Foo.Bar.Baz);

    /// <summary />
    [Fact]
    public void CanUseTextResource()
        => Assert.NotNull(ThisAssembly.Resources.Content.Styles.Custom.Text);

    /// <summary />
    [Fact]
    public void CanUseByteResource()
        => Assert.NotNull(ThisAssembly.Resources.Content.Styles.Main.GetBytes());

    /// <summary />
    [Fact]
    public void CanUseSameNameDifferentExtensions()
        => Assert.NotNull(ThisAssembly.Resources.Content.Swagger.swagger_ui.css.GetBytes());

    /// <summary />
    [Fact]
    public void CanUseFileInvalidCharacters()
        => Assert.NotNull(ThisAssembly.Resources.webhook_data.Text);

    /// <summary />
    [Fact]
    public void CanUseGitConstants()
        => Assert.NotEmpty(ThisAssembly.Git.Commit);

    /// <summary />
    [Fact]
    public void CanUseGitBranchConstants()
    {
        Assert.NotEmpty(ThisAssembly.Git.Branch);
        Output.WriteLine(ThisAssembly.Git.Branch);
    }

    /// <summary />
    [Fact]
    public void CanUseSemicolonsInConstant()
        => Assert.Equal("A;B;C", ThisAssembly.Constants.WithSemiColon);
}
