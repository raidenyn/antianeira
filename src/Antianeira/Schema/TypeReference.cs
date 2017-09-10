using DotLiquid;
using System;
using System.Collections.Generic;

namespace Antianeira.Schema
{
    public abstract class TypeReference : Drop, IWritable
    {
        public abstract string Name { get; }

        public bool IsNullable { get; set; }

        public bool IsOptional { get; set; }

        public void Write(IWriter writer)
        {
            writer.Append(Name);
        }
    }

    public class AnyType : TypeReference
    {
        public override string Name { get; } = "any";
    }

    public class VoidType : TypeReference
    {
        public override string Name { get; } = "void";
    }

    public class BooleanType : TypeReference
    {
        public override string Name => "boolean";
    }

    public class StringType : TypeReference
    {
        public override string Name => "string";
    }

    public class NumberType : TypeReference
    {
        public override string Name => "number";
    }

    public class DateType : TypeReference
    {
        public override string Name => "Date";
    }

    public class ObjectType : TypeReference
    {
        public override string Name => "object";
    }

    public class ArrayType : TypeReference
    {
        public TypeReference Type { get; set; }

        public override string Name => Type.Name + "[]";
    }

    public class DictionaryType : TypeReference
    {
        public TypeReference Key { get; set; }

        public TypeReference Value { get; set; }

        public override string Name => $"{{ [key: {Key.Name}]: {Value.Name} }}";
    }

    public class GenericType : TypeReference
    {
        public GenericParameter GenericParameter { get; set; }

        public override string Name => GenericParameter.Name;
    }

    public class CustomType : TypeReference
    {
        public TsType Type { get; set; }

        public IList<TypeReference> GenericArguments { get; } = new List<TypeReference>();

        public override string Name
        {
            get
            {
                var name = Type.Name;
                if (GenericArguments.Count > 0) {
                    name += $"<{String.Join(", ", GenericArguments)}>";
                }
                return name;
            }
        }
    }
}
