using System;
using System.Collections.Generic;
using System.IO;
using Scriban;
using Xunit;

namespace ThisAssembly.Tests
{
    public class Rendering
    {
        public void Render()
        {
            var template = Template.Parse(File.ReadAllText("CSharp.sbntxt"));
            var model = new Model(new ResourceArea("Strings", "")
            {
                Values =
                {
                    new ResourceValue("Foo", "Hello {0}, {1} :)")
                    {
                        Format = { "0", "1" }
                    },
                    new ResourceValue("Bar", "Bye {0}")
                    {
                        Format = { "0" }
                    },
                },
                NestedAreas =
                {
                    new ResourceArea("Constants", "Strings_")
                    {
                        Values =
                        {
                            new ResourceValue("Baz", "Yay")
                        }
                    }
                }
            }, "This");

            Console.WriteLine(template.Render(model, member => member.Name));
        }
    }
}
