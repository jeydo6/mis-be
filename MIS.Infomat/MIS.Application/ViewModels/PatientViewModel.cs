using System;
using System.Collections.Generic;

namespace MIS.Application.ViewModels
{
    public class PatientViewModel
    {
        public PatientViewModel()
        {
            VisitItems = new List<VisitItemViewModel>();
            Dispanserizations = new List<DispanserizationViewModel>();
        }

        public Int32 ID { get; set; }

        public String Code { get; set; }

        public String FirstName { get; set; }

        public String MiddleName { get; set; }

        public String DisplayName
        {
            get
            {
                return $"{FirstName} {MiddleName}".Trim();
            }
        }

        public DateTime BirthDate { get; set; }

        public Int32 Gender { get; set; }

        public ICollection<VisitItemViewModel> VisitItems { get; set; }

        public ICollection<DispanserizationViewModel> Dispanserizations { get; set; }
    }
}
