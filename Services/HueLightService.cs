using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HueApi;
using HueApi.BridgeLocator;
using HueApi.ColorConverters;
using HueApi.Extensions;
using HueApi.Models.Requests;
using Microsoft.Extensions.Configuration;
using Microsoft.Graph;

namespace Tehotasapaino.Models
{
    public class HueLightService
    {
        private readonly IConfiguration _config;
        private readonly UserService _userService;
        public HueLightService(IConfiguration config, UserService userService)
        {
            _config = config;
            _userService = userService;
        }

        public async Task SetAlertLightToDesiredState(User userFromAzureAD)
        {
            UserLightAlertClient userLightClient = await InitializeAlertLightClientForSingleUserAsync(userFromAzureAD);
            await TurnLightToDesiredState(userLightClient);
        }

        public async Task SetAlertLightToDesiredState(UserInformation userFromDb)
        {

            await TurnLightToDesiredState(userLightClient);
        }

        private async Task<UserLightAlertClient> InitializeAlertLightClientForSingleUserAsync(User userFromAzureAD) 
        {
            UserInformation dbUser = await _userService.GetDbUserWithTokenAndAlertLightDataAsync(userFromAzureAD);
            return new UserLightAlertClient(dbUser);
        }

        private static async Task TurnLightToDesiredState(RemoteHueApi AuthenticatedHueApiClient)
        {

            var lights = await AuthenticatedHueApiClient.GetLightsAsync();

            Console.WriteLine(lights);

            var id = lights.Data.SingleOrDefault(x => x.Metadata.Name.Contains("Alertlamppu")).Id;

            var req = new UpdateLight().TurnOff();
            var result = await AuthenticatedHueApiClient.UpdateLightAsync(id, req);
            Console.WriteLine(result);
        }

    }
    
    public class UserLightAlertClient
    {
        public RemoteHueApi UserHueClient { get; set; }
        public Guid UserAlertLight { get; set; }

        public UserLightAlertClient(UserInformation dbUser) 
        {
            string userHueUsername = dbUser.UserExternalAPITokens.SingleOrDefault(x => x.ProviderName == "Hue").UserNameProvider;
            string userBearerToken = dbUser.UserExternalAPITokens.SingleOrDefault(x => x.ProviderName == "Hue").Access_token;
            string alertLightId = dbUser.UserAlertLightInformation.LightGUID;

            this.UserHueClient = new RemoteHueApi(userHueUsername, userBearerToken);
            this.UserAlertLight = Guid.Parse(alertLightId);
        }
    }
}
