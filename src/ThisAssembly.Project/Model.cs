﻿using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace ThisAssembly;

public class Model
{
    public Model(IEnumerable<KeyValuePair<string, string>> properties, string? ns)
        => (Properties, Namespace)
        = (properties.ToList(), ns);

    public string? Namespace { get; }
    public bool RawStrings { get; set; } = false;
    public string Version => Assembly.GetExecutingAssembly().GetName().Version.ToString(3);

    public List<KeyValuePair<string, string>> Properties { get; }
}
