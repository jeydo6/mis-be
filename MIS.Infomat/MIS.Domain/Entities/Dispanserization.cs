using System;
using System.Collections.Generic;

namespace MIS.Domain.Entities
{
    public record Dispanserization
    {
        public Int32 ID { get; set; }

        public DateTime BeginDate { get; set; }

        public DateTime EndDate { get; set; }

        public Boolean IsClosed { get; set; }

        public Int32 PatientID { get; set; }

        public ICollection<Analysis> Analyses { get; set; }
    }
}