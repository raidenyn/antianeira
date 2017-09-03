using JetBrains.Annotations;
using System.Collections.Generic;
using System.Collections;
using System;
using Antianeira.Schema;
using System.Reflection;

namespace Antianeira.Utils
{
    public class Repository<T> : IEnumerable<T>
        where T: TsType
    {
        private readonly Dictionary<string, T> _structures = new Dictionary<string, T>();

        [NotNull]
        public T GetOrCreate([NotNull]string name, [NotNull, ItemNotNull] Func<T> creator)
        {
            if (!_structures.TryGetValue(name, out var structure))
            {
                structure = creator();
                _structures.Add(name, structure);
            }

            return structure;
        }

        [CanBeNull]
        public T Find([NotNull] string name)
        {
            if (!_structures.TryGetValue(name, out var structure))
            {
                return null;
            }

            return structure;
        }

        public IEnumerator<T> GetEnumerator()
        {
            return _structures.Values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }

    public static class RepositoryExtensions {
        [NotNull]
        public static T Get<T>(this Repository<T> repo, [NotNull] string name)
            where T : TsType
        {
            var structure = repo.Find(name);
            if (EqualityComparer<T>.Default.Equals(structure, default(T)))
            {
                throw new KeyNotFoundException(name);
            }

            return structure;
        }

        [NotNull]
        public static IEnumerable<T> AppendList<T>(this Repository<T> repo, [NotNull] IEnumerable<Type> types, [NotNull, ItemNotNull] Func<Type, T> creator)
            where T : TsType
        {
            foreach (var type in types) {
                yield return repo.GetOrCreate(type.Name, () => creator(type));
            }
        }
    }
}
