using System;
using System.Collections.Generic;

namespace MIS.Domain.Entities
{
    public record Patient
    {
        public Patient()
        {
            VisitItems = new List<VisitItem>();
            Dispanserizations = new List<Dispanserization>();
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

        public ICollection<VisitItem> VisitItems { get; set; }

        public ICollection<Dispanserization> Dispanserizations { get; set; }
    }
}
