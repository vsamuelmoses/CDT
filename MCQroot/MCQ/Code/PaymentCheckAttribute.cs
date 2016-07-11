



using System.Configuration;
using System.Net;
using System.Net.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using MCQ.Data;
using MCQ.Domain;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace MCQ.Code
{
    public class PaymentCheckAttribute : ActionFilterAttribute
    {
       public override void OnActionExecuting(HttpActionContext actionContext)
    {
            using (var service = new McqService())
            {
                var store = new UserStore<ApplicationUser>(new ApplicationDbContext());
                var userManager = new UserManager<ApplicationUser>(store);
                ApplicationUser user = userManager.FindByNameAsync(actionContext.RequestContext.Principal.Identity.Name).Result;
                var shouldPay = service.ShouldUserPay(user, user.FreeUsagePeriodMinutes, int.Parse(ConfigurationManager.AppSettings["PaidUsagePeriodDays"]));
                if (shouldPay)
                {
                    actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.PaymentRequired);
                }
                
            }

           
        }

    }
}