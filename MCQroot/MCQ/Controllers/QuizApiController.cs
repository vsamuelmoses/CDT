using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using MCQ.Code;
using MCQ.Data;
using MCQ.Models;
using Microsoft.AspNet.Identity;

namespace MCQ.Controllers
{
    [PaymentCheck]
    [Authorize]
    [RoutePrefix("api/Quiz")]
    public class QuizApiController : ApiController
    {

        [Authorize]
        [System.Web.Http.HttpGet]
        [Route("getQuestionForTopic/{id}/{questionId}/{p}/{s}")]
        public HttpResponseMessage GetQuestionForTopic(int id, int questionId, int p, bool s)
        {
            try
            {
                using (var service = new McqService())
                {
                    var data = service.GetNextQuestionForTopic(id, User.Identity.GetUserId(), questionId, p, s);
                    if (data == null)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, new ReponseResult()
                        {
                            IsSucceed = true,
                            Data = null
                        });
                    }
                    return Request.CreateResponse(HttpStatusCode.OK, new ReponseResult()
                    {
                        IsSucceed = true,
                        Data = new QuestionViewModel()
                        {
                            Question = data,
                            IsCorrect = service.IsAnswerForQuestionCorrect(User.Identity.GetUserId(), data.Id),
                            UserAnswers = service.GetUserAnswers(User.Identity.GetUserId(), data.Id)
                        }
                    });
                }
            }
            catch 
            {
                return Request.CreateResponse(HttpStatusCode.OK, new ReponseResult()
                {
                    IsSucceed = true,
                    Data = null
                });
            }
        }

        [Authorize]
        [System.Web.Http.HttpGet]
        [Route("getPrevQuestionForTopic/{id}/{questionId}/{p}/{s}")]
        public HttpResponseMessage GetPrevQuestionForTopic(int id, int questionId, int p, bool s)
        {
            using (var service = new McqService())
            {
                var data = service.GetPrevQuestionForTopic(id, User.Identity.GetUserId(), questionId, p, s);
                return Request.CreateResponse(HttpStatusCode.OK, new ReponseResult()
                {
                    IsSucceed = true,
                    Data = new QuestionViewModel()
                    {
                        Question = data,
                        IsCorrect = service.IsAnswerForQuestionCorrect(User.Identity.GetUserId(), data.Id),
                        UserAnswers = service.GetUserAnswers(User.Identity.GetUserId(), data.Id)
                    }
                });
            }
        }

        [Authorize]
        [System.Web.Http.HttpGet]
        [Route("getQuizQuestionsData/{id}")]
        public HttpResponseMessage GetQuizQuestionsData(int id)
        {
            using (var service = new McqService())
            {
                var data = service.GetQuizQuestionsData(id, User.Identity.GetUserId());
                return Request.CreateResponse(HttpStatusCode.OK, new ReponseResult()
                {
                    IsSucceed = true,
                    Data = data
                });
            }
        }

        //[Authorize]
        //[System.Web.Http.HttpGet]
        //[Route("markQuestionAsRead/{id}")]
        //public async Task<HttpResponseMessage> MarkQuestionAsRead(int id)
        //{
        //    using (var service = new McqService())
        //    {
        //        return Request.CreateResponse(HttpStatusCode.OK, new ReponseResult()
        //        {
        //            IsSucceed = await service.MarkQuestionAsRead(User.Identity.GetUserId(), id) > 0,
        //            Data = null
        //        });
        //    }
        //}

        [Authorize]
        [System.Web.Http.HttpGet]
        [Route("changeFlagState/{id}")]
        public async Task<HttpResponseMessage> ChangeFlagState(int id)
        {
            using (var service = new McqService())
            {
                return Request.CreateResponse(HttpStatusCode.OK, new ReponseResult()
                {
                    IsSucceed = await service.ChangeFlagState(User.Identity.GetUserId(), id) > 0,
                    Data = null
                });
            }
        }

        [Authorize]
        [HttpPost]
        [Route("revealAnswer")]
        public async Task<HttpResponseMessage> RevealAnswer(RevealAnswerViewModel vm)
        {
            using (var service = new McqService())
            {
                var isCorrect = service.RevealAnswer(User.Identity.GetUserId(), vm.QuestionId, vm.Answers);
                await service.MarkQuestionAsRead(User.Identity.GetUserId(), vm.QuestionId, isCorrect);

                return Request.CreateResponse(HttpStatusCode.OK, new ReponseResult()
                {
                    IsSucceed = isCorrect,
                    Data = null
                });
            }
        }

    }
}
