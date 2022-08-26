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
        public string APIToken { get; set; }
        public DateTime LastRefreshTimeStamp { get; set; }
        public int ExpirationTime { get; set; }


    }
}
