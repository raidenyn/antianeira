using System.Collections.Generic;
using JetBrains.Annotations;

namespace Antianeira.Schema
{
    /// <summary>
    /// Type script Class definition
    /// </summary>
    public class Class: TsType
    {
        public Class(string name): base(name)
        { }

        [CanBeNull]
        public Class BaseClass { get; set; }

        public bool IsAbstract { get; set; }

        [NotNull]
        public ICollection<Interface> Interfaces { get; } = new List<Interface>();

        [NotNull]
        public ICollection<ClassProperty> Properties { get; } = new List<ClassProperty>();
    }
}
