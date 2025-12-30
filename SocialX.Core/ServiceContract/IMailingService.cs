using Microsoft.AspNetCore.Http;
using SocialX.Core.Domain.IdentityEntites;
using SocialX.Core.DTO.AuthenticationDTO;
using SocialX.Core.storeCore.Domain.IdentityEntites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialX.Core.ServiceContract
{
    public interface IMailingService
    {
        Task SendMessageAsync(string mailTo, string subject, string body, IList<IFormFile>? attach);
    }
}
