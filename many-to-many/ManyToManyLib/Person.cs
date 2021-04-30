using System;
using System.Collections.Generic;

namespace ManyToManyLib
{
    public class Person
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public List<Address> Addresses { get; set; }

        public Person()
        {
            this.Addresses = new List<Address>();
        }
    }
}
