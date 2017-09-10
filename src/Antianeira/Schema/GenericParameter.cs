namespace Antianeira.Schema
{
    public class GenericParameter: IWritable
    {
        public GenericParameter(string name) {
            Name = name;
        }

        public string Name { get; set; }

        public void Write(IWriter writer)
        {
            writer.Append(Name);
        }
    }
}
