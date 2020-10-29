using System;

namespace MIS.Domain.Entities
{
    public record Specialty
    {
        public Int32 ID { get; set; }

        public String Code { get; set; }

        public String Name { get; set; }
    }
}