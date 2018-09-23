using System.ComponentModel.DataAnnotations;

namespace ChatApp.Backend.Models.Token
{
    public class TokenRequest
    {
        [Required]
        public string Username { get; set; }
    }

    public class TokenResponse
    {
        [Required]
        public string Token { get; set; }
    }
}
