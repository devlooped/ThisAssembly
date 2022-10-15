using System.IO;
using Xunit;

namespace ThisAssemblyTests
{
    public class Tests
    {
        [Fact]
        public void CanReadResourceFile()
            => Assert.NotNull(ResourceFile.Load("Resources.resx", "Strings"));

        [Fact]
        public void CanUseConstants()
            => Assert.Equal("Baz", ThisAssembly.Constants.Foo.Bar);

        [Fact]
        public void CanUseFileConstants()
            => Assert.Equal(ThisAssembly.Constants.Content.Docs.License, Path.Combine("Content", "Docs", "License.md"));

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
    }
}
