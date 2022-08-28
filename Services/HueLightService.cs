using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HueApi;
using HueApi.BridgeLocator;
using HueApi.ColorConverters;
using HueApi.Extensions;
using HueApi.Models.Requests;

namespace Tehotasapaino.Models
{
    public class HueLightService
    {

        public HueLightService()
        {
        }

        public async Task ControlLightState(LightState userRequestedState, UserExternalAPIToken userAPIToken) 
        {
            string userHueUsername = "";
            string userBearerToken = "";
            RemoteHueApi HueClient = new RemoteHueApi(userHueUsername, userBearerToken);

            var bridges = await HueClient.GetBridgesAsync();

            if (bridges == null)
            {
                throw new ArgumentException("No Hue bridge found!");
            }

            if (userRequestedState.isPriceHight)
            {
               await TurnLightToDesiredState(HueClient);
            }
        }

        private static async Task TurnLightToDesiredState(RemoteHueApi AuthenticatedHueApiClient) 
        {

            var lights = await AuthenticatedHueApiClient.GetLightsAsync();
            Console.WriteLine(lights);

            var id = lights.Data.Last().Id;
            var req = new UpdateLight().TurnOff();
            var result = await AuthenticatedHueApiClient.UpdateLightAsync(id, req);
            Console.WriteLine(result);
        }

    }
}
