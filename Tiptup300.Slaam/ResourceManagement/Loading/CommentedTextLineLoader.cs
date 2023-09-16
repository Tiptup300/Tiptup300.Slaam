﻿using SlaamMono.Library.ResourceManagement;

namespace SlaamMono.ResourceManagement.Loading
{
   public class CommentedTextLineLoader : IFileLoader<string[]>
   {
      public object Load(string baseName)
      {
         return File.ReadAllLines(baseName)
             .Where(line => line.StartsWith("//") == false)
             .ToArray();
      }
   }
}
