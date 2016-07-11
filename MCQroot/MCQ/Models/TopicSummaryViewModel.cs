using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MCQ.Models
{
    public class TopicSummaryViewModel
    {
        public int QuestionsCount { get; set; }

        public int Progress { get; set; }

        public int SuccessRate { get; set; }
    }
}