using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Tehotasapaino.Models
{
    public class UserExternalAPIToken
    {
        public int UserExternalAPITokenId { get; set; }
        public int UserInformationId { get; set; }
        public UserInformation UserInformation { get; set; }
        public string ProviderName { get; set; }
        public DateTime CreatedDate { get; set; }
        public string Access_token { get; set; }
        public int Expires_in { get; set; }
        public string Refresh_token { get; set; } = "";
        public string UserNameProvider { get; set; }
    }
}
