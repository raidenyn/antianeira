using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Antianeira.MetadataReader
{
    public interface IExpendTypesProvider
    {
        [NotNull, ItemNotNull]
        IEnumerable<Type> GetExpendTypes([NotNull] Type type);
    }

    public class InterfaceExpendTypesProvider: IExpendTypesProvider
    {
        private readonly ITypeFilterStrategy _typeFilterStrategy;

        public InterfaceExpendTypesProvider(
            ITypeFilterStrategy typeFilterStrategy
        ) {
            _typeFilterStrategy = typeFilterStrategy;
        }

        public IEnumerable<Type> GetExpendTypes(Type type)
        {
            return new[] { type.BaseType }.Concat(type.GetTypeInfo().ImplementedInterfaces)
                                          .Where(basedType => basedType != null)
                                          .Where(_typeFilterStrategy.IsAllowedType);
        }
    }
}
