using bad_each_way_finder.Interfaces;
using bad_each_way_finder.Settings;
using bad_each_way_finder_domain.DomainModel;
using bad_each_way_finder_domain.Dto;
using bad_each_way_finder_domain.Exceptions;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Text;

namespace bad_each_way_finder.Services
{
    public class ApiService : IApiService
    {
        private readonly HttpClient _httpClient;
        private readonly IOptions<ApiSettings> _options;
        private readonly ITokenService _tokenService;

        public ApiService(HttpClient httpClient, IOptions<ApiSettings> options,
            ITokenService tokenService)
        {
            _httpClient = httpClient;
            _options = options;
            _httpClient.BaseAddress = new Uri(_options.Value.BaseUrl);
            _tokenService = tokenService;
        }

        public async Task<RacesAndPropositionsDto?> GetRacesAndPropositionsDto()
        {
            try
            {
                var token = _tokenService.JwtToken;

                var result = await _httpClient.GetFromJsonAsync<RacesAndPropositionsDto?>(
                   $"/api/Proposition?token={token}");

                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception raised, GetRacesAndPropositionsDto failed.");
                throw new ApiServiceException(ex, $"{ex.Message} - GetRacesAndPropositionsDto() failed.");
            }
        }

        public async Task<List<Proposition>?> GetAccountPropositions(string userName)
        {
            try
            {
                var token = _tokenService.JwtToken;
                var result = await _httpClient.GetFromJsonAsync<List<Proposition>?>(
                    $"/api/Account/GetAccountPropositions/{userName}/{token}");

                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception raised, GetAccountPropositions failed.");
                throw new ApiServiceException(ex, $"{ex.Message} - GetAccountPropositions() failed.");
            }
        }

        public async Task<List<Proposition>> PostRaisedPropostionDto(RaisedPropositionDto raisedPropositionDto)
        {
            try
            {
                var token = _tokenService.JwtToken;
                raisedPropositionDto.Token = token;

                var json = JsonConvert.SerializeObject(raisedPropositionDto);

                StringContent content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync($"/api/Account/PostRaisedProposition", content);

                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception("broken");
                }

                var raisedPropositionsJson = await response.Content.ReadAsStringAsync();

                var raisedPropositions = JsonConvert.DeserializeObject<List<Proposition>>(raisedPropositionsJson);

                return raisedPropositions!;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception raised, PostRaisedPropostionDto failed.");
                throw new ApiServiceException(ex, $"{ex.Message} - PostRaisedPropostionDto() failed.");
            }
        }

        public async Task<List<Proposition>> RemoveRaisedPropostionDto(RaisedPropositionDto raisedPropositionDto)
        {
            try
            {
                var token = _tokenService.JwtToken;
                raisedPropositionDto.Token = token;

                var json = JsonConvert.SerializeObject(raisedPropositionDto);

                StringContent content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync($"/api/Account/RemoveAccountProposition", content);

                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception("broken");
                }

                var raisedPropositionsJson = await response.Content.ReadAsStringAsync();

                var raisedPropositions = JsonConvert.DeserializeObject<List<Proposition>>(raisedPropositionsJson);

                return raisedPropositions!;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception raised, RemoveRaisedPropostionDto failed.");
                throw new ApiServiceException(ex, $"{ex.Message} - RemoveRaisedPropostionDto() failed.");
            }
        }

        public async Task Logoout(string token)
        {
            try
            {
                await _httpClient.GetAsync($"/api/Identity/Logout/{token}");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception raised, Logoout failed.");
                throw new ApiServiceException(ex, $"{ex.Message} - Logoout() failed.");
            }
        }
    }
}
