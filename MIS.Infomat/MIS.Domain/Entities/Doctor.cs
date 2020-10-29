using System;
using System.Text;

namespace MIS.Domain.Entities
{
    public record Doctor
    {
        public Int32 ID { get; set; }

        public String Code { get; set; }

        public String FirstName { get; set; }

        public String MiddleName { get; set; }

        public String LastName { get; set; }

        public String DisplayName
        {
            get
            {
                StringBuilder sb = new StringBuilder();

                if (!String.IsNullOrEmpty(LastName))
                {
                    if (LastName.Contains(' '))
                    {
                        sb.Append(LastName[0..LastName.IndexOf(' ')]);
                    }
                    else
                    {
                        sb.Append(LastName);
                    }
                }

                if (!String.IsNullOrEmpty(FirstName))
                {
                    sb.Append($" {FirstName[0]}.");
                }

                if (!String.IsNullOrEmpty(MiddleName))
                {
                    sb.Append($" {MiddleName[0]}.");
                }

                return sb.ToString().Trim();
            }
        }

        public Int32 SpecialtyID { get; set; }

        public Specialty Specialty { get; set; }
    }
}
