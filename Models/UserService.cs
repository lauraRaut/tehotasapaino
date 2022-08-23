using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;


namespace active_directory_aspnetcore_webapp_openidconnect_v2.Models
{
    public class UserService
    {
        private readonly TehotasapainoContext _DbContext;

        public UserService(TehotasapainoContext context)
        {
            _DbContext = context;
        }

        public async Task<bool> CheckUser(string email)
        {
            UserInformation dbUser = await _DbContext.UserData.Where(x => x.Email == email).FirstOrDefaultAsync();

            if(dbUser == null)
            {

            }

          

        }
    }
}
