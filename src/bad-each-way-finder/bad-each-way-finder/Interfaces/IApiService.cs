using bad_each_way_finder_domain.DomainModel;
using bad_each_way_finder_domain.Dto;

namespace bad_each_way_finder.Interfaces
{
    public interface IApiService
    {
        Task<RacesAndPropositionsDto?> GetRacesAndPropositionsDto();
        Task<List<Proposition>?> GetAccountPropositions(string userName);
        Task<List<Proposition>> PostSavedPropostionDto(SavedPropositionDto savedPropositionDto);
        Task<List<Proposition>> RemoveSavedPropostionDto(SavedPropositionDto savedPropositionDto);
    }
}
