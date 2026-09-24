using System;
using System.Collections.Generic;
using System.Text;

namespace MalaBars.Domain.Entities
{
    public class User
    {
        public int UserId { get; set; }

        public string Name { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string PasswordHash { get; set; } = string.Empty;

        public string Role { get; set; } = "User";

        public bool IsBlocked { get; set; }
    }
}
