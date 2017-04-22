using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FEINews.Data
{
    public class Event
    {
        public int ID { get; set; }
        public string EventTitle { get; set; }
        public string EventDescription { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int EventVenueID { get; set; }
        public string Location { get; set; }

        public override string ToString()
        {
            var startDate = StartDate.ToString("d.M.yyyy H:mm");
            var endDate = EndDate.ToString("d.M.yyyy H:mm");
            var builder = new StringBuilder();
            builder.AppendLine("Kde?");
            builder.AppendLine(Location);
            builder.AppendLine("Kedy?");
            if (StartDate.Date == EndDate.Date)
                builder.AppendLine(StartDate.ToString("d.M.yyyy H:mm") + " - " + EndDate.ToString("H:mm"));
            else
                builder.AppendLine(startDate + " - " + endDate);
            builder.AppendLine("\nO čo ide?");
            builder.AppendLine(EventDescription);
            return builder.ToString();
        }
    }
}
