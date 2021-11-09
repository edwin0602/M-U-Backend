using System;

namespace RestBackend.Core.Resources
{
    public class TokenResource
    {
        public string Token { get; set; }

        public DateTime Expires { get; set; }
    }

    public class LoginResource
    {
        public string UserName { get; set; }

        public string Password { get; set; }
    }

    public class ResetPasswordResource
    {
        public string Token { get; set; }

        public string NewPassword { get; set; }
    }

    public class ForgotPasswordTokenResource
    {
        public string Token { get; set; }

        public string UserId { get; set; }
    }
}
