using bad_each_way_finder_domain.DomainModel;

namespace bad_each_way_finder_domain.Dto
{
    public class RacesAndPropositionsDto
    {
        public List<Proposition> Propositions { get; set; }
        public List<Race> Races { get; set; }
    }
}
