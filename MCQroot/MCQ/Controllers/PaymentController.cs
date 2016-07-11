using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using MCQ.Data;
using MCQ.Models;
using Microsoft.AspNet.Identity;
using PaymentProcessor;

namespace MCQ.Controllers
{
    [Authorize]
    [RoutePrefix("api/Payment")]
    public class PaymentController : ApiController
    {
        [Authorize]
        [HttpGet]
        public HttpResponseMessage GetPaymentForCurrentUser()
        {
            using (var service = new McqService())
            {

                return Request.CreateResponse(HttpStatusCode.OK, new ReponseResult()
                {
                    IsSucceed = true,
                    Data = null
                });
            }
        }

        [Authorize]
        [HttpPost]
        public async Task<HttpResponseMessage> Pay(PaymentViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return Request.CreateResponse(HttpStatusCode.OK, new ReponseResult()
                {
                    IsSucceed = false,
                    Data = ModelState
                });
            }
            var responseMessage = string.Empty;
            var wasPaid = new PaymentGatewayProvider().CardPayment(model.CardNumber, model.Cvv, model.ExpiryMonth.ToString(),
                model.ExpiryYear.ToString(), decimal.Parse(ConfigurationManager.AppSettings["PaymentAmount"]), ref responseMessage);
            if (!wasPaid)
            {
                return Request.CreateResponse(HttpStatusCode.OK, new ReponseResult()
                {
                    IsSucceed = false,
                    Data = responseMessage
                });
            }
            HttpResponseMessage result;
            using (var service = new McqService())
            {
                result = await service.Pay(User.Identity.GetUserId()).ContinueWith((e) => Request.CreateResponse(HttpStatusCode.OK, new ReponseResult()
                {
                    IsSucceed = e.Status != TaskStatus.Faulted,
                    Data = null
                }));

            }
            return result;
        }
    }
}
