using System;

namespace MIS.Domain.Entities
{
    public record TimeItem
    {
        public Int32 ID { get; set; }

        public DateTime Date { get; set; }

        public DateTime BeginDateTime { get; set; }

        public DateTime EndDateTime { get; set; }

        public Int32 ResourceID { get; set; }
        
        public Resource Resource { get; set; }

        public VisitItem VisitItem { get; set; }
    }
}
