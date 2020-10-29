using System;
using System.Collections.Generic;

namespace MIS.Application.ViewModels
{
    public class DispanserizationViewModel
    {
        public DateTime BeginDate { get; set; }

        public DateTime Today { get; set; }

        public String PatientCode { get; set; }

        public String PatientName { get; set; }

        public Boolean IsClosed { get; set; }

        public Boolean IsEnabled { get; set; }

        public IEnumerable<String> Analyses { get; set; }
    }
}
