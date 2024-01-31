namespace bad_each_way_finder.Interfaces
{
    public interface ITokenService
    {
        string JwtToken { get; set; }
        DateTime Expiration { get; set; }
        bool ValidateToken();
        void RevokeToken();
    }
}
