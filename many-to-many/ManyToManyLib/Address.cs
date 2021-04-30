using System;
using System.Collections.Generic;

namespace ManyToManyLib
{
    public class Address
    {
        public Guid Id { get; set; }
        public string Street { get; set; }
        public string PostalCode { get; set; }
        public List<Person> Residents { get; set; }

        public Address()
        {
            this.Residents = new List<Person>();
        }
    }
}