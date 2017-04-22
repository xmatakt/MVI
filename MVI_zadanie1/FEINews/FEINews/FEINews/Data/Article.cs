using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FEINews.Data
{
    public class Article
    {
        public string Title { get; set; }
        public string Category { get; set; }
        public string VideoUrl { get; set; }
        public string ImageUrl { get; set; }
        public string Content
        {
            get { return content; }
            set { ParseContent(value); }
        }
        private string content;


        private void ParseContent(string value)
        {
            value = RemoveParagraphs(value);
            if (GetVideoUrl(value))
                return;

            if (GetImageUrl(value))
                return;

            content = value;
        }

        private string RemoveParagraphs(string text)
        {
            return text.Replace("<p>", "")
                .Replace("</p>", "")
                .Replace("<br />", ""); 
        }

        private bool GetVideoUrl(string text)
        {
            if(text.Contains("<iframe"))
            {
                var splittedValue = text.Split(new[] { "<iframe" }, StringSplitOptions.None);
                VideoUrl = splittedValue[1].Split(' ').FirstOrDefault(x => x.Contains("src")).Replace("src=\"", "").Replace("\"", "");
                content = splittedValue[0];
                return true;
            }

            return false;
        }

        private bool GetImageUrl(string text)
        {
            if (text.Contains("<img src="))
            {
                var startIndex = text.IndexOf("<img src=");
                var endIndex = text.IndexOf("/>");
                var imgText = text.Substring(startIndex, endIndex - startIndex + 2);
                content = text.Replace(imgText,"").Trim();
                ImageUrl = imgText.Replace("<img src=", "").Split(' ')[0].Replace("\"", "");
                return true;
            }

            return false;
        }
    }
}
