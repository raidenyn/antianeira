using JetBrains.Annotations;
using Antianeira.Utils;
using DotLiquid;
using Antianeira.Schema.Api;

namespace Antianeira.Schema
{
    public class Definitions: Drop
    {
        [NotNull]
        public ClassRepository Classes { get; set; } = new ClassRepository();

        [NotNull]
        public InterfaceRepository Interfaces { get; set; } = new InterfaceRepository();

        [NotNull]
        public EnumRepository Enums { get; set; } = new EnumRepository();

        [NotNull]
        public TypeRepository Types { get; set; } = new TypeRepository();

        [NotNull]
        public ServiceClientRepository ServiceClients { get; set; } = new ServiceClientRepository();
    }

    public class ClassRepository : Repository<Class>
    {
    }

    public class InterfaceRepository : Repository<Interface>
    {
    }

    public class EnumRepository : Repository<Enum>
    {
    }

    public class TypeRepository : Repository<TypeDefinition>
    {
    }

    public class ServiceClientRepository : Repository<ServiceClient>
    {
    }
}
