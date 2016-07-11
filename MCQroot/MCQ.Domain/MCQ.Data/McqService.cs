using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MCQ.Domain;

namespace MCQ.Data
{
    public class McqService : IDisposable
    {
        private readonly ApplicationDbContext _ctx;

        public McqService()
        {
            _ctx = new ApplicationDbContext();
            _ctx.Configuration.ProxyCreationEnabled = false;
        }

        public ICollection<Topic> GetTopics()
        {
            return _ctx.Topics.ToList();
        }

        public int GetTotalQuestionsForTopic(int topicId)
        {
            return _ctx.Questions.Count(t => t.Topic.Id == topicId);
        }

        public int GetUserAnsweredQuestionsForTopic(string userId, int topicId)
        {
            return _ctx.UserAnswers.Where(x => x.Question.Topic.Id == topicId && x.User.Id == userId).GroupBy(x => x.Question.Id).Count();
        }

        public int GetCorrectUserAnsweredQuestionsForTopic(string userId, int topicId)
        {
            var r =
                _ctx.UserReadedQuestions.Where(x => x.Question.Topic.Id == topicId && x.User.Id == userId).GroupBy(x => x.Question.Id).Count();
            return r;
            //_ctx.Questions.Where(x => x.Topic.Id == topicId)
            //    .ToList()
            //    .Count(x => IsAnswerForQuestionCorrect(userId, x.Id) == true);
            // return   _ctx.UserAnswers.Where(x => x.Question.Topic.Id == topicId).GroupBy(x => x.Question.Id).Count(x => x.Any(ua => !ua.Answer.IsCorrect));
        }

        public int GetUserProgressForTopic(string userId, int topicId)
        {
            var answeredQuestionsForTopic = GetCorrectUserAnsweredQuestionsForTopic(userId,
                       topicId);

            return answeredQuestionsForTopic == 0
                ? 0
                : (int)Math.Ceiling((((decimal)((decimal)answeredQuestionsForTopic / (decimal)GetTotalQuestionsForTopic(topicId))) * 100));
        }

        public int GetOverallUserProgress(string userId)
        {
            var answeredQuestionsForTopic = _ctx.UserReadedQuestions.Where(x => x.User.Id == userId).GroupBy(x => x.Question.Id).Count();

            return answeredQuestionsForTopic == 0
                ? 0
                : (int)Math.Ceiling((((decimal)(decimal)answeredQuestionsForTopic / (decimal)_ctx.Questions.Count()) * 100));
        }

        public int GetSuccessRateAnswersForTopic(string userId, int topicId)
        {
            var correctAnswersForTopic =
                _ctx.UserReadedQuestions.Count(x => x.Question.Topic.Id == topicId && x.IsCorrect && x.User.Id == userId);
            var incorrectAnswersForTopic =
                _ctx.UserReadedQuestions.Count(x => x.Question.Topic.Id == topicId && !x.IsCorrect && x.User.Id == userId);

            if (correctAnswersForTopic == 0 && incorrectAnswersForTopic > 0)
                return 0;
            if (correctAnswersForTopic > 0 && incorrectAnswersForTopic == 0)
                return 100;
            //if (correctAnswersForTopic > incorrectAnswersForTopic)
            //    return 100;
            if (correctAnswersForTopic > 0 && incorrectAnswersForTopic > 0 && correctAnswersForTopic == incorrectAnswersForTopic)
                return 50;
            if (correctAnswersForTopic > 0 && incorrectAnswersForTopic > 0 &&
                correctAnswersForTopic > incorrectAnswersForTopic)
                return 100-(int)(((decimal)((decimal)incorrectAnswersForTopic / (decimal)correctAnswersForTopic * 100)));
            if (correctAnswersForTopic > 0 && incorrectAnswersForTopic > 0 &&
               correctAnswersForTopic < incorrectAnswersForTopic)
                return (int)(((decimal)((decimal)correctAnswersForTopic / (decimal)incorrectAnswersForTopic * 100)));

            return 0;
            //var answeredQuestionsForTopic = GetCorrectUserAnsweredQuestionsForTopic(userId, topicId);
            //return answeredQuestionsForTopic == 0
            //    ? 0
            //    : (int)(((decimal)((decimal)answeredQuestionsForTopic / (decimal)GetUserAnsweredQuestionsForTopic(userId, topicId))) * 100);
        }

