using System.ComponentModel.DataAnnotations;

namespace IdentityWebApiSample.Server.Entities
{
    public class LoginRequest
    {
        [Required]
        public string? UserName { get; set; }
        [Required]
        public string? Password { get; set; }
    }
}
