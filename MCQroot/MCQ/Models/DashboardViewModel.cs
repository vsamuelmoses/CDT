using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MCQ.Models
{
    public class DashboardViewModel
    {
        public int TopicsCount { get; set; }

        public int QuestionsCount { get; set; }

        public int Progress { get; set; }

        public int SuccessRate { get; set; }
      
    }
}