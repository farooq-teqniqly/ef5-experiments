using System;

namespace ManyToManyLib
{
    public class Resident
    {
        public Guid PersonId { get; set; }
        public Guid AddressId { get; set; }
        public DateTime LocatedDate { get; set; }
    }
}
