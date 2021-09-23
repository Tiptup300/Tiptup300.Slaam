using System;
using System.IO;

namespace SlaamMono
{
    public class XNAContentWriter
    {
        BinaryWriter writer;

        public XNAContentWriter(string filename)
        {

            filename = Path.Combine(Directory.GetCurrentDirectory(), filename);

            writer = new BinaryWriter(File.Create(filename));

            writer.Write(Program.Version);
        }

        public void Write(string str)
        {
            writer.Write(str);
        }

        public void Write(Int32 val)
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
