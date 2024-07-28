using System.ComponentModel.DataAnnotations;

namespace IdentityWebApiSample.Server.Dtos.Register
{
    public class RegisterRequestCreateDto
    {
        [Required]
        public string? UserName { get; set; }
        [Required]
        public string? Email { get; set; }
        [Required]
        public string? Password { get; set; }
    }
}
