using JetBrains.Annotations;
using Antianeira.Utils;
using DotLiquid;

namespace Antianeira.Schema
{
    public class Definitions: Drop
    {
        [NotNull]
        public Repository<Class> Classes { get; } = new Repository<Class>();

        [NotNull]
        public Repository<Interface> Interfaces { get; } = new Repository<Interface>();

        [NotNull]
        public Repository<Enum> Enums { get; } = new Repository<Enum>();

        [NotNull]
        public Repository<TypeDefinition> Types { get; } = new Repository<TypeDefinition>();
    }
}
