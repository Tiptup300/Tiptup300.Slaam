using System.Collections.Generic;

namespace SlaamMono.Menus.Credits
{
    public struct CreditsListing
    {
        public string Name;
        public List<string> Credits;
        public CreditsListing(string name, List<string> credits)
        {
            Name = name;
            Credits = credits;
        }
    }

}