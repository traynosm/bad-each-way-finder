using Microsoft.AspNetCore.Identity;
#nullable disable

namespace bad_each_way_finder.Model
{
    public class LoginResult
    {
        public IdentityUser IdentityUser { get; set; }
        public IEnumerable<string> Roles { get; set; }
        public bool Succeeded { get; set; }
        public string Message { get; set; }
    }
}
