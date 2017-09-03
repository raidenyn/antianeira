using DotLiquid;

namespace Antianeira.Schema
{
    public abstract class PropertyType : Drop
    {
        public abstract string Name { get; }

        public bool IsNullable { get; set; }

        public bool IsOptional { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }

    public class AnyType : PropertyType
    {
        public override string Name { get; } = "any";
    }

    public class VoidType : PropertyType
    {
        public override string Name { get; } = "void";
    }

    public class BooleanType : PropertyType
    {
        public override string Name => "boolean";
    }

    public class StringType : PropertyType
    {
        public override string Name => "string";
    }

    public class NumberType : PropertyType
    {
        public override string Name => "number";
    }

    public class DateType : PropertyType
    {
        public override string Name => "Date";
    }

    public class ObjectType : PropertyType
    {
        public override string Name => "object";
    }

    public class ArrayType : PropertyType
    {
        public PropertyType Type { get; set; }

        public override string Name => Type.Name + "[]";
    }

    public class DictionaryType : PropertyType
    {
        public PropertyType Key { get; set; }

        public PropertyType Value { get; set; }

        public override string Name => $"{{ [key: {Key.Name}]: {Value.Name} }}";
    }

    public class CustomType : PropertyType
    {
        public TsType Type { get; set; }

        public override string Name => Type.Name;
    }
}
