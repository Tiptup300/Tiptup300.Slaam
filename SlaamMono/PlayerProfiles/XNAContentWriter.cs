using System.IO;

namespace SlaamMono.PlayerProfiles
{
    public class XnaContentWriter
    {
        BinaryWriter writer;

        public XnaContentWriter(string filename)
        {

            filename = Path.Combine(Directory.GetCurrentDirectory(), filename);

            writer = new BinaryWriter(File.Create(filename));

            writer.Write(Program.Version);
        }

        public void Write(string str)
        {
            writer.Write(str);
        }

        public void Write(int val)
        {
            writer.Write(val);
        }

        public void Write(bool val)
        {
            writer.Write(val);
        }

        public void Close()
        {
            writer.Close();
        }
    }
}
