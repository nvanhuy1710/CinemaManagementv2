﻿using Cinema.Model;

namespace Cinema.Module.Account.Register
{
    public class RegisterData
    {
        public string Name { get; set; }

        public string Email { get; set; }

        public bool Gender { get; set; }

        public DateTime Birth { get; set; }

        public string Phone { get; set; }

        public string? Address { get; set; }

        public string Password { get; set; }

        public string Role { get; set; }
    }
}
