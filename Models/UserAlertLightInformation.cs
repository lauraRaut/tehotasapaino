using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Tehotasapaino.Models
{
    public class UserAlertLightInformation
    {
        public int UserAlertLightInformationId { get; set; }
        public int UserInformationId { get; set; }
        public UserInformation UserInformation { get; set; }
        public string LightGUID { get; set; }
        public string LightAlertHexColor { get; set; }
        public string LightBeforeAlertHexColor { get; set; }
        public int AlertPriceLevel { get; set; }
    }
}
