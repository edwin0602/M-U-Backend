﻿namespace RestBackend.Core.Resources
{
    public class UserResource
    {
        public string Id { get; set; }

        public string UserName { get; set; }

        public string PhoneNumber { get; set; }

        public string Email { get; set; }

        public string Role { get; set; }

        public bool EmailConfirmed { get; set; }
    }

    public class CreateUserResource
    {
        public string PhoneNumber { get; set; }

        public string Email { get; set; }

        public string UserName { get; set; }

        public string Password { get; set; }

        public string RoleName { get; set; }
    }
}
