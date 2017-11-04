using Antianeira.Schema;
using JetBrains.Annotations;
using System.Collections.Generic;
using System.Reflection;

namespace Antianeira.MetadataReader
{
    public class TypeContext
    {
        public TypeContext(Definitions definitions)
        {
            Definitions = definitions;
        }

        [NotNull]
        public Definitions Definitions { get; }
    }

    public class TypeReferenceContext
    {
        public TypeReferenceContext(Definitions definitions)
        {
            Definitions = definitions;
        }

        [NotNull]
        public Definitions Definitions { get; }

        [CanBeNull]
        public IList<GenericParameter> GenericParameters { get; set; }

        [CanBeNull]
        public PropertyInfo PropertyInfo { get; set; }
    }

    public class InterfacePropertyMappingContext
    {
        public InterfacePropertyMappingContext([NotNull] Definitions definitions)
        {
            Definitions = definitions;
        }

        [NotNull]
        public Definitions Definitions { get; }

        [CanBeNull]
        public IList<GenericParameter> GenercisParameters { get; set; }
    }

    public static class ContextsExtensions
    {
        public static TypeReferenceContext GetTypeReferenceContext(this InterfacePropertyMappingContext context, PropertyInfo property = null)
        {
            return new TypeReferenceContext(context.Definitions)
            {
                GenericParameters = context.GenercisParameters,
                PropertyInfo = property
            };
        }

        public static TypeReferenceContext GetTypeReferenceContext(this TypeContext context)
        {
            return new TypeReferenceContext(context.Definitions);
        }

        public static InterfacePropertyMappingContext GetInterfacePropertyMappingContext(this TypeContext context, IList<GenericParameter> generics = null)
        {
            return new InterfacePropertyMappingContext(context.Definitions) {
                GenercisParameters = generics
            };
        }

        public static TypeContext GetTypeContext(this TypeReferenceContext context)
        {
            return new TypeContext(context.Definitions);
        }
    }
}
