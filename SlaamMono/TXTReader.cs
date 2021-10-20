using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;

namespace SlaamMono
{
    public class TXTReader : ContentTypeReader<List<String>>
    {
        protected override List<String> Read(ContentReader input, List<String> existingInstance)
        {
            List<String> list = new List<string>();
            int cnt = input.ReadInt32();

            for (int x = 0; x < cnt; x++)
                list.Add(input.ReadString());

            return list;
        }
    }
}
