using Microsoft.AspNetCore.Identity;

namespace bad_each_way_finder_domain.DomainModel
{
    public class Account
    {
        public int Id { get; set; }
        public virtual IdentityUser IdentityUser { get; set; }
        public string IdenityUserName { get; set; }
        public List<Proposition> AccountPropositions { get; set; }
    }
}
