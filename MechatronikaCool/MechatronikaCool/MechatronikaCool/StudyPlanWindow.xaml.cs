using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using MechatronikaCool.Classes;
using MechatronikaCool.HtmlParsers;

namespace MechatronikaCool
{
    /// <summary>
    /// Interaction logic for StudyPlanWindow.xaml
    /// </summary>
    public partial class StudyPlanWindow : Window
    {
        private StudyPlan plan;
        public StudyPlanWindow()
        {
            InitializeComponent();

            periodComboBox.Background = new SolidColorBrush(Color.FromRgb(0, 92, 174));
            periodComboBox.Foreground = Brushes.White;
            periodComboBox.Resources.Add(SystemColors.WindowBrushKey, new SolidColorBrush(Color.FromRgb(0, 92, 174)));

            plan =  new StudyPlanLoader().GetStudyPlanTable();
            
            CreateDataGrid();
            InitializeComboBox();
        }

        private void CreateDataGrid()
        {
            CreateColumns();
            //LoadGridData(GetSelectedPeriod());
        }

        private void InitializeComboBox()
        {
            foreach (var key in plan.Plan.Keys)
            {
                periodComboBox.Items.Add(key);
            }

            periodComboBox.SelectedIndex = 0;
        }

        private void CreateColumns()
        {
            DG.Items.Clear();
            var headerRow = plan.Plan[plan.Plan.Keys.ToArray()[0]].TableRows.First(x => x.IsHeaderRow);

            var counter = 1;
            foreach (var rowItem in headerRow.GetRowData())
            {
                var col = new DataGridTextColumn();
                col.MaxWidth = 400;
                col.ElementStyle = new Style(typeof(TextBlock));
                col.ElementStyle.Setters.Add(new Setter(TextBlock.TextWrappingProperty, TextWrapping.Wrap));
                if (counter != 5)
                    col.ElementStyle.Setters.Add(new Setter(TextBlock.TextAlignmentProperty, TextAlignment.Justify));
                else
                    col.ElementStyle.Setters.Add(new Setter(TextBlock.TextAlignmentProperty, TextAlignment.Center));
                col.ElementStyle.Setters.Add(new Setter(TextBlock.MarginProperty, new Thickness(2,1,1,4)));

                DG.Columns.Add(col);
                col.Binding = new Binding("col" + counter);
                col.Header = rowItem;
                counter++;
            }
        }

        private void LoadGridData(string selectedPeriod)
        {
            DG.Items.Clear();
            var rows = plan.Plan[selectedPeriod].TableRows.Where(x => !x.IsHeaderRow);

            foreach (var row in rows)
            {
                var dgRow = new DataGridRow();
                dgRow.Item = UltraBadClass.ReturnSomething(row.GetRowData());
                if (row.IsCollapsedRow)
                {
                    dgRow.Background = new SolidColorBrush(Colors.CornflowerBlue);
                    DG.Items.Add(dgRow);    
                }
                else
                {
                    if (row.GetRowData()[0] == "Spolu")
                        dgRow.Background = new SolidColorBrush(Colors.CornflowerBlue);
                    else
                        dgRow.Background = new SolidColorBrush(Color.FromRgb(157,206,255));
                    DG.Items.Add(dgRow);
                }
            }
        }

        private void comboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            LoadGridData(GetSelectedPeriod());
        }

        private string GetSelectedPeriod()
        {
            return periodComboBox.Items[periodComboBox.SelectedIndex].ToString();
        }
    }
}