        public int GetSuccessOverAllRateAnswersForTopic(string userId)
        {
            var correctAnswersForTopic =
                _ctx.UserReadedQuestions.Count(x => x.IsCorrect && x.User.Id == userId);
            var incorrectAnswersForTopic =
                _ctx.UserReadedQuestions.Count(x => !x.IsCorrect && x.User.Id == userId);

            if (correctAnswersForTopic == 0 && incorrectAnswersForTopic > 0)
                return 0;
            if (correctAnswersForTopic > 0 && incorrectAnswersForTopic == 0)
                return 100;
            //if (correctAnswersForTopic > incorrectAnswersForTopic)
            //    return 100;
            if (correctAnswersForTopic > 0 && incorrectAnswersForTopic > 0 && correctAnswersForTopic == incorrectAnswersForTopic)
                return 50;
            if (correctAnswersForTopic > 0 && incorrectAnswersForTopic > 0 &&
                correctAnswersForTopic > incorrectAnswersForTopic)
                return 100 - (int)(((decimal)((decimal)incorrectAnswersForTopic / (decimal)correctAnswersForTopic * 100)));
            if (correctAnswersForTopic > 0 && incorrectAnswersForTopic > 0 &&
               correctAnswersForTopic < incorrectAnswersForTopic)
                return (int)(((decimal)((decimal)correctAnswersForTopic / (decimal)incorrectAnswersForTopic * 100)));

            return 0;
            //var answeredQuestionsForTopic = GetCorrectUserAnsweredQuestionsForTopic(userId, topicId);
            //return answeredQuestionsForTopic == 0
            //    ? 0
            //    : (int)(((decimal)((decimal)answeredQuestionsForTopic / (decimal)GetUserAnsweredQuestionsForTopic(userId, topicId))) * 100);
        }


        public dynamic GetDashboardData(string userId = null)
        {
            var questionsCount = _ctx.Questions.Count();
            //var readedQuestionsWithAnswers = userId == null ? 0 : _ctx.UserAnswers.Where(x => x.User.Id == userId).GroupBy(x => x.Question.Id).Count();
            //var topics = _ctx.Topics.ToList();

            //var avarageOverallSuccessRate = 0;
            //var cnt = _ctx.UserAnswers.Where(x => x.User.Id == userId).GroupBy(x => x.Question.Topic.Id).Count();
            //if (cnt == 0)
            //    avarageOverallSuccessRate = 0;
            //else
            //    avarageOverallSuccessRate = userId == null ? 0 : (int)(topics.Sum(x => GetSuccessRateAnswersForTopic(userId, x.Id)) / cnt);

            dynamic result = new ExpandoObject();

            result.TopicsCount = _ctx.Topics.Count();
            result.QuestionsCount = questionsCount;
            result.Progress = GetOverallUserProgress(userId);
            result.SuccessRate = GetSuccessOverAllRateAnswersForTopic(userId);
            return result;
        }

        public dynamic GetQuizQuestionsData(int topicId, string userId)
        {
            dynamic result = new ExpandoObject();

            //var s =
            //   result.UnreadQuestionsIds =
            //    _ctx.Questions.Include(x => x.Topic).Where(
            //        q =>
            //            q.Topic.Id == topicId &&
            //            !_ctx.UserReadedQuestions.Where(qu => qu.Question.Topic.Id == topicId && qu.User.Id == userId)
            //                .Select(x => x.Question.Id)
            //                .Contains(q.Id)).OrderBy(x => x.Order).Select(x => x.Id).ToList();

            result.UnreadQuestionsIds =
                _ctx.Questions.Include(x => x.Topic).Where(
                    q =>
                        q.Topic.Id == topicId &&
                        !_ctx.UserReadedQuestions.Where(qu => qu.Question.Topic.Id == topicId && qu.User.Id == userId)
                            .Select(x => x.Question.Id)
                            .Contains(q.Id)).OrderBy(x => x.Order).Select(x => x.Id).ToList();

            //_ctx.Questions.Where(
            //    q =>
            //        q.Topic.Id == topicId &&
            //        !_ctx.UserReadedQuestions.Where(qu => qu.Question.Topic.Id == topicId && qu.User.Id == userId)
            //            .Select(x => x.Question.Id)
            //            .Contains(q.Id)).OrderBy(x => x.Order).Select(x => x.Id).ToList();

            result.FlaggedQuestionsIds =
                _ctx.UserQuestionFlags.Where(qf => qf.Question.Topic.Id == topicId && qf.User.Id == userId).OrderBy(x => x.Question.Order).Select(x => x.Question.Id).ToList();

            return result;
        }

