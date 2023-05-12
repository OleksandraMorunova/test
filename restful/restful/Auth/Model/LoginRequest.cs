using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace restful.Auth
{
    public class LoginRequest
    {
        [Required]
        [EmailAddress(ErrorMessage = "Invalide email address")]
        public string Email { get; set; } = string.Empty;

        [Required]
        public string Password { get; set; } = string.Empty;
    }
}
