namespace PEngine.Web.Models.ViewModels;

public class HomeVM
{
    public List<Post> Latest = new ();
    public Dictionary<string, List<Post>> Categories = new ();
}