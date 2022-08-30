using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HueApi;
using HueApi.BridgeLocator;
using HueApi.ColorConverters.Original.Extensions;
using HueApi.Extensions;
using HueApi.Models.Requests;
using HueApi.Models;
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

        public async Task<HuePutResponse> SetAlertLightToDesiredState(User userFromAzureAD, LightStateFromTestPage requestedLightState)
        {
            UserLightAlertClient userLightClient = await InitializeAlertLightClientForSingleUserAsync(userFromAzureAD);
            return await TurnLightToDesiredState(userLightClient, requestedLightState);
        }

        private async Task SetAlertLightToDesiredState(UserInformation userFromDb)
        {
            //TODO
            return;
        }
        private async Task<HuePutResponse> TurnLightToDesiredState(UserLightAlertClient AuthenticatedHueApiClient, LightStateFromTestPage requestedLightState)
        {

            UpdateLight newLightStateReq = new UpdateLight()
                                        .TurnOn()
                                        .SetColor(new HueApi.ColorConverters.RGBColor(requestedLightState.AlertLightHexColor));
            Guid userLightId = AuthenticatedHueApiClient.UserAlertLight;

            HuePutResponse result = await AuthenticatedHueApiClient.UserHueClient.UpdateLightAsync(userLightId, newLightStateReq);
            return result;
        }

        private async Task<UserLightAlertClient> InitializeAlertLightClientForSingleUserAsync(User userFromAzureAD) 
        {
            UserInformation dbUser = await _userService.GetDbUserWithTokenAndAlertLightDataAsync(userFromAzureAD);
            return new UserLightAlertClient(dbUser);
        }

        private void LightRequestBuilder(string HEXColorCode) 
        {
            UpdateLight req = new UpdateLight().TurnOn().SetColor(new HueApi.ColorConverters.RGBColor(HEXColorCode));
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
