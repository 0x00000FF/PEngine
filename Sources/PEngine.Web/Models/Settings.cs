namespace PEngine.Web.Models
{
    public class Settings
    {
        public string SiteTitle { get; set; }
        public string SiteDescription { get; set; }
        public string SiteRoot { get; set; }

        public string ContentSecurityPolicy { get; set; }
        public string RobotsTxt { get; set; }
        public bool RssEnabled { get; set; }
        
    }
}
