using HtmlAgilityPack;

namespace PEngine.Web.Helper;

public static class ContentHelper
{
    public static string GetPlaintext(string html, int length)
    {
        HtmlDocument doc = new HtmlDocument();
        doc.LoadHtml(html);

        var text = doc.DocumentNode.InnerText;

        if (length > text.Length)
        {
            length = text.Length;
        }

        return text[0..length];
    }
}