using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MechatronikaCool.Classes
{
    class StudyPlan
    {
        public Dictionary<string, PlanTable> Plan { get; set; }

        public StudyPlan()
        {
            Plan = new Dictionary<string, PlanTable>();
        }

        public Dictionary<string, PlanTable> GetPlan()
        {
            return Plan;
        }

        public void AddNewItemToPlan(string period, PlanTable table)
        {
            if(!Plan.ContainsKey(period))
                Plan.Add(period, table);
        }
    }
}
