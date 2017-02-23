using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using MechatronikaCool.Classes;

using System.Diagnostics;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using MechatronikaCool.HtmlParsers;

namespace MechatronikaCool
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly Dictionary<string,string> fieldsDictionary = ApplicationFields.Fields;
        private List<ApplicationField> applicationFields;
        private int startIndex = 0;
        private int endIndex = 5;
        private const int maxIndex = 10;
        private int maxSpanIndex = 0;
        private int actualSpanIndex = 0;
        private int actualArticleIndex = 0;
        private ApplicationField actualField = null;
        private MenuItem oldMenuItem = null;
        private readonly List<MenuItem> menuItems = null;

        public MainWindow()
        {
            applicationFields = new List<ApplicationField>();
            menuItems = new List<MenuItem>();

            InitializeComponent();
            CreateMenuItemList();

            spans.TextAlignment = TextAlignment.Justify;
            //articleTextBlock.TextAlignment = TextAlignment.Justify;
            spanTitle.TextAlignment = TextAlignment.Center;
            spanTitle.FontSize = 20;
            spanTitle.Foreground = new SolidColorBrush(Colors.White);
            articleTitle.TextAlignment = TextAlignment.Left;
            articleTextBlock.FontSize = 18;
            articleTextBlock.TextAlignment = TextAlignment.Justify;

            LoadFirstPage();
            AddItemsToMenuItem();
        }

        public void item_Click(Object sender, RoutedEventArgs e)
        {
            var menuItem = ((MenuItem) sender);
            var fieldName = menuItem.Header.ToString();
            menuItem.Background = new SolidColorBrush(Colors.White);
            menuItem.Foreground = new SolidColorBrush(Colors.Black);

            actualField = LoadField(fieldName);
            
            if(actualField == null)
            {
                actualField = new ApplicationField(fieldName, fieldsDictionary[fieldName]);
                applicationFields.Add(actualField);
            }
            actualSpanIndex = 0;
            actualArticleIndex = 0;

            maxSpanIndex = actualField.Spans.Count;

            spans.Text = actualField.Spans[actualSpanIndex];
            spanTitle.Text = actualField.Titles[actualSpanIndex];
            SetArticle();

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
                    Header = fieldsDictionary.Keys.ToArray()[i],
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
            spanTitle.Text = actualField.Titles[actualSpanIndex];
        }

        private void spanRightButton_Click(object sender, RoutedEventArgs e)
        {
            if (actualSpanIndex >= maxSpanIndex - 1) return;

            actualSpanIndex++;
            spans.Text = actualField.Spans[actualSpanIndex];
            spanTitle.Text = actualField.Titles[actualSpanIndex];
        }

        private void LoadFirstPage()
        {
            actualField = new ApplicationField(fieldsDictionary.Keys.ToArray()[0], fieldsDictionary.Values.ToArray()[0]);
            applicationFields.Add(actualField);

            actualSpanIndex = 0;
            maxSpanIndex = actualField.Spans.Count;

            spans.Text = actualField.Spans[actualSpanIndex];
            spanTitle.Text = actualField.Titles[actualSpanIndex];
            articleTitle.Text = actualField.Titles[actualField.Spans.Count];
            articleTextBlock.Text = actualField.Articles[0];

            oldMenuItem = menuItems[0];
            oldMenuItem.Background = new SolidColorBrush(Colors.White);
            oldMenuItem.Foreground = new SolidColorBrush(Colors.Black);
        }

        private ApplicationField LoadField(string fieldName)
        {
            return applicationFields.FirstOrDefault(x => x.FieldName == fieldName);
        }

        private void articleButtonRight_Click(object sender, RoutedEventArgs e)
        {
            if (actualArticleIndex >= actualField.Articles.Count - 1) return;

            actualArticleIndex++;
            SetArticle();
        }

        private void articleButtonLeft_Click(object sender, RoutedEventArgs e)
        {
            if (actualArticleIndex <= 0) return;

            actualArticleIndex--;
            SetArticle();
        }

        private void SetArticle()
        {
            articleTitle.Text = actualField.Titles[actualField.Spans.Count + actualArticleIndex];
            articleTextBlock.Text = actualField.Articles[actualArticleIndex];
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var planWindow = new StudyPlanWindow();
            planWindow.Show();
        }

        private void planButton_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            Mouse.OverrideCursor = Cursors.Hand;
        }

        private void planButton_MouseLeave(object sender, MouseEventArgs e)
        {
            Mouse.OverrideCursor = null;
        }
    }
}
