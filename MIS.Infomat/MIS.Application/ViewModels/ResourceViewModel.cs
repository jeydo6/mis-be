using System;
using System.Collections.Generic;

namespace MIS.Application.ViewModels
{
    public class ResourceViewModel
    {
        public String ResourceName { get; set; }

        public Boolean IsEnabled { get; set; }

        public Boolean IsBlocked { get; set; }

        public Int32 ResourceID { get; set; }

        public Int32 SpecialtyID { get; set; }

        public IEnumerable<DateItemViewModel> Dates { get; set; }
    }
}
