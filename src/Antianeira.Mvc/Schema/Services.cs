using Antianeira.Utils;
using JetBrains.Annotations;

namespace Antianeira.Schema
{
    public class Services
    {
        [NotNull]
        public Definitions Definitions { get; } = new Definitions();

        [NotNull]
        public Repository<ServiceClient> Clients { get; } = new Repository<ServiceClient>();
    }
}
