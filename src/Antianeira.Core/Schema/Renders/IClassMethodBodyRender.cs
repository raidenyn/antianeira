using JetBrains.Annotations;

namespace Antianeira.Schema.Renders
{
    public interface IClassMethodBodyRender
    {
        void WriteBody([NotNull] IWriter writer, [NotNull] ClassMethod method);
    }
}
