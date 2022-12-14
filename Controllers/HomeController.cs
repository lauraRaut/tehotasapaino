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
using TempDataExtensions;

namespace Tehotasapaino.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly UserService _userService;
        private readonly GraphServiceClient _graphServiceClient;


        public HomeController(ILogger<HomeController> logger,
                          GraphServiceClient graphServiceClient, UserService userService)
        {
            _logger = logger;
            _graphServiceClient = graphServiceClient;
            _userService = userService;
       }

        [AuthorizeForScopes(ScopeKeySection = "DownstreamApi:Scopes")]
        public async Task<IActionResult> Index()
        {
            var user = await _graphServiceClient.Me.Request().GetAsync();
            ViewData["ApiResult"] = user.DisplayName;
            IndexViewModel indexViewModel = await _userService.CreateIndexViewModel(user);
            
            return View(indexViewModel);
        }

        [AuthorizeForScopes(ScopeKeySection = "DownstreamApi:Scopes")]
        public async Task<IActionResult> DeleteUser()
        {
            try
            {
                var user = await _graphServiceClient.Me.Request().GetAsync();
                await _userService.DeleteUserFromDbAsync(user);

                TempData.Put("UserMessage", new SuccessMessage()
                { CssClassName = "alert-success", Title = "Success!", DisplayMessage = "You have been terminated!" });

                return RedirectToAction(nameof(Index));
            }

            catch (Exception e)
            {

                TempData.Put("UserMessage", new SuccessMessage()
                { CssClassName = "alert-danger", Title = "Error!", DisplayMessage = $"Termination failed! With error msg:  {e.ToString()}" });
                return RedirectToAction(nameof(Index));
            }
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [AllowAnonymous]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        

        //Uploading file on server
        public async Task<bool> UploadFile(Microsoft.AspNetCore.Http.IFormFile file)
        {
            string path = "";
            //Returning a boolean as a sort of OK that the file saving worked, so that FileUpload() method can take over
            bool isCopied = false;
            try
            {
                //Telling our program that the filename should stay as the name that is uploaded
                string fileName = file.FileName;
                //Created a folder named Upload in the project, and finding it as the path
                path = Path.GetFullPath(Path.Combine(System.IO.Directory.GetCurrentDirectory(), "Upload"));
                using (var fileStream = new FileStream(Path.Combine(path, fileName), FileMode.Create))
                {
                    await file.CopyToAsync(fileStream);
                }

                isCopied = true;
            }
            catch (Exception)
            {
                throw;
            }

            return isCopied;

        }

        //Adding a message that upload succeeded and staying in the Index-view
        [AuthorizeForScopes(ScopeKeySection = "DownstreamApi:Scopes")]
        [HttpPost]
        public async Task<ActionResult> FileUpload(IFormFile fileFromUser)
        {
            try
            {
                var user = await _graphServiceClient.Me.Request().GetAsync();
                //Metodikutsu tiedostonkäsittelijälle
                await _userService.AddUserAndUserConsumptionDataToDbAsync(user.Mail, fileFromUser);

                TempData.Put("UserMessage", new SuccessMessage()
                { CssClassName = "alert-success", Title = "Success!", DisplayMessage = "Thank you for registering! Data saved and analysed! " });


                return RedirectToAction(nameof(Index));
            }

            catch (ArgumentException args) 
            {
                if (args.Message.Contains("User found with email"))
                {
                    TempData.Put("UserMessage", new SuccessMessage()
                    { CssClassName = "alert-danger", Title = "Error!", DisplayMessage = $"File upload failed! You have already uploaded data. Data update is not currently supported." });
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    TempData.Put("UserMessage", new SuccessMessage()
                    { CssClassName = "alert-danger", Title = "Error!", DisplayMessage = $"File upload failed!{args.ToString()}" });
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (Exception e)
            {

                TempData.Put("UserMessage", new SuccessMessage()
                { CssClassName = "alert-danger", Title = "Error!", DisplayMessage = $"File upload failed!{e.ToString()}" });
                return RedirectToAction(nameof(Index));
            }
        }

    }
}
