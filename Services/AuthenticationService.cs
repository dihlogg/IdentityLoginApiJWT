using IdentityWebApiSample.Server.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace IdentityWebApiSample.Server.Services
{
    public interface IAuthenticationService
    {
        Task<string> Register(RegisterRequest registerRequest);
        Task<string> Login(LoginRequest loginRequest);
        Task ConfirmEmailAsync(string userId, string token);
    }
    public class AuthenticationService : IAuthenticationService
    {
        private readonly UserManager<UserSystem> _userManager;
        private readonly IConfiguration _configuration;
        private readonly IEmailSender _emailSender;

        public AuthenticationService(UserManager<UserSystem> userManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _configuration = configuration;
        }

        public async Task<string> Register(RegisterRequest registerRequest)
        {
            var userByEmail = await _userManager.FindByEmailAsync(registerRequest.Email);
            var userByUsername = await _userManager.FindByNameAsync(registerRequest.UserName);
            if (userByEmail is not null || userByUsername is not null)
            {
                throw new ArgumentException($"User with email {registerRequest.Email} or username {registerRequest.UserName} already exists.");
            }

            UserSystem user = new()
            {
                Email = registerRequest.Email,
                UserName = registerRequest.UserName,
                SecurityStamp = Guid.NewGuid().ToString()
            };

            var result = await _userManager.CreateAsync(user, registerRequest.Password);

            if (!result.Succeeded)
            {
                throw new ArgumentException($"Unable to register user {registerRequest.UserName} errors: {GetErrorsText(result.Errors)}");
            }

            return await Login(new LoginRequest { UserName = registerRequest.Email, Password = registerRequest.Password });
        }

        public async Task<string> Login(LoginRequest loginRequest)
        {
            var user = await _userManager.FindByNameAsync(loginRequest.UserName);

            if (user is null)
            {
                user = await _userManager.FindByEmailAsync(loginRequest.UserName);
            }

            if (user is null || !await _userManager.CheckPasswordAsync(user, loginRequest.Password))
            {
                throw new ArgumentException($"Unable to authenticate user {loginRequest.UserName}");
            }

            var authClaims = new List<Claim>
        {
            new(ClaimTypes.Name, user.UserName),
            new(ClaimTypes.Email, user.Email),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        };

            var token = GetToken(authClaims);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private JwtSecurityToken GetToken(IEnumerable<Claim> authClaims)
        {
            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));

            var token = new JwtSecurityToken(
                issuer: _configuration["JWT:ValidIssuer"],
                audience: _configuration["JWT:ValidAudience"],
                expires: DateTime.Now.AddHours(3),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256));

            return token;
        }

        private string GetErrorsText(IEnumerable<IdentityError> errors)
        {
            return string.Join(", ", errors.Select(error => error.Description).ToArray());
        }

        public Task ConfirmEmailAsync(string userId, string token)
        {
            throw new NotImplementedException();
        }
    }
}