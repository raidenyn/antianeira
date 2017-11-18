using Antianeira.Schema;
using System;
using System.Reflection;

namespace Antianeira.MetadataReader.Comments
{
    public interface ICommentsProvider
    {
        Comment GetComment(PropertyInfo property);

        Comment GetComment(Type type);

        Comment GetComment(FieldInfo field);

        Comment GetComment(MethodInfo method);
    }
}
