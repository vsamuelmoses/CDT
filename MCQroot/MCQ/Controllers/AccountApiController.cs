using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

using System.Web.UI.WebControls;
using MCQ.Code;
using MCQ.Data;
using MCQ.Domain;
using MCQ.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;


namespace MCQ.Controllers
{
    [System.Web.Http.Authorize]
    [System.Web.Http.RoutePrefix("api/Account")]
    public class AccountApiController : ApiController
    {
        [System.Web.Http.HttpGet]
        [System.Web.Http.AllowAnonymous]
        [Route("getCurrentUser")]
        public virtual HttpResponseMessage GetCurrentUser(string name = null)
        {

            var store = new UserStore<ApplicationUser>(new ApplicationDbContext());
            var userManager = new UserManager<ApplicationUser>(store);
            ApplicationUser user = userManager.FindByNameAsync(name ?? User.Identity.Name).Result;


            return Request.CreateResponse(HttpStatusCode.OK, user == null ? new ReponseResult() { IsSucceed = false, Data = null } : new ReponseResult()
            {
                IsSucceed = true,
                Data = new RegisterViewModel()
                    {
                        Email = user.Email,
                        FirstName = user.FirstName,
                        LastName = user.LastName,
                        ShouldSetPassword = user.PhoneNumberConfirmed
                    }
            });
        }

        [System.Web.Http.HttpGet]
        [System.Web.Http.AllowAnonymous]
        [Route("getPaymentInfo")]
        public virtual HttpResponseMessage GetPaymentInfo()
        {

            var store = new UserStore<ApplicationUser>(new ApplicationDbContext());
            var userManager = new UserManager<ApplicationUser>(store);
            ApplicationUser user = userManager.FindByNameAsync(User.Identity.Name).Result;

            using (var service = new McqService())
            {
                return Request.CreateResponse(HttpStatusCode.OK,
                    user == null
                        ? new ReponseResult() { IsSucceed = false, Data = null }
                        : new ReponseResult()
                        {
                            IsSucceed = true,
                            Data = service.GetPaymentInfo(user, user.FreeUsagePeriodMinutes, int.Parse(ConfigurationManager.AppSettings["PaidUsagePeriodDays"]))
                        });
            }
        }

        [System.Web.Http.HttpPost]
        [System.Web.Http.AllowAnonymous]
        [Route("register")]
        public virtual async Task<HttpResponseMessage> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser { UserName = model.Email, Email = model.Email, FirstName = model.FirstName, LastName = model.LastName, CreateDate = DateTime.UtcNow, FreeUsagePeriodMinutes = int.Parse(ConfigurationManager.AppSettings["FreeUsagePeriodMinutes"]) };
                var result = await Request.GetOwinContext().Get<ApplicationUserManager>().CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    await Request.GetOwinContext().Get<ApplicationSignInManager>().SignInAsync(user, isPersistent: false, rememberBrowser: false);

