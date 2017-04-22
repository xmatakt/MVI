using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FEINews.Interfaces
{
    public interface IDbCommunicator
    {
        Task GetOfficeHoursAsync(List<Data.Office> offices);
        Task GetEventsAsync(List<Data.Event> events);
    }
}
