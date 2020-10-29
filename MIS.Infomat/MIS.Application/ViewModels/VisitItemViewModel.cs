using System;

namespace MIS.Application.ViewModels
{
    public class VisitItemViewModel
    {
        public DateTime BeginDateTime { get; set; }

        public String PatientCode { get; set; }

        public String PatientName { get; set; }

        public String DoctorName { get; set; }

        public String SpecialtyName { get; set; }

        public String RoomCode { get; set; }

        public Int32 RoomFlat { get; set; }

        public Boolean IsEnabled { get; set; }

        public Int32 ResourceID { get; set; }
    }
}
