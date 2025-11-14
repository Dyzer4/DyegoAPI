namespace TIVIT.CIPA.Api.Domain.Model
{
    public class OperationLog
    {
        public int Id { get; set; }
        public string Operation { get; set; }
        public int? RecordId { get; set; }
        public string EntityName { get; set; }
        public string UserName { get; set; }
        public DateTime When { get; set; }
        public string ChangesJson { get; set; }
    }
}
