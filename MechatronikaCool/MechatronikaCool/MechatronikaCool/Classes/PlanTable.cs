using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MechatronikaCool.Classes
{
    class PlanTable
    {
        public List<PlanTableRow> TableRows { get; set; }

        public PlanTable()
        {
            TableRows = new List<PlanTableRow>();
        }

        public void AddTableRow(PlanTableRow row)
        {
            TableRows.Add(row);
        }
    }
}
