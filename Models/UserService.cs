using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Graph;
using Microsoft.AspNetCore.Http;
using static Tehotasapaino.Models.DayAheadPrice;

namespace Tehotasapaino.Models
{
    public class UserService
    {
        private readonly TehotasapainoContext _DbContext;
        private readonly UserElectricityConsumptionDataService _ConsumptionData;
        public UserService(TehotasapainoContext context, UserElectricityConsumptionDataService dataService )
        {
            _DbContext = context;
            _ConsumptionData = dataService;
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

        public async Task AddUserAndUserConsumptionDataToDb(string email, IFormFile fileFromUser) 
        {
            bool userExcists = await CheckUserExistDbAsync(email);
            if (!userExcists)
            {
                UserInformation newUser = new UserInformation()
                {
                    Email = email,
                    HasUploadedData = true,
                    UserElectricityConsumptionDatas = _ConsumptionData.GetUserElectricityWeekDayHourAverages(fileFromUser)
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
            PriceProcessor processor = new PriceProcessor();
            List<Point> nextDayPrices = processor.GetPricesPerSearch();
           // metodikutsu dayahead pricelle
            IndexViewModel newIndexViewModel = new IndexViewModel(userFromAzureAD, userExcists, nextDayPrices);


            return newIndexViewModel;
        }
    }
}
