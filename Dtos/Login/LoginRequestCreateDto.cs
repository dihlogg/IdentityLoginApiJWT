using System.ComponentModel.DataAnnotations;

namespace IdentityWebApiSample.Server.Dtos.Login
{
    public class LoginRequestCreateDto
    {
        [Required]
        public string? UserName { get; set; }
        [Required]
        public string? Password { get; set; }
    }
}
