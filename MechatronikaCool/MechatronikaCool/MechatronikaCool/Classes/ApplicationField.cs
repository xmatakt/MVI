using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MechatronikaCool.Classes
{
    class ApplicationField
    {
        public string FieldName { get; set; }
        public List<string> Titles { get; private set; }
        public List<string> Spans { get; private set; }
        public List<string> Articles { get; private set; }

        private readonly string url;

        public ApplicationField(string name, string url)
        {
            FieldName = name;
            this.url = url;

            LoadField();
        }

        private void LoadField()
        {
            var htmlCode = HtmlParsers.PageContentLoader.GetPageContent(url);

            Titles = HtmlParsers.PageContentLoader.GetTitles(htmlCode);
            Spans = HtmlParsers.PageContentLoader.GetSpans(htmlCode);
            Articles = HtmlParsers.PageContentLoader.GetArticles(htmlCode);
        }
    }
}
