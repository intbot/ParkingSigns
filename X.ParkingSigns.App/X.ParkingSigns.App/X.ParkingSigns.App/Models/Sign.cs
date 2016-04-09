using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace X.ParkingSigns.App.Models
{
    public class Sign : Location
    {
		public string DayOfWeek { get; set; } // DateTime.DayOfWeek
		public string StartTime { get; set; } // TODO - Store in Time???
		public string EndTime { get; set; } // Store in Time???
    }
}
