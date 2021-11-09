using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using RestBackend.Core.Models.Auth;
using RestBackend.Core.Models.Exceptions;
using RestBackend.Core.Resources;
using RestBackend.Core.Services;
using RestBackend.Core.Services.Infrastructure;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace RestBackend.Services
{
    public class UserService : IUserService
    {
        private readonly ILogger<UserService> _logger;

        private readonly IMapper _mapper;

        private readonly UserManager<User> _userManager;
        private readonly RoleManager<Role> _roleManager;

        private readonly IJWTService _jwtService;
        private readonly IEmailService _emailService;

        public UserService(
            ILogger<UserService> logger,
            IMapper mapper,
            IJWTService jwtService,
            IEmailService emailService,
            UserManager<User> userManager,
            RoleManager<Role> roleManager)
        {
            _logger = logger;

            _mapper = mapper;

            _jwtService = jwtService;
            _emailService = emailService;

            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<UserResource> Create(CreateUserResource toCreate)
        {
            var userModel = _mapper.Map<CreateUserResource, User>(toCreate);

            var userRole = _roleManager.Roles.FirstOrDefault(x => x.Name == toCreate.RoleName);
            if (userRole == default)
                throw new BusinessException($"Role {toCreate.RoleName} not exist.");

            var identityResult = await _userManager.CreateAsync(userModel, toCreate.Password);
            if (!identityResult.Succeeded)
                throw new BusinessException(identityResult.Errors.First().Description);

            await _userManager.AddToRoleAsync(userModel, toCreate.RoleName);

            _logger.LogInformation($"User created! ({userModel.Id})");

            return _mapper.Map<User, UserResource>(userModel);
        }

        public async Task<TokenResource> Authenticate(LoginResource loginResource)
        {
            var user = _userManager.Users.SingleOrDefault(u => u.UserName == loginResource.UserName);
            if (user is null)
                return null;

            var userSigninResult = await _userManager.CheckPasswordAsync(user, loginResource.Password);
            if (!userSigninResult)
            {
                _logger.LogWarning($"User password incorrect! ({user.Id})");
                return null;
            }

            var roles = await _userManager.GetRolesAsync(user);
            if (!roles.Any())
            {
                _logger.LogWarning($"User not have a role! ({user.Id})");
                throw new BusinessException("User is not authorized.");
            }

            var mainRole = _roleManager.Roles.FirstOrDefault(x => x.Name == roles.FirstOrDefault());
            var permissions = await _roleManager.GetClaimsAsync(mainRole);

            _logger.LogInformation($"User was authorized! ({user.Id})");

            return _jwtService.GenerateJwt(user, roles, permissions);
        }

        public async Task ResetPassword(string userId, ResetPasswordResource resetPasswordResource)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user is null)
                throw new BusinessException("User not found");

            var resetValidationResult = await _userManager.VerifyUserTokenAsync(
                    user,
                    TokenOptions.DefaultProvider,
                    UserManager<User>.ResetPasswordTokenPurpose,
                    resetPasswordResource.Token);
            if (!resetValidationResult)
            {
                _logger.LogWarning($"Reset token verification was incorrect! ({user.Id})");
                throw new Exception("Couldn't reset password.");
            }

            var resetResult = await _userManager.ResetPasswordAsync(user, resetPasswordResource.Token, resetPasswordResource.NewPassword);
            if (!resetResult.Succeeded)
            {
                _logger.LogWarning($"Reset password fail! ({user.Id})");
                throw new Exception(resetResult.Errors.First().Description);
            }

            _logger.LogInformation($"Reset password success! ({user.Id})");
        }

        public async Task SendForgotPasswordToken(string userName, string returnUrl = null)
        {
            var user = _userManager.Users.SingleOrDefault(u => u.UserName == userName);
            if (user is null)
            {
                _logger.LogWarning($"Reset token request failed! ({userName})");
                return;
            }
                
            var resetTokenResult = await _userManager.GeneratePasswordResetTokenAsync(user);
            if (string.IsNullOrEmpty(resetTokenResult))
                return;

            string resetTokenURL = $"{returnUrl}/{user.Id}/ResetPassword?tk={HttpUtility.UrlEncode(resetTokenResult)}";
            await _emailService.Send(new Core.Models.Notification.EmailNotification
            {
                To = user.Email,
                Message = $"Dear user {user.UserName}: Your password recovery link is {resetTokenURL}.",
                Subject = "Account recovery notification."
            });

            _logger.LogInformation($"Reset token request success! ({userName})");
        }
    }
}
