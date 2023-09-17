using System.IO;

namespace Tiptup300.Slaam.PlayerProfiles;

public class XnaContentWriter
{
   private BinaryWriter _writer;

   private readonly ProfileFileVersion _profileFileVersion;

   public XnaContentWriter(ProfileFileVersion profileFileVersion)
   {
      _profileFileVersion = profileFileVersion;

   }

   public void Initialize(string filename)
   {
      string filePath = Path.Combine(Directory.GetCurrentDirectory(), filename);
      _writer = new BinaryWriter(File.Create(filePath));
      _writer.Write(_profileFileVersion.Version);
   }

   public void Write(string str)
   {
      _writer.Write(str);
   }

   public void Write(int val)
   {
      _writer.Write(val);
   }

   public void Write(bool val)
   {
      _writer.Write(val);
   }

   public void Close()
   {
      _writer.Close();
   }
}
