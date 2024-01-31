using bad_each_way_finder.Interfaces;

namespace bad_each_way_finder.Services
{
    public class TokenService : ITokenService
    {
        public TokenService() 
        { 
            JwtToken = string.Empty;
        }

        public string JwtToken { get; set; }
        public DateTime Expiration { get; set; }

        public bool ValidateToken()
        {
            return !string.IsNullOrEmpty(JwtToken) && Expiration >= DateTime.Now;
        }
        public void RevokeToken()
        {
            JwtToken = string.Empty;
            Expiration = default;
        }
    }
}
