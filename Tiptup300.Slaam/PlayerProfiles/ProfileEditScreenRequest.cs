using ZzziveGameEngine;

namespace SlaamMono.PlayerProfiles
{
   public class ProfileEditScreenRequest : IRequest
   {
      public bool CreateNewProfile { get; set; }
      public int width { get; set; }
      public int height { get; set; }
   }
}
