using System;
using System.Collections.Generic;
using System.Text;

namespace MalaBars.Domain.Entities
{
    public class Address
    {
        public int AddressId { get; set; }

        public int UserId { get; set; } //→ Which user owns this address

        public string FullName { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string AddressLine { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string State { get; set; } = string.Empty;
        public string Pincode { get; set; } = string.Empty;

        public User User { get; set; } = null!;
    }
}
