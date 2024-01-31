#nullable disable 

namespace bad_each_way_finder.Model
{
    public class User
    {
        public User() 
        {
            Username = string.Empty;
            Password = string.Empty;
            Email = string.Empty;
            UserRoles = new List<string>();        
        }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public IEnumerable<string> UserRoles { get; set; }
    }
}
