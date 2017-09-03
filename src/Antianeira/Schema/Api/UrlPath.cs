using DotLiquid;
using System.Collections.Generic;

namespace Antianeira.Schema.Api
{
    public class MethodUrl : Drop
    {
        public string Path { get; set; }

        public MethodUrlParameters Parameters { get; } = new MethodUrlParameters();
    }

    public class MethodUrlParameters : Drop {
        public TsType Structure { get; set; }
    }
}
