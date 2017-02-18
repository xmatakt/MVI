using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using MechatronikaCool.Classes;

namespace MechatronikaCool
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly Dictionary<string,string> fields = ApplicationFields.Fields;
        private int startIndex = 0;
        private int endIndex = 5;
        private const int maxIndex = 10;
        private int maxSpanIndex = 0;
        private int actualSpanIndex = 0;
        private ApplicationField actualField = null;
        private MenuItem oldMenuItem = null;
        private List<MenuItem> menuItems = null;

        public MainWindow()
        {
            menuItems = new List<MenuItem>();

            InitializeComponent();
            CreateMenuItemList();

            spans.TextAlignment = TextAlignment.Justify;

            spanTitle.TextAlignment = TextAlignment.Left;
            spanTitle.FontSize = 15;
            spanTitle.Foreground = new SolidColorBrush(Colors.Black);

            LoadFirstPage();
            AddItemsToMenuItem();
        }

        public void item_Click(Object sender, RoutedEventArgs e)
        {
            var menuItem = ((MenuItem) sender);
            var fieldName = menuItem.Header.ToString();
            menuItem.Background = new SolidColorBrush(Colors.White);
            menuItem.Foreground = new SolidColorBrush(Colors.Black);
            actualField = new ApplicationField(fieldName, fields[fieldName]);
            actualSpanIndex = 0;

            maxSpanIndex = actualField.Spans.Count;

            spans.Text = actualField.Spans[actualSpanIndex];
            spanTitle.Text = actualField.Titles[actualSpanIndex];

            if (oldMenuItem != null)
            {
                oldMenuItem.Background = new SolidColorBrush(Colors.Black);
                oldMenuItem.Foreground = new SolidColorBrush(Colors.White);
            }
                
            oldMenuItem = menuItem;
        }

        private void AddItemsToMenuItem()
        {
            menu.Items.Clear();

            for (var i = startIndex; i < endIndex; i++)
                menu.Items.Add(menuItems[i]);
        }

        private void CreateMenuItemList()
        {
            for (var i = 0; i < maxIndex; i++)
            {
                var item = new MenuItem
                {
                    Header = fields.Keys.ToArray()[i],
                    Margin = new Thickness(5, 0, 5, 0),
                    Height = menu.Height,
                    BorderThickness = new Thickness(0, 0, 0, 1),
                    Foreground = new SolidColorBrush(Colors.White),
                };
                item.Click += item_Click;

                menuItems.Add(item);
            }
        }

        private void moveLeft_Click(object sender, RoutedEventArgs e)
        {
            if (startIndex <= 0) return;

            startIndex--;
            endIndex--;
            AddItemsToMenuItem();
        }

        private void moveRight_Click(object sender, RoutedEventArgs e)
        {
            if (endIndex >= maxIndex) return;

            startIndex++;
            endIndex++;
            AddItemsToMenuItem();
        }

        private void spanLeftButton_Click(object sender, RoutedEventArgs e)
        {
            if (actualSpanIndex <= 0) return;

            actualSpanIndex--;
            spans.Text = actualField.Spans[actualSpanIndex];

            //spanTitle.FontSize = 15;
            //spanTitle.Foreground = new SolidColorBrush(Colors.Red);
            spanTitle.Text = actualField.Titles[actualSpanIndex];
        }

        private void spanRightButton_Click(object sender, RoutedEventArgs e)
        {
            if (actualSpanIndex >= maxSpanIndex - 1) return;

            actualSpanIndex++;
            spans.Text = actualField.Spans[actualSpanIndex];

            //spanTitle.FontSize = 15;
            //spanTitle.Foreground = new SolidColorBrush(Colors.Red);
            spanTitle.Text = actualField.Titles[actualSpanIndex];
        }

        private void LoadFirstPage()
        {
            actualField = new ApplicationField(fields.Keys.ToArray()[0], fields.Values.ToArray()[0]);
            actualSpanIndex = 0;

            maxSpanIndex = actualField.Spans.Count;

            spans.Text = actualField.Spans[actualSpanIndex];
            spanTitle.Text = actualField.Titles[actualSpanIndex];

            oldMenuItem = menuItems[0];
            oldMenuItem.Background = new SolidColorBrush(Colors.White);
            oldMenuItem.Foreground = new SolidColorBrush(Colors.Black);
        }
    }
}
