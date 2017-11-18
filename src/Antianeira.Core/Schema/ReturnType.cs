namespace Antianeira.Schema
{
    public class ReturnType : TypeReferenceDefinition
    {
        public override void Write(IWriter writer)
        {
            base.Write(writer);

            if (IsOptional)
            {
                writer.Append(" | undefined");
            }
        }
    }
}
