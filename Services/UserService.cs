using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Graph;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static Tehotasapaino.Models.DayAheadPrice;

namespace Tehotasapaino.Models
{
    public class UserService
    {
        private readonly TehotasapainoContext _DbContext;
        private readonly UserElectricityConsumptionDataService _ConsumptionData;
        public UserService(TehotasapainoContext context, UserElectricityConsumptionDataService dataService)
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

        public async Task<UserElectricityConsumptionData> GetUserElectricityConsumptionData(User userFromAzure)
        {
            throw new Exception("not implemented");

        }


        public async Task<UserInformation> GetDbUserWithTokenData(User userFromAzure)
        {

            UserInformation dbUser = await _DbContext.UserData.Include(token => token.UserExternalAPITokens)
                                                              .FirstOrDefaultAsync(x => x.Email == userFromAzure.Mail);
            return dbUser;
        }

        public UserExternalAPIToken GetTokenFromUser(UserInformation userData, string service)
        {

            UserExternalAPIToken userTokenData = userData.UserExternalAPITokens.FirstOrDefault(x => x.ProviderName == service);
            return userTokenData;
        }

        public async Task<UserExternalAPIToken> GetUserExternalAPITokenData(User userFromAzure, string service)
        {

            UserInformation dbUser = await this.GetDbUserWithTokenData(userFromAzure);

            if (dbUser == null)
            {

                throw new ArgumentException("User not found");

            }

            UserExternalAPIToken userToken = this.GetTokenFromUser(dbUser, service);

            if (userToken == null)
            {
                throw new ArgumentException("User has no access to this service");
            }

            return userToken;

        }


        public async Task<IndexViewModel> CreateIndexViewModel(User userFromAzureAD)
        {
            bool userExcists = await CheckUserExistDbAsync(userFromAzureAD.Mail);
            PriceProcessorService processor = new PriceProcessorService();
            List<Point> nextDayPrices = processor.GetPricesPerSearch();
            //TODO metodikutsu dayahead pricelle
            IndexViewModel newIndexViewModel = new IndexViewModel(userFromAzureAD, userExcists, nextDayPrices);


            return newIndexViewModel;
        }

        public async Task<UserPriceAlertConfiguratorViewModel> CreatePriceAlertViewModel(User userFromAzureAD)
        {
            bool isUserInPriceAlertProgram = false;
            UserInformation dbUser = await this.GetDbUserWithTokenData(userFromAzureAD);
            if (dbUser != null)
            {
                isUserInPriceAlertProgram = true;
            }


            PriceProcessorService processor = new PriceProcessorService();
            List<Point> nextDayPrices = processor.GetPricesPerSearch();
            UserPriceAlertConfiguratorViewModel UserPriceAlertViewModel = new UserPriceAlertConfiguratorViewModel(userFromAzureAD, isUserInPriceAlertProgram, nextDayPrices);


            return UserPriceAlertViewModel;
        }
    }
}
