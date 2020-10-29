using System;

namespace MIS.Domain.Entities
{
    public record Resource
    {
        public Int32 ID { get; set; }

        public Int32 DoctorID { get; set; }

        public Doctor Doctor { get; set; }

        public Int32 RoomID { get; set; }

        public Room Room { get; set; }
    }
}
