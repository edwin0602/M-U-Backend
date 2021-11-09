using RestBackend.Core.Resources;
using System.Threading.Tasks;

namespace RestBackend.Core.Services
{
    public interface IUserService
    {
        Task<UserResource> Create(CreateUserResource toCreate);

        Task<TokenResource> Authenticate(LoginResource loginResource);

        Task ResetPassword(string userId, ResetPasswordResource resetPasswordResource);

        Task SendForgotPasswordToken(string userName, string returnUrl = null);

    }
}
