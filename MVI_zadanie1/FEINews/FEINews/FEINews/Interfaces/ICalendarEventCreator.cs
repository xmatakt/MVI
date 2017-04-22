using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FEINews.Interfaces
{
    public interface ICalendarEventCreator
    {
        void CreateEventForExam(Data.Exam exam);
        void CreateEventForEvent(Data.Event ivent, bool auto);
    }
}
