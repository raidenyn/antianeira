using DotLiquid;
using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;

namespace Antianeira.Schema
{
    public abstract class TypeReference : Drop, IWritable
    {
        [CanBeNull]
        public Type Source { get; set; }

        [NotNull]
        public abstract string Name { get; }

        [CanBeNull]
        public abstract bool? IsSimple { get; }


        public void Write(IWriter writer)
        {
            writer.Append(Name);
        }
    }

    public class VoidType : TypeReference
    {
        public override string Name { get; } = "void";

        public override bool? IsSimple { get; } = true;
    }

    public class AnyType : TypeReference
    {
        public override string Name { get; } = "any";

        public override bool? IsSimple { get; } = null;
    }

    public class BooleanType : TypeReference
    {
        public override string Name => "boolean";

        public override bool? IsSimple { get; } = true;
    }

    public class StringType : TypeReference
    {
        public override string Name => "string";

        public override bool? IsSimple { get; } = true;
    }

    public class NumberType : TypeReference
    {
        public override string Name => "number";

        public override bool? IsSimple { get; } = true;
    }

    public class DateType : TypeReference
    {
        public override string Name => "Date";

        public override bool? IsSimple { get; } = true;
    }

    public class ObjectType : TypeReference
    {
        public override string Name => "object";

        public override bool? IsSimple { get; } = false;
    }

    public class ArrayType : TypeReference
    {
        public ArrayType([NotNull] TypeReference type)
        {
            Type = type;
        }

        [NotNull]
        public TypeReference Type { get; set; }

        public override string Name => Type.Name + "[]";

        public override bool? IsSimple { get; } = false;
    }

    public class DictionaryType : TypeReference
    {
        public TypeReference Key { get; set; }

        public TypeReference Value { get; set; }

        public override bool? IsSimple { get; } = false;

        public override string Name => $"{{ [key: {Key.Name}]: {Value.Name} }}";
    }

    public class GenericType : TypeReference
    {
        public GenericParameter GenericParameter { get; set; }

        public override bool? IsSimple { get; } = null;

        public override string Name => GenericParameter.Name;
    }

    public class CustomType : TypeReference
    {
        public CustomType([NotNull] TsType type)
        {
            Type = type;
        }

        [NotNull]
        public TsType Type { get; set; }

        [NotNull, ItemNotNull]
        public IList<TypeReference> GenericArguments { get; } = new List<TypeReference>();

        public override bool? IsSimple { get; } = false;

        public override string Name
        {
            get
            {
                var name = Type.Name;
                if (GenericArguments.Count > 0) {
                    name += $"<{String.Join(", ", GenericArguments.Select(g=>g.WriteToString()))}>";
                }
                return name;
            }
        }
    }
}
