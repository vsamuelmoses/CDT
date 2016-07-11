using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MCQ.Domain;
using Microsoft.AspNet.Identity.EntityFramework;

namespace MCQ.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection")
        {
            //Database.SetInitializer<DataContext>(null);
        }

        public DbSet<Answer> Answers { get; set; }

        public DbSet<Question> Questions { get; set; }

        public DbSet<Topic> Topics { get; set; }

        public DbSet<UserAnswers> UserAnswers { get; set; }

        public DbSet<UserQuestionFlags> UserQuestionFlags { get; set; }

        public DbSet<UserReadedQuestions> UserReadedQuestions { get; set; }

        public DbSet<Payment> Payments { get; set; }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
    }
}