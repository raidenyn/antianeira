using System;
using System.Collections.Generic;
using DotLiquid;
using JetBrains.Annotations;

namespace Antianeira.Schema
{
    public class Service: Drop
    {
        public Service(string name)
        {
            Name = name;
        }

        [CanBeNull]
        public Type SourceClass { get; set; }

        [CanBeNull]
        public Comment Comment { get; set; }

        [NotNull]
        public string Name { get; set; }

        public ICollection<ServiceClientMethod> Methods { get; } = new List<ServiceClientMethod>();
    }
}
