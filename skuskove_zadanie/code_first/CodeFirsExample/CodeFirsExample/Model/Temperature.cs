using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeFirsExample.Model
{
    public class Temperature
    {
        public int ID { get; set; }
        public DateTime Date { get; set; }
        public double Value { get; set; }
        public int UserID { get; set; }

        public virtual User User { get; set; }
    }
}
