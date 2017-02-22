using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using MechatronikaCool.Classes;

namespace MechatronikaCool.HtmlParsers
{
    class StudyPlanLoader
    {
        private readonly string url = @"http://www.automobilova-mechatronika.fei.stuba.sk/webstranka/?q=node/97/";
        private string htmlString = null;
        private StudyPlan plan = null;

        public StudyPlanLoader()
        {
            plan = new StudyPlan();

            LoadHtmlString();
            CreatePlanTable();
        }

        public StudyPlan GetStudyPlanTable()
        {
            return plan;
        }

        private void LoadHtmlString()
        {
            using (var client = new WebClient())
            {
                htmlString = client.DownloadString(url);
                htmlString = Encoding.UTF8.GetString(Encoding.Default.GetBytes(htmlString));
            }
        }

        private void CreatePlanTable()
        {
            var h5Split = htmlString.Split(new[] {"<h5>"}, StringSplitOptions.RemoveEmptyEntries)
                .Where(x => x.Contains("</h5>"));

            foreach (var h5 in h5Split)
                LoadPeriods(h5);
        }

        /// <summary>
        /// Perioda: X.rocnik / YZ semester
        /// Metoda predpoklada, ze preberie cast htmlStringu obsahujucu element </h5> (X.rocnik)
        /// a 2x element <h6> (YZ semester) a MALA BY pre tieto periody vytvorit tabulku.
        /// </summary>
        private void LoadPeriods(string htmlStringPart)
        {
            var grade = ExtractString(htmlStringPart);

            var rawPeriods = htmlStringPart.Split(new[] {"<h6>"}, StringSplitOptions.RemoveEmptyEntries)
                .Where(x => x.Contains("</h6>") && x.Contains("<tr>"));

            foreach (var period in rawPeriods)
            {
                LoadPeriodTable(grade, period);
            }
        }

        private void LoadPeriodTable(string grade, string rawPeriod)
        {
            var planTable = new PlanTable();
            var period = grade + " - " + ExtractString(rawPeriod);

            var htmlRows = rawPeriod.Split(new[] { "<tr>" }, StringSplitOptions.RemoveEmptyEntries)
                .Where(x => x.Contains("</tr>"));

            foreach (var row in htmlRows)
            {
                if (row.Contains("<th>"))
                   planTable.AddTableRow(LoadHeaderRow(row));

                if (row.Contains("<td>"))
                    planTable.AddTableRow(LoadTableRow(row));

                if (row.Contains("<td colspan="))
                    planTable.AddTableRow(LoadTableRow(row, true));
            }

            plan.AddNewItemToPlan(period, planTable);
        }

        private PlanTableRow LoadHeaderRow(string htmlRow)
        {
            var planTableRow = new PlanTableRow(true);

            var rowItems = htmlRow.Split(new[] {"<th>"}, StringSplitOptions.RemoveEmptyEntries)
                .Where(x => x.Contains("</th>"));

            foreach (var item in rowItems)
                planTableRow.AddRowItem(ExtractString(item));

            return planTableRow;
        }

        private PlanTableRow LoadTableRow(string htmlRow, bool isCollapsed = false)
        {
            var planTableRow = new PlanTableRow(false, isCollapsed);

            if (!isCollapsed)
            {
                var rowItems = htmlRow.Split(new[] {"<td>"}, StringSplitOptions.RemoveEmptyEntries)
                    .Where(x => x.Contains("</td>"));

                foreach (var item in rowItems)
                    planTableRow.AddRowItem(
                        ExtractString(item
                            .Replace("<sup>", "")
                            .Replace("<strong>", "")
                            .Replace("</strong>", "")
                        )
                    );
            }
            else
            {
                var rowItem = htmlRow.Split(new[] {"</tr>"}, StringSplitOptions.RemoveEmptyEntries)
                    .Where(x => x.Contains("</td>")).ToArray()[0];

                rowItem = rowItem.Replace("<td colspan=\"6\"><strong>", "")
                    .Replace("</strong></td>", "").Trim();

                planTableRow.AddRowItem(rowItem);
            }

            return planTableRow;
        }

        private string ExtractString(string htmlStringPart)
        {
            var startIndex = 0;
            var endIndex = htmlStringPart.IndexOf("<");
            return htmlStringPart.Substring(startIndex, endIndex).Trim();
        }
    }
}
