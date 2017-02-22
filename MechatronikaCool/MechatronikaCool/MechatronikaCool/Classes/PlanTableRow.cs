using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace MechatronikaCool.Classes
{
    class PlanTableRow
    {
        public bool IsHeaderRow { get; set; }
        public bool IsCollapsedRow { get; set; }
        private List<string> rowData;

        public PlanTableRow(bool isHeaderRow, bool isCollapsedRow = false)
        {
            IsCollapsedRow = isCollapsedRow;        
            IsHeaderRow = isHeaderRow;
            rowData = new List<string>();
        }

        public List<string> GetRowData()
        {
            return rowData;
        }

        public void AddRowItem(string rowItem)
        {
            rowData.Add(rowItem);
        }
    }
}
