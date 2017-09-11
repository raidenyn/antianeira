using JetBrains.Annotations;

namespace Antianeira.Schema
{
    public static class DefinitionsExtensions
    {
        [CanBeNull]
        public static TsType FineType([NotNull] this Definitions definitions, [NotNull] string name)
        {
            return definitions.Interfaces.Find(name) ??
                   definitions.Classes.Find(name) ??
                   definitions.Types.Find(name) ??
                   definitions.Enums.Find(name) as TsType;
        }
    }
}
