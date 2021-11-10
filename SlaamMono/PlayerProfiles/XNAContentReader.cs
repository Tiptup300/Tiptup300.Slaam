using SlaamMono.Composition;
using SlaamMono.Library.Logging;
using System.IO;

namespace SlaamMono.PlayerProfiles
{
    public class XnaContentReader
    {
        public bool WasNotFound = false;

        private BinaryReader _reader;

        private readonly ILogger _logger;
        private readonly ProfileFileVersion _profileFileVersion;

        public XnaContentReader(
            ILogger logger,
            ProfileFileVersion profileFileVersion
            )
        {
            _logger = logger;
            _profileFileVersion = profileFileVersion;

        }

        public void Initialize(string filename)
        {
            string filePath = Path.Combine(Directory.GetCurrentDirectory(), filename);
            WasNotFound = !File.Exists(filename);
            _reader = new BinaryReader(File.Open(filename, FileMode.OpenOrCreate));
        }

        public void Close()
        {
            _reader.Close();
        }

        public int ReadInt32()
        {
            return _reader.ReadInt32();
        }

        public string ReadString()
        {
            return _reader.ReadString();
        }

        public bool ReadBool()
        {
            return _reader.ReadBoolean();
        }

        public bool IsWrongVersion()
        {
            bool wrongversion = false;
            byte[] filever = _reader.ReadBytes(4);

            for (int x = 0; x < 4; x++)
            {
                if (filever.Length == 0 || filever[x] != _profileFileVersion.Version[x])
                {
                    wrongversion = true;
                    break;
                }
            }

            if (wrongversion)
            {
                _reader.Close();
                _logger.Log("\"" + "" + "\" is incorrect version.");
                return true;
            }
            return false;
        }
    }
}
