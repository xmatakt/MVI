using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MechatronikaCool.Classes
{
    public static class UltraBadClass
    {
        public static object ReturnSomething(List<string> list)
        {
            switch (list.Count)
            {
                case 1:
                    return new
                    {
                        col1 = list[0],
                        col2 = "",
                        col3 = "",
                        col4 = "",
                        col5 = ""
                    };
                case 5:
                    return new
                    {
                        col1 = list[0],
                        col2 = list[1],
                        col3 = list[2],
                        col4 = list[3],
                        col5 = list[4]
                    };
                default:
                    return new {col1 = "VYSKYTLA SA CHYBA! SPRAV TO LEPSIE!"};
            }
        }
    }
}
