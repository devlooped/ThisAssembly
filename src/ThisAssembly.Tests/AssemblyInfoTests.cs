using System.Reflection;
using Xunit;

// Define assembly information attributes here, so we can use AssemblyInfoValues
[assembly: AssemblyConfiguration(AssemblyInfoValues.Configuration)]
[assembly: AssemblyCompany(AssemblyInfoValues.Company)]
[assembly: AssemblyCopyright(AssemblyInfoValues.Copyright)]
[assembly: AssemblyTitle(AssemblyInfoValues.Title)]
[assembly: AssemblyDescription(AssemblyInfoValues.Description)]
[assembly: AssemblyProduct(AssemblyInfoValues.Product)]
[assembly: AssemblyVersion(AssemblyInfoValues.Version)]
[assembly: AssemblyInformationalVersion(AssemblyInfoValues.InformationalVersion)]
[assembly: AssemblyFileVersion(AssemblyInfoValues.FileVersion)]

// Single source of truth for assembly information values
static class AssemblyInfoValues
{
    public const string Configuration = "Of course there is a configuration!";
    public const string Company = "Example, Inc.";
    public const string Copyright= "(C) Example. All rights reserved.";
    public const string Title = "Some catchy title";
    public const string Description = "A (possibly lengthy and boring) description.";
    public const string Product = "Our marvellous product";
    public const string Version = "11.22.33";
    public const string InformationalVersion = "11.22.33-preview.44+metadata";
    public const string FileVersion = "11.00.00";
    public const string UserValue = "This value is project-specific.";
}

// User-added constant
partial class ThisAssembly
{
    partial class Info
    {
        public static readonly string UserValue = AssemblyInfoValues.UserValue;
    }
}

namespace ThisAssemblyTests
{
    public static class AssemblyInfoTests
    {
        [Fact]
        public static void AssemblyInfoValues_AreCorrect()
        {
            Assert.Equal(AssemblyInfoValues.Configuration, ThisAssembly.Info.Configuration);
            Assert.Equal(AssemblyInfoValues.Company, ThisAssembly.Info.Company);
            Assert.Equal(AssemblyInfoValues.Copyright, ThisAssembly.Info.Copyright);
            Assert.Equal(AssemblyInfoValues.Title, ThisAssembly.Info.Title);
            Assert.Equal(AssemblyInfoValues.Description, ThisAssembly.Info.Description);
            Assert.Equal(AssemblyInfoValues.Product, ThisAssembly.Info.Product);
            Assert.Equal(AssemblyInfoValues.Version, ThisAssembly.Info.Version);
            Assert.Equal(AssemblyInfoValues.InformationalVersion, ThisAssembly.Info.InformationalVersion);
            Assert.Equal(AssemblyInfoValues.FileVersion, ThisAssembly.Info.FileVersion);
            Assert.Equal(AssemblyInfoValues.UserValue, ThisAssembly.Info.UserValue);
        }
    }
}
