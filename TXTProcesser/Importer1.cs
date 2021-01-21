using Microsoft.Xna.Framework.Content.Pipeline;
using System.Collections.Generic;
using System.IO;
using TImport = System.Collections.Generic.List<string>;

namespace TXTProcesser
{
    [ContentImporter(".txt", DisplayName = "TextImporter", DefaultProcessor = "TextImporter")]
    public class Importer1 : ContentImporter<TImport>
    {
        public override TImport Import(string filename, ContentImporterContext context)
        {
            List<string> list = new List<string>(File.ReadAllLines(filename));
            return list;
        }
    }
}
