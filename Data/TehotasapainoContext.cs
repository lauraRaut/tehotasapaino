using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace active_directory_aspnetcore_webapp_openidconnect_v2.Models
{
    public class TehotasapainoContext : DbContext
    {
        public DbSet<UserInformation> UserData { get; set; }
       
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=localhost\;Database=Tehotasapaino;Trusted_Connection=True;MultipleActiveResultSets=true");
        }

    }
}
