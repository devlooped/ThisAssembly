using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Scriban;
using Xunit;
using Xunit.Abstractions;

namespace ThisAssemblyTests;

/// <summary />
public class ScribanTests(ITestOutputHelper Console)
{
    /// <summary />
    [Fact]
    public void CanRenderModel()
    {
        var source =
            """
            {{ func remarks }}
            /// <see cref="{{ url }}"/>
            {{ end }}
            /// <summary>
            /// {{ summary }}
            /// </summary>
            {{ remarks }}
            """;

        var template = Template.Parse(source);
        var output = template.Render(new
        {
            summary = "This is a summary",
            url = "https://github.com/devlooped#sponsorlink"
        });

        Assert.Contains("https://github.com/devlooped#sponsorlink", output);
        Console.WriteLine(output);
    }
}
