﻿using bad_each_way_finder.Interfaces;
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

                if (string.IsNullOrEmpty(token))
                {
                    throw new InvalidDataException($"Invalid Token.");
                }

                var result = await _httpClient.GetFromJsonAsync<RacesAndPropositionsDto?>(
                   $"/api/v1/Proposition?token={token}");

                return result;
            }
            catch (InvalidDataException ivdEx)
            {
                Console.WriteLine("InvalidDataException raised, GetRacesAndPropositionsDto failed.");
                throw new ApiServiceException(ivdEx, $"{ivdEx.Message} - GetRacesAndPropositionsDto() failed.");
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

                if (string.IsNullOrEmpty(token))
                {
                    throw new InvalidDataException($"Invalid Token.");
                }

                var result = await _httpClient.GetFromJsonAsync<List<Proposition>?>(
                    $"/api/v1/Account/GetAccountPropositions/{userName}/{token}");

                return result;
            }
            catch (InvalidDataException ivdEx)
            {
                Console.WriteLine("InvalidDataException raised, GetAccountPropositions failed.");
                throw new ApiServiceException(ivdEx, $"{ivdEx.Message} - GetAccountPropositions() failed.");
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

                var response = await _httpClient.PostAsync($"/api/v1/Account/PostRaisedProposition", content);

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

                var response = await _httpClient.PostAsync($"/api/v1/Account/RemoveAccountProposition", content);

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
    }
}