                    return Request.CreateResponse(HttpStatusCode.OK, new ReponseResult() { IsSucceed = true, Data = model });
                }
                AddErrors(result);
                return Request.CreateResponse(HttpStatusCode.OK, new ReponseResult() { IsSucceed = false, Data = result });
            }

            // If we got this far, something failed, redisplay form
            return Request.CreateResponse(HttpStatusCode.OK, new ReponseResult() { IsSucceed = false, Data = ModelState });
        }

        [System.Web.Http.HttpPost]
        [System.Web.Http.AllowAnonymous]
        [Route("logIn")]
        public async Task<HttpResponseMessage> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return Request.CreateResponse(HttpStatusCode.OK, new ReponseResult() { IsSucceed = false, Data = ModelState });
            }

            var result = await Request.GetOwinContext().Get<ApplicationSignInManager>().PasswordSignInAsync(model.Email, model.Password, true, shouldLockout: false);
            switch (result)
            {
                case SignInStatus.Success:
                    return GetCurrentUser(model.Email);
                default:
                    ModelState.AddModelError("0", "Invalid login attempt.");
                    return Request.CreateResponse(HttpStatusCode.OK, new ReponseResult() { IsSucceed = false, Data = ModelState });
            }
        }

        [System.Web.Http.HttpPost]
        [System.Web.Http.AllowAnonymous]
        [Route("logInExternal")]
        public async Task<HttpResponseMessage> LoginExternal(ExternalAccountViewModel model)
        {
            var webClient = new WebClient();
            dynamic faceBookResponse = System.Web.Helpers.Json.Decode(webClient.DownloadString(string.Format("https://graph.facebook.com/me?access_token={0}", model.AccessToken)));
            if (faceBookResponse.id != null && faceBookResponse.id != model.Id.ToString())
            {
                return Request.CreateResponse(HttpStatusCode.OK, new ReponseResult() { IsSucceed = false, Data = "Malformed access token" });
            }

            if (!GetCurrentUser(model.Email).Content.ReadAsAsync<ReponseResult>().Result.IsSucceed)
            {
                var appUser = new ApplicationUser()
                {
                    EmailConfirmed = true,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    UserName = model.Email,
                    Email = model.Email,
                    CreateDate = DateTime.UtcNow,
                    FreeUsagePeriodMinutes = int.Parse(ConfigurationManager.AppSettings["FreeUsagePeriodMinutes"])
                };
                var result = await Request.GetOwinContext().Get<ApplicationUserManager>().CreateAsync(appUser);
                if (!result.Succeeded)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, new ReponseResult() { IsSucceed = false, Data = "Unable to create user" });
                }
                result = await Request.GetOwinContext().Get<ApplicationUserManager>().AddLoginAsync(appUser.Id, new UserLoginInfo("facebook", appUser.Email));

                if (!result.Succeeded)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, new ReponseResult() { IsSucceed = false, Data = "Unable to create user" });
                }
            }

            var loginResult = await Request.GetOwinContext().Get<ApplicationSignInManager>().ExternalSignInAsync(new ExternalLoginInfo()
            {
                Email = model.Email,
                DefaultUserName = model.Email,
                Login = new UserLoginInfo("facebook", model.Email)
            }, true);

            switch (loginResult)
            {
                case SignInStatus.Success:
                    return Request.CreateResponse(HttpStatusCode.OK, new ReponseResult()
                    {
                        IsSucceed = true,
                        Data = new ApplicationUser()
                        {
                            EmailConfirmed = true,
                            FirstName = model.FirstName,
                            LastName = model.LastName,
                            Email = model.Email
                        }
                    });
                default:
                    return Request.CreateResponse(HttpStatusCode.OK, new ReponseResult() { IsSucceed = false, Data = "Unexpected error" });
            }
        }

        [System.Web.Http.HttpGet]
        [Route("logOff")]
        public HttpResponseMessage LogOff()
        {
            Request.GetOwinContext().Authentication.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            return Request.CreateResponse(HttpStatusCode.OK);
        }

        [System.Web.Http.HttpPost]
        [Route("resetPassword")]
        [AllowAnonymous]
        public async Task<HttpResponseMessage> ResetPassword(dynamic data)
        {

            ApplicationUser user = await Request.GetOwinContext().Get<ApplicationUserManager>().FindByNameAsync(data.email.ToString());
            if (user == null)
            {
                // Don't reveal that the user does not exist or is not confirmed
                return Request.CreateResponse(HttpStatusCode.OK, new ReponseResult() { IsSucceed = false, Data = "User does not exist" });
            }
            if (user.PasswordHash == null)
            {
                return Request.CreateResponse(HttpStatusCode.OK, new ReponseResult() { IsSucceed = false, Data = "User does not exist" });
            }

            string code = await Request.GetOwinContext().Get<ApplicationUserManager>().GeneratePasswordResetTokenAsync(user.Id);


            var pwd = Guid.NewGuid().ToString("n").Substring(0, 6);
            await Request.GetOwinContext().Get<ApplicationUserManager>().ResetPasswordAsync(user.Id, code, pwd);
            user.PhoneNumberConfirmed = true;
            Request.GetOwinContext().Get<ApplicationUserManager>().Update(user);

            EmailHelper.Send(user.Email, "Reset Password", string.Format("Hi your new password is {0}.", pwd));
            return Request.CreateResponse(HttpStatusCode.OK, new ReponseResult() { IsSucceed = true });
        }

        [System.Web.Http.HttpPost]
        [Route("updatePassword")]
        public async Task<HttpResponseMessage> UpdatePassword(dynamic data)
        {
            ApplicationUser user = await Request.GetOwinContext().Get<ApplicationUserManager>().FindByNameAsync(User.Identity.Name);

            string code = await Request.GetOwinContext().Get<ApplicationUserManager>().GeneratePasswordResetTokenAsync(user.Id);
            user.PhoneNumberConfirmed = false;
            Request.GetOwinContext().Get<ApplicationUserManager>().Update(user);

           
                await
                    Request.GetOwinContext()
                        .Get<ApplicationUserManager>()
                        .ResetPasswordAsync(user.Id, code, data.password.ToString());
            
            return Request.CreateResponse(HttpStatusCode.OK, new ReponseResult() { IsSucceed = true });
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }
    }
}