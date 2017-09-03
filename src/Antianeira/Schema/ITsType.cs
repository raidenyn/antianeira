using JetBrains.Annotations;

namespace Antianeira.Schema
{
    public interface ITsType
    {
        [NotNull]
        string Name { get; }
    }
}
