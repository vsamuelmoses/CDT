using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MCQ.Models
{
    public class LoggedUserViewModel : RegisterViewModel
    {
        public Guid? Id { get; set; }
    }
}