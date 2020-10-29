using System;
using System.Collections.Generic;

namespace MIS.Application.ViewModels
{
    public class DateItemViewModel
    {
        public DateTime Date { get; set; }

        public Boolean IsEnabled { get; set; }

        public Boolean IsBlocked { get; set; }

        public Int32 ResourceID { get; set; }

        public IEnumerable<TimeItemViewModel> Times { get; set; }
    }
}
