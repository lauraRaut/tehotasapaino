using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Tehotasapaino.Models
{
    //olemassaoleva käyttäjä
    public class UserInformation
    {
        public int UserInformationId { get; set; }

        [Required (ErrorMessage = "Enter valid email address")]
        public string Email { get; set; }

        public bool HasUploadedData { get; set; }

        public ICollection<UserElectricityConsumptionData> UserElectricityConsumptionDatas { get; set; }
        public ICollection<UserExternalAPIToken> UserExternalAPITokens { get; set; }


    }
}