        public Question GetNextQuestionForTopic(int topicId, string userId, int prevQuestionOrderNumber = 1, int p = 0, bool isScrolled = false)
        {
            //  var questionsQuery = _ctx.Questions.Include(x => x.Answers).Include(x => x.Topic).Where(q => q.Topic.Id == topicId).OrderBy(q => q.Id);
            //return prevQuestionOrderNumber == 1 ? questionsQuery.FirstOrDefault(x=> x.Order ==  ) : questionsQuery.FirstOrDefault(q => q.Id > prevQuestionOrderNumber);
            if (p == 0)
            {
                return _ctx.Questions.Include(x => x.Answers).Include(x => x.Topic).Where(q => q.Topic.Id == topicId).FirstOrDefault(q => q.Order == prevQuestionOrderNumber + 1);
            }
            if (p == 1)
            {
                var readed = _ctx.UserReadedQuestions.Where(urq => urq.User.Id == userId).Select(x => x.Question.Id).ToList();
                var order = prevQuestionOrderNumber == 0 ? 0 : isScrolled ? _ctx.Questions.FirstOrDefault(x => x.Id == prevQuestionOrderNumber).Order - 1 : _ctx.Questions.FirstOrDefault(x => x.Id == prevQuestionOrderNumber).Order;
                //var asd =
                //    _ctx.Questions.Include(x => x.Answers)
                //        .Include(x => x.Topic)
                //        .Where(q => q.Topic.Id == topicId && flagged.Select(x => x.Question.Id).Contains(q.Id))
                //        .ToList();
                var result = _ctx.Questions.Include(x => x.Answers).Include(x => x.Topic).Where(q => q.Topic.Id == topicId && !readed.Contains(q.Id)).ToList();

                if (prevQuestionOrderNumber == 0)
                {
                    return result[0];
                }
                if (readed.Count > 0 && prevQuestionOrderNumber == readed[readed.Count - 1])
                {
                    return result[result.Count - 1];
                }

                //.FirstOrDefault(q => q.Order > order);
                return result.FirstOrDefault(q => q.Order > order);

                //var readed = _ctx.UserReadedQuestions.Where(urq => urq.User.Id == userId).Select(x => x.Question.Id);

                //return _ctx.Questions.Include(x => x.Answers).Include(x => x.Topic).Where(q => q.Topic.Id == topicId && !readed.Contains(q.Id)).FirstOrDefault(q => q.Order > prevQuestionOrderNumber);
            }
            if (p == 2)
            {
                var flagged = _ctx.UserQuestionFlags.Where(urq => urq.User.Id == userId).OrderBy(x => x.Question.Order).Select(x => x.Question.Id).ToList();
                var order = prevQuestionOrderNumber == 0 ? 0 : isScrolled ? _ctx.Questions.FirstOrDefault(x => x.Id == prevQuestionOrderNumber).Order - 1 : prevQuestionOrderNumber;
                //var asd =
                //    _ctx.Questions.Include(x => x.Answers)
                //        .Include(x => x.Topic)
                //        .Where(q => q.Topic.Id == topicId && flagged.Select(x => x.Question.Id).Contains(q.Id))
                //        .ToList();
                var result = _ctx.Questions.Include(x => x.Answers).Include(x => x.Topic).Where(q => q.Topic.Id == topicId && flagged.Contains(q.Id)).ToList();

                if (flagged.Count > 0 && prevQuestionOrderNumber == 0)
                {
                    return result[0];
                }
                if (!isScrolled && flagged.Count > 0 && prevQuestionOrderNumber == flagged[flagged.Count - 1])
                {
                    return result[result.Count - 1];
                }

                //.FirstOrDefault(q => q.Order > order);
                return result.FirstOrDefault(q => q.Order > order);
                //return _ctx.Questions.Include(x => x.Answers).Include(x => x.Topic).Where(q => q.Topic.Id == topicId && flagged.Select(x => x.Question.Id).Contains(q.Id)).FirstOrDefault(q => _ctx.Questions.FirstOrDefault(x => x.Id == q.Id + 1).Order > prevQuestionOrderNumber);
            }
            return _ctx.Questions.Include(x => x.Answers).Include(x => x.Topic).Where(q => q.Topic.Id == topicId).FirstOrDefault(q => q.Order == prevQuestionOrderNumber + 1);
        }

