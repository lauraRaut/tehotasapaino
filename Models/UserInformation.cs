using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace active_directory_aspnetcore_webapp_openidconnect_v2.Models
{
    //olemassaoleva käyttäjä
    public class UserInformation
    {
        public int UserInformationId { get; set; }

        [Required (ErrorMessage = "Enter valid email address")]
        public string Email { get; set; }

        public bool HasUploadedData { get; set; }


    }
}
