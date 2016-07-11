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
using MCQ.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;


namespace MCQ.Controllers
{
    //[System.Web.Http.Authorize]
    [System.Web.Http.RoutePrefix("api/Topic")]
    public class TopicApiController : ApiController
    {
        [HttpGet]
        [AllowAnonymous]
        [Route("getTopics")]
        public HttpResponseMessage GetTopics()
        {
            using (var service = new McqService())
            {
                if (!User.Identity.IsAuthenticated)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, new ReponseResult()
                    {
                        IsSucceed = true,
                        Data = service.GetTopics().Select(x => new TopicViewModel
                        {
                            Topic = x,
                            Progress = 0
                        })
                    });
                }
                else
                {

                    return Request.CreateResponse(HttpStatusCode.OK, new ReponseResult()
                    {
                        IsSucceed = true,
                        Data = service.GetTopics().Select(x => new TopicViewModel
                        {
                            Topic = x,
                            Progress = service.GetUserProgressForTopic(User.Identity.GetUserId(), x.Id)

                        }).ToList()
                    });
                }
            }
        }

        [HttpGet]
        [Route("getTopicById/{id}")]
        public HttpResponseMessage GetTopicById(int id)
        {
            using (var service = new McqService())
            {
                return Request.CreateResponse(HttpStatusCode.OK, new ReponseResult()
                {
                    IsSucceed = true,
                    Data = new
                    {
                        Topics = service.GetTopics().FirstOrDefault(x => x.Id == id),
                        QuestionsCount = service.GetTotalQuestionsForTopic(id)
                    }
                });
            }
        }

        //[Authorize]
        [AllowAnonymous]
        [System.Web.Http.HttpGet]
        [Route("getTopicInfo/{id}")]
        
    
        public HttpResponseMessage GetTopicInfo(int id)
        {
            if (User.Identity.IsAuthenticated)
            {
                using (var service = new McqService())
                {
                    return Request.CreateResponse(HttpStatusCode.OK, new ReponseResult()
                    {
                        IsSucceed = true,
                        Data = new TopicSummaryViewModel()
                        {
                            Progress = service.GetUserProgressForTopic(User.Identity.GetUserId(), id),
                            QuestionsCount = service.GetTotalQuestionsForTopic(id),
                            SuccessRate = service.GetSuccessRateAnswersForTopic(User.Identity.GetUserId(), id)
                        }
                    });
                }
            }
            else
            {
                using (var service = new McqService())
                {
                    return Request.CreateResponse(HttpStatusCode.OK, new ReponseResult()
                    {
                        IsSucceed = true,
                        Data = new TopicSummaryViewModel()
                        {
                            Progress = 0,
                            QuestionsCount = service.GetTotalQuestionsForTopic(id),
                            SuccessRate = 0
                        }
                    });
                }
            }
        }
        //[Authorize]
        [AllowAnonymous]
        [System.Web.Http.HttpGet]
        [Route("getDashboardInfo")]
        
    
        public HttpResponseMessage GetDashboardInfo()
        {
            if (User.Identity.IsAuthenticated)
            {
                using (var service = new McqService())
                {
                    dynamic response = service.GetDashboardData(User.Identity.GetUserId());
                    return Request.CreateResponse(HttpStatusCode.OK, new ReponseResult()
                    {
                        IsSucceed = true,

                        Data = new DashboardViewModel()
                        {
                            Progress = response.Progress,
                            QuestionsCount = response.QuestionsCount,
                            SuccessRate = response.SuccessRate,
                            TopicsCount = response.TopicsCount
                        }
                    });
                }
            }
            else
            {
                using (var service = new McqService())
                {
                    dynamic response = service.GetDashboardData();
                    return Request.CreateResponse(HttpStatusCode.OK, new ReponseResult()
                    {
                        IsSucceed = true,

                        Data = new DashboardViewModel()
                        {
                            Progress = 0,
                            QuestionsCount = response.QuestionsCount,
                            SuccessRate = 0,
                            TopicsCount = response.TopicsCount
                        }
                    });
                }
            }
        }

        [Authorize]
        [HttpGet]
        [Route("resetProgressForTopic/{id}")]
        public HttpResponseMessage ResetProgressForTopic(int id)
        {
            using (var service = new McqService())
            {
                service.ResetProgressForTopic(User.Identity.GetUserId(), id);
                return Request.CreateResponse(HttpStatusCode.OK, new ReponseResult()
                {
                    IsSucceed = true,
                    Data = null
                });
            }
        }

        [Authorize]
        [HttpGet]
        [Route("resetProgress")]
        public HttpResponseMessage ResetProgress()
        {
            using (var service = new McqService())
            {
                service.ResetProgress(User.Identity.GetUserId());
                return Request.CreateResponse(HttpStatusCode.OK, new ReponseResult()
                {
                    IsSucceed = true,
                    Data = null
                });
            }
        }
    }
}