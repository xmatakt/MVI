using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using FEINews.Data;

namespace FEINews
{
    public partial class MainPage : ContentPage
    {
        private List<Article> articles;
        private List<Office> officeHours;
        private List<Event> events;

        public MainPage()
        {
            articles = new List<Article>();
            officeHours = new List<Office>();
            events = new List<Event>();

            InitializeComponent();
            LoadData();
        }

        private async void LoadData()
        {
            await LoadArticles();
            await LoadOfficeHoursAsync();
            await LoadEventsAsyns();
        }

        private async Task LoadArticles()
        {
            try
            {
                await ArticlesLoader.GetArticlesAsync(articles);
            }
            catch (Exception ex)
            {

            }
        }

        private async Task LoadOfficeHoursAsync()
        {
            await DependencyService.Get<Interfaces.IDbCommunicator>().GetOfficeHoursAsync(officeHours);
        }

        private async Task LoadEventsAsyns()
        {
            await DependencyService.Get<Interfaces.IDbCommunicator>().GetEventsAsync(events);
        }

        private void ArticledButton_Clicked(object sender, EventArgs e)
        {
            if(articles.Count != 0)
            {
                // to show OtherPage and be able to go back
                Navigation.PushAsync(new ArticlePage(articles));
            }
        }

        private void TemplatesButton_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new TemplatesPage()); 
        }

        private void HoursButton_Clicked(object sender, EventArgs e)
        {
            if(officeHours.Count != 0)
                Navigation.PushAsync(new OfficeHoursPage(officeHours));
        }

        private void ExamsButton_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new ExamsPage());
        }

        private void EventsButton_Clicked(object sender, EventArgs e)
        {
            if (events.Count != 0)
                Navigation.PushAsync(new EventsPage(events));
        }
    }
}
