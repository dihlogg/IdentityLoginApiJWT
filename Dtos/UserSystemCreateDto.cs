using System.ComponentModel.DataAnnotations;

namespace IdentityWebApiSample.Server.Dtos
{
    public class UserSystemCreateDto
    {
        [Required]
        [EmailAddress]
        public string UserEmail { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string UserPassword { get; set; }
        public bool RememberMe { get; set; }
    }
}
