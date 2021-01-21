using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace SlaamMono
{
    public static class XNAContentManager
    {
        public static bool NeedsDevice = true;

        public static void Initialize()
        {
        }

        public static void Update()
        {
            NeedsDevice = false;

            ProfileManager.Initialize();
            LogHelper.Write("Profile Manager Created;");

        }
    }

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
                LogHelper.Write("\"" + "" + "\" is incorrect version.");
                return true;
            }
            return false;
        }
    }

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

    public class FileMisMatchException : Exception
    {

    }
}
