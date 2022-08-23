using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Graph;


namespace active_directory_aspnetcore_webapp_openidconnect_v2.Models
{
    public class UserService
    {
        private readonly TehotasapainoContext _DbContext;

        public UserService(TehotasapainoContext context)
        {
            _DbContext = context;
        }

        public async Task<bool> CheckUserExistDbAsync(string email)
        {
            UserInformation dbUser = await _DbContext.UserData.FirstOrDefaultAsync(x => x.Email == email);

            if (dbUser == null)
            {
                return false;
            }
            return true;
        }

        public async Task AddUserToDb(string email) 
        {
            bool userExcists = await CheckUserExistDbAsync(email);
            if (!userExcists)
            {
                UserInformation newUser = new UserInformation()
                {
                    Email = email,
                    HasUploadedData = false
                };

                _DbContext.UserData.Add(newUser);
                await _DbContext.SaveChangesAsync();
            }
            else
            {
                throw new ArgumentException($"User found with email {email}");
            }
        }

        public async Task<IndexViewModel> CreateIndexViewModel(User userFromAzureAD) 
        {
            bool userExcists = await CheckUserExistDbAsync(userFromAzureAD.Mail);
            IndexViewModel newIndexViewModel = new IndexViewModel(userFromAzureAD, userExcists);

            return newIndexViewModel;
        }
    }
}
