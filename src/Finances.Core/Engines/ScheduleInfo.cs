using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Finances.Core.Interfaces;

namespace Finances.Core.Entities
{
    //public class ScheduleInfo : IScheduleInfo
    //{
    //    public string GetDescription(IScheduleEntity schedule)
    //    {
    //        var sb = new StringBuilder();

    //        if (schedule.StartDate == schedule.EndDate)
    //        {
    //            sb.AppendFormat("One-off on {0:yyyy-MM-dd}", schedule.StartDate);
    //        }
    //        else
    //        {
    //            sb.AppendFormat("Every {0} {1} from {2:yyyy-MM-dd}", schedule.FrequencyEvery,schedule.Frequency,schedule.StartDate);
    //            if (schedule.EndDate != null)
    //            {
    //                sb.AppendFormat(" until {0:yyyy-MM-dd}", schedule.EndDate);
    //            }
    //        }


    //        return sb.ToString(); ;
    //    }
    //}
}
