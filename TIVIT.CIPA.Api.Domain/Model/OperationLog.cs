using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TIVIT.CIPA.Api.Domain.Model
{
    public class OperationLog
    {
        public int Id { get; set; }
        public string Operation { get; set; }
        public int RecordId { get; set; }
        public string EntityName { get; set; }
        public string UserName { get; set; }
        public DateTime When { get; set; }

        public string ChangesJson { get; set; }
    }
    public class EntityChange
    {
        public string Field { get; set; }
        public string Old { get; set; }
        public string New { get; set; }
    }
}
