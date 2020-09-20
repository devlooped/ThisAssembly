using Xunit;

namespace ThisAssemblyTests
{
    public class StringsTests
    {
        [Fact]
        public void CanRenderNamedArguments()
            => Assert.NotNull(ThisAssembly.Strings.Named("hello", "world"));

        [Fact]
        public void CanRenderIndexedArguments()
            => Assert.NotNull(ThisAssembly.Strings.Indexed("hello", "world"));
    }
}
