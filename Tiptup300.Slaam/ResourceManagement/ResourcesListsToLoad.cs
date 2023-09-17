namespace Tiptup300.Slaam.ResourceManagement;

public class ResourcesListsToLoad
{
   public string[] TextLists { get; private set; }

   public ResourcesListsToLoad(string[] textLists)
   {
      TextLists = textLists;
   }
}
