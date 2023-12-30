using bad_each_way_finder.Model;

namespace bad_each_way_finder.Interfaces
{
    public interface ILoginService
    {
        Task<bool> EnsureBackend();
        Task<LoginResult> Login(User user);
        Task<LoginResult> Register(User user);
    }
}
