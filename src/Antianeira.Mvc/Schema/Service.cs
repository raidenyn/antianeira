using System;
using System.Collections.Generic;
using DotLiquid;
using JetBrains.Annotations;

namespace Antianeira.Schema
{
    public class Service: Drop
    {
        public Service([NotNull] string name)
        {
            Name = name;
        }

        /// <summary>
        /// Original Source type
        /// </summary>
        [CanBeNull]
        public Type SourceClass { get; set; }

        /// <summary>
        /// Text and human readable service description
        /// </summary>
        [CanBeNull]
        public Comment Comment { get; set; }

        /// <summary>
        /// Unique name of the service
        /// </summary>
        [NotNull]
        public string Name { get; set; }

        /// <summary>
        /// List of service methods
        /// </summary>
        public ICollection<ServiceMethod> Methods { get; } = new List<ServiceMethod>();
    }
}
