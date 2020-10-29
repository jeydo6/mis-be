using System;

namespace MIS.Domain.Entities
{
    public record TimeItemTotal
    {
        public Int32 ResourceID { get; set; }

        public DateTime Date { get; set; }

        public Int32 TimesCount { get; set; }

        public Int32 VisitsCount { get; set; }
    }
}
