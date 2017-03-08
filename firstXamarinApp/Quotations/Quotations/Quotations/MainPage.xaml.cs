using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace Quotations
{
    public partial class MainPage : ContentPage
    {
        private double stepValue;
        private List<string> quotations;
        private int actualIndex = 0;

        public MainPage()
        {
            stepValue = 1.0;
            quotations = new List<string>()
            {
                "Je to tak, jak to je. L.J.",
                "Dobry kod nikdy nie je zly.",
                "Zly kod nikdy nie je dobry.",
                "Hody boli vkoctene."
            };

            InitializeComponent();
            Padding = Device.OnPlatform(
                iOS: new Thickness(20, 30, 20, 20),
                Android: new Thickness(20, 30, 20, 20),
                WinPhone: new Thickness(20, 40, 20, 20)
                );
            quotationText.Text = quotations[actualIndex];
        }

        void OnSliderValueChanged(object sender, ValueChangedEventArgs e)
        {
            var newStep = Math.Round(e.NewValue / stepValue);
            fontSizeSlider.Value = newStep * stepValue;
        }

        void OnLeftButtonClick(object sender, EventArgs e)
        {
            if ((actualIndex - 1) >= 0)
                quotationText.Text = quotations[--actualIndex];
        }

        void OnRightButtonClick(object sender, EventArgs e)
        {
            if ((actualIndex + 1) < quotations.Count)
                quotationText.Text = quotations[++actualIndex];
        }
    }
}
