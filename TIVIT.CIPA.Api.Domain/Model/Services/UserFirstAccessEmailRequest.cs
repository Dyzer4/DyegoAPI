using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TIVIT.CIPA.Api.Domain.Model.Services
{
    public record UserFirstAccessEmailRequest(string Name, string Email, string TempPassword);
}
