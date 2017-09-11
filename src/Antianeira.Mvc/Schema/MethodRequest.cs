using DotLiquid;

namespace Antianeira.Schema
{
    public class MethodRequest : Drop
    {
        public TypeReference Type { get; set; }

        public override string ToString()
        {
            return Type.WriteToString();
        }
    }
}
