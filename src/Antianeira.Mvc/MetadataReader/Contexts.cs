using Antianeira.Schema;
using JetBrains.Annotations;

namespace Antianeira.MetadataReader
{
    public class MethodContexts
    {
        public MethodContexts([NotNull] ServicesDefinitions services)
        {
            Services = services;
        }

        [NotNull]
        public ServicesDefinitions Services { get; }
    }

    public class MethodResponseContexts
    {
        public MethodResponseContexts([NotNull] ServicesDefinitions services)
        {
            Services = services;
        }

        [NotNull]
        public ServicesDefinitions Services { get; }
    }

    public class MethodParameterContexts
    {
        public MethodParameterContexts([NotNull] ServicesDefinitions services)
        {
            Services = services;
        }

        [NotNull]
        public ServicesDefinitions Services { get; }
    }
}
