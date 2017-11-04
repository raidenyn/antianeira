using Antianeira.Utils;
using DotLiquid;
using JetBrains.Annotations;

namespace Antianeira.Schema
{
    public class ServicesDefinitions: Drop
    {
        [NotNull]
        public Definitions Definitions { get; } = new Definitions();

        [NotNull]
        public Repository<Service> Services { get; } = new Repository<Service>();
    }
}
