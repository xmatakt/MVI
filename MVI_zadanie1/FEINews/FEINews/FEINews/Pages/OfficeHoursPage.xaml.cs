using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FEINews
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class OfficeHoursPage : ContentPage
    {
        List<Data.Office> offices;
        public OfficeHoursPage(List<Data.Office> offices)
        {
            this.offices = offices;

            InitializeComponent();

            OfficeHoursListView.ItemsSource = offices;
        }

        private void TextCell_Tapped(object sender, EventArgs e)
        {
            var textCell = (TextCell)sender;
            var office = offices.Find(x => x.Name == textCell.Text);
            DisplayAlert(office.Name, office.Hours, "OK");
        }
    }
}