        public Question GetPrevQuestionForTopic(int topicId, string userId, int prevQuestionOrderNumber = 1, int p = 0, bool isScrolled = false)
        {
            if (p == 2)
            {
                var flagged = _ctx.UserQuestionFlags.Where(urq => urq.User.Id == userId).OrderByDescending(x => x.Question.Order).Select(x => x.Question.Id).ToList();
                var order = prevQuestionOrderNumber == 0 ? 0 : isScrolled ? _ctx.Questions.FirstOrDefault(x => x.Id == prevQuestionOrderNumber).Order + 1 : prevQuestionOrderNumber;
                //var asd =
                //    _ctx.Questions.Include(x => x.Answers)
                //        .Include(x => x.Topic)
                //        .Where(q => q.Topic.Id == topicId && flagged.Select(x => x.Question.Id).Contains(q.Id))
                //        .ToList();
                var result = _ctx.Questions.Include(x => x.Answers).Include(x => x.Topic).Where(q => q.Topic.Id == topicId && flagged.Contains(q.Id)).ToList();

                if (flagged.Count > 0 && prevQuestionOrderNumber == 0)
                {
                    return result[0];
                }
                if (!isScrolled && flagged.Count > 0 && prevQuestionOrderNumber == flagged[flagged.Count - 1])
                {
                    return result[result.Count - 1];
                }

                //.FirstOrDefault(q => q.Order > order);
                return result.Where(q => q.Order < order ).OrderByDescending(x => x.Order).First();
                //return _ctx.Questions.Include(x => x.Answers).Include(x => x.Topic).Where(q => q.Topic.Id == topicId && flagged.Select(x => x.Question.Id).Contains(q.Id)).FirstOrDefault(q => _ctx.Questions.FirstOrDefault(x => x.Id == q.Id + 1).Order > prevQuestionOrderNumber);
            }
            if (p == 1)
            {
                //var readed = _ctx.UserReadedQuestions.Where(urq => urq.User.Id == userId).Select(x => x.Question.Id);

                //return _ctx.Questions.Include(x => x.Answers).Include(x => x.Topic).Where(q => q.Topic.Id == topicId && !readed.Contains(q.Id)).FirstOrDefault(q => q.Order < prevQuestionOrderNumber);

                var readed = _ctx.UserReadedQuestions.Where(urq => urq.User.Id == userId).Select(x => x.Question.Id).ToList();
                var order = prevQuestionOrderNumber == 0 ? 0 : isScrolled ? _ctx.Questions.FirstOrDefault(x => x.Id == prevQuestionOrderNumber).Order + 1 : prevQuestionOrderNumber;
                //var asd =
                //    _ctx.Questions.Include(x => x.Answers)
                //        .Include(x => x.Topic)
                //        .Where(q => q.Topic.Id == topicId && flagged.Select(x => x.Question.Id).Contains(q.Id))
                //        .ToList();
                var result = _ctx.Questions.Include(x => x.Answers).Include(x => x.Topic).Where(q => q.Topic.Id == topicId && !readed.Contains(q.Id)).ToList();

                if (readed.Count > 0 && prevQuestionOrderNumber == 0)
                {
                    return result[0];
                }
                if (!isScrolled && readed.Count > 0 && prevQuestionOrderNumber == readed[readed.Count - 1])
                {
                    return result[result.Count - 1];
                }

                //.FirstOrDefault(q => q.Order > order);
                return result.Where(q => q.Order < order).OrderByDescending(x => x.Order).First();

            }
            //if (p == 2)
            //{
            //    var flagged = _ctx.UserQuestionFlags.Where(urq => urq.User.Id == userId).Select(x => x.Question.Id);

            //    return _ctx.Questions.Include(x => x.Answers).Include(x => x.Topic).Where(q => q.Topic.Id == topicId && flagged.Contains(q.Id)).FirstOrDefault(q => q.Order < prevQuestionOrderNumber);
            //}
            return _ctx.Questions.Include(x => x.Answers).Include(x => x.Topic).Where(q => q.Topic.Id == topicId).FirstOrDefault(q => q.Order == prevQuestionOrderNumber - 1);
        }

