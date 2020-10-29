using System;

namespace MIS.Domain.Entities
{
    public record Analysis
    {
        public Int32 ID { get; set; }

        public String Description { get; set; }
    }
}
