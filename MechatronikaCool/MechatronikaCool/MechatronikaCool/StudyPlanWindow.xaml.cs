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
            plan =  new StudyPlanLoader().GetStudyPlanTable();

            CreateDataGrid();
        }

        private void CreateDataGrid()
        {
            CreateColumns();
            LoadGridData();
        }

        private void CreateColumns()
        {
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

        private void LoadGridData()
        {
            var rows = plan.Plan[plan.Plan.Keys.ToArray()[2]].TableRows.Where(x => !x.IsHeaderRow);

            foreach (var row in rows)
            {
                var dgRow = new DataGridRow();
                //dgRow.Height = 200;
                //dgRow.MaxWidth = 100;
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
    }
}
