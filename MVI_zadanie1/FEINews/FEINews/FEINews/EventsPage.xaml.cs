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
    public partial class EventsPage : ContentPage
    {
        private List<Event> events = null;
        public EventsPage(List<Event> events)
        {
            this.events = events;

            InitializeComponent();

            EventsListView.ItemsSource = events;
        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            var evnt = events.Find(x => x.ID == int.Parse(((Button)sender).Text.Trim()));

            DependencyService.Get<Interfaces.ICalendarEventCreator>().CreateEventForEvent(evnt, Switcher.IsToggled);

            if (Switcher.IsToggled)
                DisplayAlert(evnt.EventTitle, "Event vytvorený", "OK");
        }

        private void EventsListView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            var item = (Event)e.Item;
            DisplayAlert(item.EventTitle, item.ToString(), "OK");
        }

        private void Switcher_Toggled(object sender, ToggledEventArgs e)
        {

        }
    }
}
