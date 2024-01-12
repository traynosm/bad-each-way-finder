using bad_each_way_finder_domain.DomainModel;

namespace bad_each_way_finder_domain.Dto
{
    public class RacesAndPropositionsDto
    {
        public List<Proposition> LivePropositions { get; set; }
        public List<Proposition> RaisedPropositions { get; set; }
        public List<Proposition> NewlyRaisedPropositions { get; set; }
        public List<Race> Races { get; set; }
    }
}
