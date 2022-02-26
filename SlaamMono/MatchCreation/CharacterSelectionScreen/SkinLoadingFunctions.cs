using Microsoft.Xna.Framework.Graphics;
using SlaamMono.Library.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SlaamMono.MatchCreation
{
    public class SkinLoadingFunctions
    {

        public static Texture2D[] SkinTexture;
        public static List<string> Skins = new List<string>();
        private static bool SkinsLoaded = false;
        private static Random rand = new Random();

        public static string ReturnRandSkin(ILogger logger)
        {
            if (!SkinsLoaded)
            {
                LoadAllSkins(logger);
            }
            return Skins[rand.Next(0, Skins.Count)];
        }

        public static void LoadAllSkins(ILogger logger)
        {
            if (!SkinsLoaded)
            {
                List<string> skins = File.ReadAllLines(Directory.GetCurrentDirectory() + "\\content\\SkinList.txt").ToList();
                for (int x = 0; x < skins.Count; x++)
                {
                    Skins.Add(skins[x]);
                    logger.Log(" - \"" + skins[x] + "\" was added to listing.");
                }
                SkinTexture = new Texture2D[Skins.Count];
                for (int y = 0; y < Skins.Count; y++)
                {
                    SkinTexture[y] = SlaamGame.Content.Load<Texture2D>("content\\skins\\" + Skins[y]);
                    if (!(SkinTexture[y].Width == 250 && SkinTexture[y].Height == 180))
                    {
                        Skins.RemoveAt(y);
                        y--;
                    }
                }
                SkinsLoaded = true;
            }
        }
    }
}
