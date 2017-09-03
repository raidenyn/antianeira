using DotLiquid;
using JetBrains.Annotations;

namespace Antianeira.Schema
{
    public class ClassProperty : Drop
    {
        public ClassProperty(string name)
        {
            Name = name;
        }

        [NotNull]
        public string Name { get; set; }

        [NotNull]
        public PropertyType Type { get; set; } = new AnyType();

        public PropertyAccessLevel AccessLevel { get; set; }

        public bool IsStatic { get; set; }

        public bool IsAbstract { get; set; }

        [CanBeNull]
        public Comment Comment { get; set; }
    }

    public class InterfaceProperty : Drop
    {
        public InterfaceProperty(string name)
        {
            Name = name;
        }

        [NotNull]
        public string Name { get; set; }

        [NotNull]
        public PropertyType Type { get; set; } = new AnyType();

        [CanBeNull]
        public Comment Comment { get; set; }
    }
}
