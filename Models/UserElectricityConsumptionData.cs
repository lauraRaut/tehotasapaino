using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Tehotasapaino.Models
{
    public class UserElectricityConsumptionData
    {
        public int UserElectricityConsumptionDataId { get; set; }
        public int UserInformationId { get; set; }
        public UserInformation UserInformation { get; set; }

        public int WeekNum { get; set; }
        public int WeekDay { get; set; }
        public int Hour { get; set; }
        public decimal AverageConsumptionkWh { get; set; }
    }
}
