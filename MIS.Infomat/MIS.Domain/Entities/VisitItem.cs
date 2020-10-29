using System;

namespace MIS.Domain.Entities
{
    public record VisitItem
    {
        public Int32 ID { get; set; }

        public Int32 PatientID { get; set; }

        public Patient Patient { get; set; }

        public Int32 TimeItemID { get; set; }
        
        public TimeItem TimeItem { get; set; }
    }
}