        public List<Answer> GetUserAnswers(string userId, int questionId)
        {
            return _ctx.UserAnswers.Where(ua => ua.User.Id == userId && ua.Question.Id == questionId).Select(x => x.Answer).ToList();
        }

        public bool? IsAnswerForQuestionCorrect(string userId, int questionId)
        {
            var answers = _ctx.UserAnswers.Where(ua => ua.User.Id == userId && ua.Question.Id == questionId).Select(x => x.Answer.Id).ToList();
            if (answers.Count == 0)
                return null;
            var correct = String.Join("", _ctx.Answers.Where(a => a.Question.Id == questionId && a.IsCorrect).OrderBy(a => a.Id).Select(x => x.Id).ToList());
            answers.Sort();
            return correct.Equals(string.Join("", answers));
        }

        public async Task<int> MarkQuestionAsRead(string userId, int questionId, bool isCorrect)
        {
            //if (_ctx.UserReadedQuestions.Any(x => x.User.Id == userId && x.Question.Id == questionId))
            //    return 0;
            _ctx.UserReadedQuestions.Add(new UserReadedQuestions()
            {
                Question = _ctx.Questions.First(q => q.Id == questionId),
                User = _ctx.Users.First(u => u.Id == userId),
                IsCorrect = isCorrect
            });
            return await _ctx.SaveChangesAsync();
        }

        public async Task<int> MarkQuestionAsUnRead(string userId, int questionId)
        {
            //if (_ctx.UserReadedQuestions.Any(x => x.User.Id == userId && x.Question.Id == questionId))
            //    return 0;
            var read = _ctx.UserReadedQuestions.FirstOrDefault(x => x.User.Id == userId && x.Question.Id == questionId);
            _ctx.UserReadedQuestions.Remove(read);

            return await _ctx.SaveChangesAsync();
        }

        //public async Task<int> MarkQuestionAsFlagged(string userId, int questionId)
        //{
        //    if (_ctx.UserQuestionFlags.Any(x => x.User.Id == userId && x.Question.Id == questionId))
        //        return 0;
        //    _ctx.UserQuestionFlags.Add(new UserQuestionFlags()
        //    {
        //        Question = _ctx.Questions.First(q => q.Id == questionId),
        //        User = _ctx.Users.First(u => u.Id == userId)
        //    });
        //    return await _ctx.SaveChangesAsync();
        //}

        public async Task<int> ChangeFlagState(string userId, int questionId)
        {
            var flag = _ctx.UserQuestionFlags.FirstOrDefault(x => x.User.Id == userId && x.Question.Id == questionId);
            if (flag != null)
            {
                _ctx.UserQuestionFlags.Remove(flag);

            }
            else
            {
                _ctx.UserQuestionFlags.Add(new UserQuestionFlags()
                {
                    Question = _ctx.Questions.First(q => q.Id == questionId),
                    User = _ctx.Users.First(u => u.Id == userId)
                });
            }
            return await _ctx.SaveChangesAsync();
        }

        public bool ShouldUserPay(ApplicationUser user, int freeUsagePeriodMinutes, int paidUsagePeriodDays)
        {
            var lastPaidDate = _ctx.Payments.OrderByDescending(x => x.Date).FirstOrDefault(x => x.User.Id == user.Id.ToString());
            if (lastPaidDate == null)
            {
                if ((DateTime.UtcNow - user.CreateDate).Minutes < freeUsagePeriodMinutes)
                {
                    return false;
                }

                return true;
            }

            if ((DateTime.UtcNow - lastPaidDate.Date).Days < paidUsagePeriodDays)
            {
                return false;
            }
            return true;

        }

