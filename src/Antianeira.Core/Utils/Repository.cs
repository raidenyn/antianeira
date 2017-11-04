using JetBrains.Annotations;
using System.Collections.Generic;
using System.Collections;
using System;
using Antianeira.Schema;

namespace Antianeira.Utils
{
    public class Repository<T> : IEnumerable<T>
        where T: class
    {
        private readonly Dictionary<string, T> _structures = new Dictionary<string, T>();

        [NotNull]
        public T GetOrCreate([NotNull] string name, [NotNull] Func<T> creator)
        {
            if (!_structures.TryGetValue(name, out var structure))
            {
                _structures.Add(name, null);

                structure = creator();

                _structures[name] = structure;
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
            if (structure == null)
            {
                throw new KeyNotFoundException(name);
            }

            return structure;
        }

        [NotNull]
        public static IEnumerable<TItem> AppendList<TItem, TSource>(
            [NotNull] this Repository<TItem> repo,
            [NotNull, ItemNotNull] IEnumerable<TSource> sources,
            [NotNull] Func<TSource, string> namer,
            [NotNull] Func<string, TSource, TItem> creator)
            where TItem : TsType
        {
            foreach (var source in sources) {
                var name = namer(source);
                yield return repo.GetOrCreate(name, () => creator(name, source));
            }
        }
    }
}
