using bad_each_way_finder.Interfaces;
using bad_each_way_finder.Settings;
using bad_each_way_finder_domain.DomainModel;
using bad_each_way_finder_domain.Dto;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Text;

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

        public async Task<RacesAndPropositionsDto?> GetRacesAndPropositionsDto()
        {
            try
            {
               var result = await _httpClient.GetFromJsonAsync<RacesAndPropositionsDto?>(
                   "/api/Proposition");

                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);

                throw;
            }
        }

        public async Task<List<Proposition>?> GetAccountPropositions(string userName)
        {
            try
            {
                var result = await _httpClient.GetFromJsonAsync<List<Proposition>?>(
                    $"/api/Account/GetAccountPropositions/{userName}");

                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);

                throw;
            }
        }

        public async Task<List<Proposition>> PostSavedPropostionDto(SavedPropositionDto savedPropositionDto)
        {
            var json = JsonConvert.SerializeObject(savedPropositionDto);

            StringContent content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync($"/api/Account/PostSaveProposition", content);

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("broken");
            }

            var savedPropositionsJson = await response.Content.ReadAsStringAsync();

            var savedPropositions = JsonConvert.DeserializeObject<List<Proposition>>(savedPropositionsJson);

            return savedPropositions!;
        }

        public async Task<List<Proposition>> RemoveSavedPropostionDto(SavedPropositionDto savedPropositionDto)
        {
            var json = JsonConvert.SerializeObject(savedPropositionDto);

            StringContent content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync($"/api/Account/RemoveAccountProposition", content);

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("broken");
            }

            var savedPropositionsJson = await response.Content.ReadAsStringAsync();

            var savedPropositions = JsonConvert.DeserializeObject<List<Proposition>>(savedPropositionsJson);

            return savedPropositions!;
        }
    }
}
