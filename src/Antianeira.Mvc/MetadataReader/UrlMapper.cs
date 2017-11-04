using System.Reflection;
using Antianeira.Schema.MethodUrl;

namespace Antianeira.MetadataReader
{
    public interface IMethodUrlMapper
    {
        MethodUrl MapMethodUrl(MethodInfo methodInfo);
    }

    public class MethodUrlMapper
    {

    }
}
