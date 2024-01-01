using bad_each_way_finder.Extensions;
using bad_each_way_finder.Interfaces;
using bad_each_way_finder.Model;
using bad_each_way_finder.Settings;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net.Http.Headers;
using System.Text;

namespace bad_each_way_finder.Services
{
    public class LoginService : ILoginService
    {
        private readonly ILogger<LoginService> _logger;
        private readonly HttpClient _httpClient;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ITokenService _tokenService;
        private readonly IOptions<ApiSettings> _options;

        public LoginService(ILogger<LoginService> logger, HttpClient httpClient, 
            UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager, ITokenService tokenService,
            IOptions<ApiSettings> options) 
        {
            _logger = logger;
            _httpClient = httpClient;
            _userManager = userManager;
            _roleManager = roleManager;
            _tokenService = tokenService;
            _options = options;

            _httpClient.BaseAddress = new Uri(_options.Value.BaseUrl);
        }

        public async Task<bool> EnsureBackend()
        {
            try
            {
                var response = await _httpClient.GetAsync("health");

                _logger.LogInformation($"Backend is running = {response.IsSuccessStatusCode}");

                return response.IsSuccessStatusCode;
            }
            catch
            {
                _logger.LogCritical($"Backend is running = false. " +
                    $"Please ensure core-strength-yoga-products-management-api is running!");

                return false;
            }
        }

        public async Task<LoginResult> Login(User user)
        {
            var response = await PostAsync("login", user);

            var contentResult = TryExtractContent(response, out var jsonObject);
            
            if(contentResult.Contains("failed"))
            {
                return new LoginResult { Succeeded = false, Message = contentResult };
            }

            _tokenService.JwtToken = ExtractFromJson<string>(jsonObject, "token");
            var expiration = ExtractFromJson<string>(jsonObject, "expiration");
            _tokenService.Expiration = DateTime.Parse(expiration);

            var identityUser = ExtractFromJson<IdentityUser>(jsonObject, "user");
            var roles = ExtractFromJson<IEnumerable<string>>(jsonObject, "roles");

            identityUser = await identityUser.UpsertUser(_userManager);
            identityUser = await identityUser.UpdateRoles(_userManager, _roleManager, roles);

            return new LoginResult() { IdentityUser = identityUser, UserRoles = roles, Succeeded = true };
        }

        public async Task<LoginResult> Register(User user)
        {
            var response = await PostAsync("register", user);

            var contentResult = TryExtractContent(response, out var jsonObject);

            if (contentResult.Contains("failed"))
            {
                return new LoginResult { Succeeded = false, Message = contentResult };
            }

            _tokenService.JwtToken = ExtractFromJson<string>(jsonObject, "token");
            var expiration = ExtractFromJson<string>(jsonObject, "expiration");
            _tokenService.Expiration = DateTime.Parse(expiration);

            var identityUser = ExtractFromJson<IdentityUser>(jsonObject, "user");
            var roles = ExtractFromJson<IEnumerable<string>>(jsonObject, "roles");

            identityUser = await identityUser.UpsertUser(_userManager);
            identityUser = await identityUser.UpdateRoles(_userManager, _roleManager, roles);

            return new LoginResult() { IdentityUser = identityUser, UserRoles = roles, Succeeded = true };
        }

        private async Task<HttpResponseMessage> PostAsync(string endpoint, object body)
        {
            return await _httpClient.PostAsync($"/api/identity/{endpoint}", WithContent(body));
        }

        private StringContent WithContent(object body)
        {
            WithJsonHeader();
            return new StringContent(JsonConvert.SerializeObject(body), Encoding.UTF8, "application/json");
        }

        private void WithJsonHeader()
        {
            _httpClient.DefaultRequestHeaders
                .Accept
                .Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        private static string TryExtractContent(HttpResponseMessage response, out JObject result)
        {
            result = new JObject();
            
            if (!response.IsSuccessStatusCode)
            {
                return $"Request failed {response.StatusCode}. {response.Content.ReadAsStringAsync().Result}";
            }

            var content = string.Empty;
            using (var sr = new StreamReader(response.Content.ReadAsStream())) 
            { 
                content = sr.ReadToEnd(); 
            }

            if(string.IsNullOrEmpty(content))
            {
                return $"Response content failed to be parsed.";
            }

            result = JObject.Parse(content);

            return "success";
        }

        private static T ExtractFromJson<T>(JObject jsonObject, string prop) where T : class
        {
            var value = jsonObject?.GetValue(prop)?.ToString() ??
                throw new NullReferenceException($"Could not read {prop} from JsonObject.");

            T result = null;

            if(typeof(T) != typeof(string))
            {
                result = JsonConvert.DeserializeObject<T>(value) ??
                            throw new JsonSerializationException($"Deserialization of {prop} to {typeof(T).Name} failed.");
            }
            else
            {
                result = (T)Convert.ChangeType(value, typeof(T)); 
            }

            return result;
        }
    }
}
