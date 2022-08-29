using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Identity.Web;
using Microsoft.Graph;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using System;
using Tehotasapaino.Models;

namespace Tehotasapaino.Controllers
{
    [Authorize]
    public class PriceLightAlertController : Controller
    {
        private readonly ILogger<PriceLightAlertController> _logger;
        private readonly GraphServiceClient _graphServiceClient;
        private readonly UserService _userService;
        private readonly HueLightService _hueLightService;

        public PriceLightAlertController(ILogger<PriceLightAlertController> logger,
                          GraphServiceClient graphServiceClient, UserService userService,
                           HueLightService hueLightService)
        {
            _logger = logger;
            _graphServiceClient = graphServiceClient;
            _userService = userService;
            _hueLightService = hueLightService;
       }

        [AuthorizeForScopes(ScopeKeySection = "DownstreamApi:Scopes")]
        public async Task<IActionResult> UserPriceAlertConfigurator()
        {
            var user = await _graphServiceClient.Me.Request().GetAsync();
            ViewData["ApiResult"] = user.DisplayName;

            UserPriceAlertConfiguratorViewModel priceAlertViewModel = await _userService.CreatePriceAlertViewModel(user);

            return View(priceAlertViewModel);
        }

        [HttpPost, ActionName("lightstate")]
        [AuthorizeForScopes(ScopeKeySection = "DownstreamApi:Scopes")]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> SetLightState(LightState requestedLightState)
        {
            _logger.LogInformation($"POST lightstate received {requestedLightState}");
            try
            {
                var user = await _graphServiceClient.Me.Request().GetAsync();
                UserExternalAPIToken userHueAPIToken = await _userService.GetUserExternalAPITokenDataAsync(user, "Hue");
                
                try
                {
                    await _hueLightService.ControlLightState(requestedLightState, userHueAPIToken);
                    return Ok();
                }
                catch (Exception e)
                {
                    _logger.LogInformation($"Error controlling the lights {e}");
                    return StatusCode(418);
                }
            }
            catch (Exception e)
            {
                _logger.LogInformation($"User not authorized {e}");
                return StatusCode(401);
            }
        }
    }
}
