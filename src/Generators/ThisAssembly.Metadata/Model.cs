﻿using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace ThisAssembly
{
    public class Model
    {
        public Model(IEnumerable<KeyValuePair<string, string>> metadata) => Metadata = metadata.ToList();

        public string Version => Assembly.GetExecutingAssembly().GetName().Version.ToString(3);

        public List<KeyValuePair<string, string>> Metadata { get; }
    }
}