        private bool ShouldUserPay(DateTime? lastPaidDate, DateTime userCreateDate, int freeUsagePeriodMinutes, int paidUsagePeriodDays)
        {
            if (lastPaidDate == null)
            {
                if ((DateTime.UtcNow - userCreateDate).TotalMinutes < freeUsagePeriodMinutes)
                {
                    return false;
                }

                return true;
            }

            if ((DateTime.UtcNow - lastPaidDate.Value).Days < paidUsagePeriodDays)
            {
                return false;
            }
            return true;
        }

        public dynamic GetPaymentInfo(ApplicationUser user, int freeUsagePeriodMinutes, int paidUsagePeriodDays)
        {
            dynamic result = new ExpandoObject();
            var lastPaidDate = _ctx.Payments.OrderByDescending(x => x.Date).FirstOrDefault(x => x.User.Id == user.Id.ToString());

            result.DoesHaveAccessNow = !ShouldUserPay(lastPaidDate != null ? (DateTime?)lastPaidDate.Date : null, user.CreateDate, user.FreeUsagePeriodMinutes, paidUsagePeriodDays);

            result.LastPaymentDate = lastPaidDate != null ? (DateTime?)lastPaidDate.Date : null;
            result.ExpireDate = lastPaidDate != null ? lastPaidDate.Date.AddDays(paidUsagePeriodDays) : user.CreateDate.AddMinutes(freeUsagePeriodMinutes);

            return result;
        }

        public async Task<int> Pay(string userId)
        {
            _ctx.Payments.Add(new Payment()
            {
                Date = DateTime.UtcNow,
                User = _ctx.Users.First(u => u.Id == userId)
            });
            return await _ctx.SaveChangesAsync();
        }

        public bool RevealAnswer(string userId, int questionId, List<int> answers)
        {
            var oldAnswers = _ctx.UserAnswers.Where(a => a.User.Id == userId && a.Question.Id == questionId).ToList();
            _ctx.UserAnswers.RemoveRange(oldAnswers);

            var q = _ctx.Questions.First(qq => qq.Id == questionId);
            var u = _ctx.Users.First(uu => uu.Id == userId);

            var userAnswers = answers.Select(x => new UserAnswers()
            {
                Answer = _ctx.Answers.First(a => a.Id == x),
                Date = DateTime.UtcNow,
                Question = q,
                User = u
            });

            var userAnswerses = userAnswers as IList<UserAnswers> ?? userAnswers.ToList();
            _ctx.UserAnswers.AddRange(userAnswerses);
            _ctx.SaveChanges();

            var correct = String.Join("", _ctx.Answers.Where(a => a.Question.Id == questionId && a.IsCorrect).OrderBy(a => a.Id).Select(x => x.Id).ToList());
            answers.Sort();
            return correct.Equals(string.Join("", answers));// userAnswerses.Any(ua => ua.Answer.IsCorrect == false);
        }

        public void ResetProgressForTopic(string userId, int topicId)
        {
            var answers = _ctx.UserAnswers.Where(ua => ua.User.Id == userId && ua.Question.Topic.Id == topicId);
            _ctx.UserAnswers.RemoveRange(answers);
            var reads = _ctx.UserReadedQuestions.Where(ua => ua.User.Id == userId && ua.Question.Topic.Id == topicId);
            _ctx.UserReadedQuestions.RemoveRange(reads);
            var flags = _ctx.UserQuestionFlags.Where(ua => ua.User.Id == userId && ua.Question.Topic.Id == topicId);
            _ctx.UserQuestionFlags.RemoveRange(flags);
            _ctx.SaveChanges();
        }

        public void ResetProgress(string userId)
        {
            var answers = _ctx.UserAnswers.Where(ua => ua.User.Id == userId);
            _ctx.UserAnswers.RemoveRange(answers);
            var reads = _ctx.UserReadedQuestions.Where(ua => ua.User.Id == userId);
            _ctx.UserReadedQuestions.RemoveRange(reads);
            var flags = _ctx.UserQuestionFlags.Where(ua => ua.User.Id == userId);
            _ctx.UserQuestionFlags.RemoveRange(flags);
            _ctx.SaveChanges();
        }

        public void Dispose()
        {
            _ctx.Dispose();
        }
    }
}
