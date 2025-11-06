using System;
using System.Collections.Generic;

namespace TIVIT.CIPA.Api.Domain.Model
{
    public class Election
    {
        public int Id { get; set; }
        public int CompanyId { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        // Tornado nullable para ficar consistente com o mapeamento (IsRequired(false))
        public DateTime? RegistrationStartDate { get; set; }
        public DateTime? RegistrationEndDate { get; set; }
        public DateTime? ElectionStartDate { get; set; }
        public DateTime? ElectionEndDate { get; set; }

        public string Type { get; set; }
        public string InvitationMessage { get; set; }
        public bool IsActive { get; set; } = true;
        public DateTime? CreateDate { get; set; }
        public string CreateUser { get; set; }
        public DateTime? UpdateDate { get; set; }
        public string UpdateUser { get; set; }

        public ICollection<Candidate> Candidates { get; set; } = new List<Candidate>();
        public ICollection<ElectionSite> ElectionSites { get; set; } = new List<ElectionSite>();
        public ICollection<Voter> Voters { get; set; } = new List<Voter>();
        public ICollection<Vote> Votes { get; set; } = new List<Vote>();
    }
}
