using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FEINews.Data
{
    public class Exam
    {
        public int ID { get; set; }
        public string Workplace { get; set; }
        public string Code { get; set; }
        public string SubjectName { get; set; }
        public string RTDate { get; set; }
        public string RTTime { get; set; }
        public string RTPlace { get; set; }
        public string OTDate { get; set; }
        public string OTTime { get; set; }
        public string OTPlace { get; set; }
        public string Details
        {
            get { return Code + ' ' + SubjectName; }
        }

        public string Description
        {
            get
            {
                var builder = new StringBuilder();

                builder.AppendLine("Pracovisko: " + Workplace);
                builder.AppendLine("Predmet: " + SubjectName);
                builder.AppendLine();
                builder.AppendLine("-------Riadny termín-------");
                builder.AppendLine("Dátum: " + RTDate);
                builder.AppendLine("Čas: " + RTTime);
                builder.AppendLine("Miesto: " + RTPlace);
                builder.AppendLine();
                builder.AppendLine("-------Opravný termín-------");
                builder.AppendLine("Dátum: " + OTDate);
                builder.AppendLine("Čas: " + OTTime);
                builder.AppendLine("Miesto: " + OTPlace);

                return builder.ToString();
            }
        }

        private const string messyString = "dohodu";
    }
}
