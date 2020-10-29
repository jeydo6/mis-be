using System;

namespace MIS.Domain.Entities
{
    public record Room
    {
        public Int32 ID { get; set; }

        public String Code { get; set; }

        public Int32 Flat { get; set; }
    }
}
