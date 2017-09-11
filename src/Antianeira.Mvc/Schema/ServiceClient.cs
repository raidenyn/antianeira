using System.Collections.Generic;

namespace Antianeira.Schema
{
    public class ServiceClient: Class
    {
        public ServiceClient(string name) : base(name)
        { }

        public ICollection<Method> Methods { get; } = new List<Method>();
    }
}
