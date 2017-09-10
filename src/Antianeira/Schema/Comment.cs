using DotLiquid;

namespace Antianeira.Schema
{
    public class Comment : Drop, IWritable
    {
        public string Summary { get; set; }

        public void Write(IWriter writer)
        {
            var lines = Summary.Split('\n');

            writer.Append("/*");

            foreach (var line in lines) {
                writer.Append("\n *");
                writer.Append(line);
            }

            writer.Append("\n */\n");
        }
    }
}
