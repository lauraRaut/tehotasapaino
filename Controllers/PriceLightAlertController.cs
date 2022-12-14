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
using HueApi.Models;
using System;
using Tehotasapaino.Models;
using TempDataExtensions;

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
            User user = await _graphServiceClient.Me.Request().GetAsync();
            ViewData["ApiResult"] = user.DisplayName;

            try
            {

                UserPriceAlertConfiguratorViewModel priceAlertViewModel = await _userService.CreatePriceAlertViewModel(user);
                return View(priceAlertViewModel);

            }
            catch (Exception)
            {

                TempData.Put("UserMessage", new SuccessMessage()
                { CssClassName = "alert-danger", Title = "Error!", DisplayMessage = $"You have no access to service" });
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpPost, ActionName("lightstate")]
        [AuthorizeForScopes(ScopeKeySection = "DownstreamApi:Scopes")]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> SetLightState([FromBody] LightStateFromTestPage requestedLightState)
        {
            _logger.LogInformation($"POST lightstate received {requestedLightState}");
            try
            {
                User user = await _graphServiceClient.Me.Request().GetAsync();
                HuePutResponse response = await _hueLightService.SetAlertLightToDesiredState(user, requestedLightState);

                if (response.HasErrors)
                {
                    _logger.LogInformation($"Problems {response.Errors}");
                    return StatusCode(418, $"{response.Data} and {response.Errors}");
                }

                return Ok();

            }
            catch (Exception e)
            {
                _logger.LogInformation($"Problems {e}");
                return StatusCode(500, e.StackTrace);
            }
        }

        [HttpPost, ActionName("savealertprice")]
        [AuthorizeForScopes(ScopeKeySection = "DownstreamApi:Scopes")]
        public async Task<IActionResult> SaveAlertPrice()
        {
            _logger.LogInformation($"POST save price received");

            User user = await _graphServiceClient.Me.Request().GetAsync();

            TempData.Put("UserMessage", new SuccessMessage()
            { CssClassName = "alert-success", Title = "Success!", DisplayMessage = $"{user.DisplayName} your new alert price have been set!" });
            return RedirectToAction("Index", "Home");
        }
    }
}
