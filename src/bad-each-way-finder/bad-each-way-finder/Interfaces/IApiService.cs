using bad_each_way_finder_domain.Dto;

namespace bad_each_way_finder.Interfaces
{
    public interface IApiService
    {
        Task<RacesAndPropositionsDto> Get();
    }
}
