using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MCQ.Domain
{

    [Table("UserQuestionFlags")]
    public class UserQuestionFlags
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public virtual ApplicationUser User { get; set; }

        public virtual Question Question { get; set; }
    }
}
