using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using FEINews.Data;
using System.Net;

namespace FEINews
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ArticleContentPage : ContentPage
    {
        private Article article;
        public ArticleContentPage()
        {
            InitializeComponent();
        }

        public ArticleContentPage(Article article)
        {
            InitializeComponent();
            this.article = article;

            if (article.VideoUrl != null)
                ShowVideoButton.IsVisible = true;

            Title = article.Category;
            ArticleTitleLabel.Text = article.Title;
            ArticleContentLabel.Text = article.Content;

            LoadArticleImage();
        }

        private async void LoadArticleImage()
        {
            if (article.ImageUrl == null)
                return;

            var imageStream = await ArticlesLoader.DownloadImage(article.ImageUrl);
            ArticleImage.Source = ImageSource.FromStream(() => imageStream);
            //ArticleImage.Source = ImageSource.FromUri(new Uri(article.ImageUrl));
        }

        private void ShowVideoButton_Clicked(object sender, EventArgs e)
        {
            if (article.VideoUrl == null)
                return;

            Device.OpenUri(new Uri(article.VideoUrl));
        }
    }
}
