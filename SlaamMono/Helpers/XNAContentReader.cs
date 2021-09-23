using System.IO;

namespace SlaamMono
{
    public class XNAContentReader
    {
        public bool WasNotFound = false;
        BinaryReader reader;

        public XNAContentReader(string filename)
        {
            filename = Path.Combine(Directory.GetCurrentDirectory(), filename);

            WasNotFound = !File.Exists(filename);

            reader = new BinaryReader(File.Open(filename,FileMode.OpenOrCreate));

            
        }

        public void Close()
        {
            reader.Close();
        }

        public int ReadInt32()
        {
            return reader.ReadInt32();
        }

        public string ReadString()
        {
            return reader.ReadString();
        }

        public bool ReadBool()
        {
            return reader.ReadBoolean();
        }

        public bool IsWrongVersion()
        {
            bool wrongversion = false;
            byte[] filever = reader.ReadBytes(4);

            for (int x = 0; x < 4; x++)
            {
                if (filever.Length == 0 || filever[x] != Program.Version[x])
                {
                    wrongversion = true;
                    break;
                }
            }

            if (wrongversion)
            {
                reader.Close();
                TextLogger.Instance.Log("\"" + "" + "\" is incorrect version.");
                return true;
            }
            return false;
        }
    }
}
