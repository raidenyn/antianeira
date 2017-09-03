using System.Collections.Generic;
using JetBrains.Annotations;

namespace Antianeira.Schema
{
    public class Interface : TsType
    {
        public Interface(string name): base(name)
        { }

        [NotNull]
        public ICollection<Interface> Interfaces { get; } = new List<Interface>();

        [NotNull]
        public ICollection<InterfaceProperty> Properties { get; } = new List<InterfaceProperty>();

        [NotNull]
        public ICollection<InterfaceMethod> Methods { get; } = new List<InterfaceMethod>();
    }
}
