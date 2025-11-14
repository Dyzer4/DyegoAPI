using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TIVIT.CIPA.Api.Domain.Model.Responses
{
    public class CandidateVerifyResponse
    {
        public int VoterId { get; set; }
        public string CorporateId { get; set; }
        public string Name { get; set; }
        public DateTime BirthDate { get; set; }
        public string Department { get; set; }
        public string Area { get; set; }
        public DateTime AdmissionDate { get; set; }
        public string CorporateEmail { get; set; }
        public string ContactPhone { get; set; }
        public string SiteName { get; set; }
        public string ElectionName { get; set; }
        public bool IsActive { get; set; }
        public bool IsAvailable { get; set; }
        public string Message { get; set; }
    }
}
