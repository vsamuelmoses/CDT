using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MCQ.Models
{
    public class ExternalAccountViewModel : RegisterViewModel
    {
        public long Id { get; set; }

        public string AccessToken { get; set; }
    }
}