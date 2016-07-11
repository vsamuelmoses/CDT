using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MCQ.Domain;

namespace MCQ.Models
{
    public class TopicViewModel
    {
        public Topic Topic { get; set; }

        public int Progress { get; set; }
    }
}