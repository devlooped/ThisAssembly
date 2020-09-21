using System;
using System.IO;
using Scriban;

namespace ThisAssemblyTests
{
    public class Rendering
    {
        public void LoadAndRender()
        {
            var root = ResourceFile.Load(@"Resources.resx", "Strings");
            var model = new Model(root, "MyAssembly.Resources");
            var template = Template.Parse(File.ReadAllText("CSharp.sbntxt"));

            Console.WriteLine(template.Render(model, member => member.Name));
        }

        public void Render()
        {
            var template = Template.Parse(File.ReadAllText("CSharp.sbntxt"));
            var model = new Model(new ResourceArea("Strings", "")
            {
                Values =
                {
                    new ResourceValue("Foo", "Hello {first}, {last}. Yay {first} :)")
                    {
                        Format = { "first", "last" }
                    },
                    new ResourceValue("Bar", "Bye {0} and {name}")
                    {
                        Format = { "0", "{name}" }
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
