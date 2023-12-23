using bad_each_way_finder_domain.DomainModel;

namespace bad_each_way_finder_domain.Dto
{
    public class RacesAndPropositionsDto
    {
        public List<Proposition> LivePropositions { get; set; }
        public List<Proposition> SavedPropositions { get; set; }
        public List<Race> Races { get; set; }
    }
}
