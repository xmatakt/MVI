using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Net;
using Newtonsoft.Json;
using System.Text;
using System.Threading.Tasks;

namespace FEINews.Data
{
    public class JsonArticle
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public Title Title { get; set; }
        public Content Content { get; set; }
        public List<int> Categories { get; set; }
    }

    public class Title
    {
        public string Rendered { get; set; }
    }

    public class Content
    {
        public string Rendered { get; set; }
        public bool @protected { get; set; }
    }

    public static class ArticlesLoader
    {
        private const string jsonUrl = @"http://mechatronika.cool/noviny/wp-json/wp/v2/posts";
        private static HttpClient client = new HttpClient();
        private static ObservableCollection<JsonArticle> jsonArticles;

        private static Dictionary<int, string> categoriesDictionary = new Dictionary<int, string>()
        {
            { 1, "Celebrity"},
            { 8, "Správy zo sveta FEI"},
            { 14, "Technické novinky"},
            { 13, "Rady pre študentov"}
        };

        public static async Task GetArticlesAsync(List<Article> articles)
        {
            var url = @"http://mechatronika.cool/noviny/wp-json/wp/v2/posts";
            var content = await client.GetStringAsync(url);
            var posts = JsonConvert.DeserializeObject<List<JsonArticle>>(content);
            jsonArticles = new ObservableCollection<JsonArticle>(posts);
            try
            {
                foreach (var jsonArticle in jsonArticles)
                {
                    var article = new Article()
                    {
                        Title = jsonArticle.Title.Rendered.Replace("&#8211;", "-"),
                        Content = jsonArticle.Content.Rendered,
                        Category = categoriesDictionary[jsonArticle.Categories.First()]
                    };
                    articles.Add(article);
                }
            }
            catch (Exception ex)
            {
                var tmp = 10;
            }
        }

        public static async Task<System.IO.Stream> DownloadImage(string imageUrl)
        {
            var image = await client.GetStreamAsync(imageUrl);
            return image;
        }
    }
}
