using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using FEINews.Data;

namespace FEINews
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ArticlePage : ContentPage
    {
        private List<Article> articles;
        private const string allCategories = "Všetky kategórie";

        public ArticlePage(List<Article> articles)
        {
            InitializeComponent();

            this.articles = articles;
            //ArticlesListView.ItemsSource = articles;
            InitializePicker();
        }

        private void TextCell_Tapped(object sender, EventArgs e)
        {
            var textCell = (TextCell)sender;
            var article = articles.Find(x=> x.Title == textCell.Detail);
            var articlePage = new ArticleContentPage(article);
            Navigation.PushAsync(articlePage);
        }

        private void InitializePicker()
        {
            CategoryPicker.Items.Add(allCategories);
            foreach (var category in articles.Select(x => x.Category).Distinct())
            {
                CategoryPicker.Items.Add(category);
            }
            CategoryPicker.SelectedIndex = 0;
        }

        private void FilterArticles()
        {
            var selectedCategory = CategoryPicker.Items[CategoryPicker.SelectedIndex];

            if(selectedCategory == allCategories)
                ArticlesListView.ItemsSource = articles;
            else
                ArticlesListView.ItemsSource = articles.Where(x => x.Category == selectedCategory).ToList();
        }

        private void CategoryPicker_SelectedIndexChanged(object sender, EventArgs e)
        {
            FilterArticles();
        }
    }
}
