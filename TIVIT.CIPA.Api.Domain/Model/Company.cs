namespace TIVIT.CIPA.Api.Domain.Model
{
    public class Company
    {
        public int Id { get; set; }
        public string CNPJ { get; set; }
        public string LegalName { get; set; }
        public string ProtheusCode { get; set; }
        public string Name { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsActive { get; set; } = true;

        public ICollection<Site> Sites { get; set; }
    }
}