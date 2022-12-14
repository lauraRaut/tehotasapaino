using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Tehotasapaino.Models
{
    public class TehotasapainoContext : DbContext
    {
        public DbSet<UserInformation> UserData { get; set; }
        public DbSet<UserElectricityConsumptionData> UserConsumptionData { get; set; }
        public DbSet<UserExternalAPIToken> UserExternalAPITokens { get; set; }
        public DbSet<UserAlertLightInformation> UserAlertLightInformation { get; set; }


        public TehotasapainoContext(DbContextOptions<TehotasapainoContext> options) : base(options) { }
    }
}
