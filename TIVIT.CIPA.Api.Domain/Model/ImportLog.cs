namespace TIVIT.CIPA.Api.Domain.Model
{
    public class ImportLog
    {
        public int Id { get; set; }
        public string RegistryUser { get; set; }
        public DateTime Timestamp { get; set; }
        public string ObjectType { get; set; }
        public int? RecordsInserted { get; set; }
        public int? RecordsUpdated { get; set; }
        public string FileName { get; set; }
        public string Details { get; set; }
    }
}
