using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MechatronikaCool.HtmlParsers
{
    public static class PageContentLoader
    {
        private static readonly string iTag = "</i>";
        private static readonly string pTag = "<p>";

        public static string GetPageContent(string url)
        {
            using (WebClient client = new WebClient())
            {
                return client.DownloadString(url);
            }
        }

        public static List<string> GetTitles(string htmlCode)
        {
            var result = new List<string>();

            var matches = Regex.Matches(htmlCode, "<h3>.+</h3>");
            foreach (var match in matches)
            {
                var bytes = Encoding.Default.GetBytes(match.ToString());
                var titleEncoded = Encoding.UTF8.GetString(bytes);

                if (titleEncoded.Contains(iTag))
                {
                    var startIndex = titleEncoded.IndexOf(iTag, StringComparison.Ordinal) + iTag.Length;
                    var endIndex = titleEncoded.LastIndexOf("<", StringComparison.Ordinal);

                    titleEncoded = titleEncoded.Substring(startIndex, endIndex - startIndex);
                }
                else
                {
                    var startIndex = titleEncoded.IndexOf(">", StringComparison.Ordinal) + 1;
                    var endIndex = titleEncoded.LastIndexOf("<", StringComparison.Ordinal);

                    titleEncoded = titleEncoded.Substring(startIndex, endIndex - startIndex);
                }

                System.Diagnostics.Debug.WriteLine(titleEncoded);
                if(titleEncoded != "Spolupracujeme")
                    result.Add(titleEncoded);
            }

            return result;
        }

        public static List<string> GetSpans(string htmlCode)
        {
            var result = new List<string>();

            var matches = Regex.Matches(htmlCode, "<span class=\"odstavec\">.+</span>");
            foreach (var match in matches)
            {
                var bytes = Encoding.Default.GetBytes(match.ToString());
                var spanEncoded = Encoding.UTF8.GetString(bytes);

                var startIndex = spanEncoded.IndexOf(">", StringComparison.Ordinal) + 1;
                var endIndex = spanEncoded.LastIndexOf("<", StringComparison.Ordinal);

                spanEncoded = spanEncoded.Substring(startIndex, endIndex - startIndex);
                result.Add(spanEncoded);

                System.Diagnostics.Debug.WriteLine(spanEncoded);
            }

            return result;
        }

        public static List<string> GetArticles(string htmlCode)
        {
            var result = new List<string>();
            
            htmlCode = Encoding.UTF8.GetString(Encoding.Default.GetBytes(htmlCode));
            htmlCode = htmlCode.Replace("\n", "");
            var matches = htmlCode.Split(new[] {"<h3>"}, StringSplitOptions.RemoveEmptyEntries);//.Where(x => x.StartsWith("p>")).ToArray();

            foreach (var match in matches)
            {
                if (!match.Contains(pTag)) continue;

                var startIndex = match.IndexOf(pTag, StringComparison.Ordinal) + 3;
                var endIndex = match.LastIndexOf("</p>", StringComparison.Ordinal);

                result.Add(
                    match.Substring(startIndex, endIndex - startIndex).Replace(pTag, "").Replace("</p>", "").Trim());

                System.Diagnostics.Debug.WriteLine(result.Last());
            }

            return result;
        }
    }
}
