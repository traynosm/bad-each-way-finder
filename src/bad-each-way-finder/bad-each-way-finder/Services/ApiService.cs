using bad_each_way_finder.Interfaces;
using bad_each_way_finder.Settings;
using bad_each_way_finder_domain.Dto;
using Microsoft.Extensions.Options;

namespace bad_each_way_finder.Services
{
    public class ApiService : IApiService
    {
        private readonly HttpClient _httpClient;
        private readonly IOptions<ApiSettings> _options;

        public ApiService(HttpClient httpClient, IOptions<ApiSettings> options)
        {
            _httpClient = httpClient;
            _options = options;
            _httpClient.BaseAddress = new Uri(_options.Value.BaseUrl);
        }

        public async Task<RacesAndPropositionsDto?> Get()
        {
            return await _httpClient.GetFromJsonAsync<RacesAndPropositionsDto>("/api/Proposition");
        }
    }
}
