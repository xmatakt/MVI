using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FEINews.Data
{
    public class Office
    {
        private string name;
        public string Name
        {
            get { return name; }
            set { name = GetOfficeName(value); }
        }
        public string Hours { get; set; }

        private string GetOfficeName(string value)
        {
            return value;
        }
    }
}
