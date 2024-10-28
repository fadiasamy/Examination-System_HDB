using Microsoft.EntityFrameworkCore;

namespace Examination_System.Models
{
    public class MyApplicationContext : DbContext
    {
        public MyApplicationContext(DbContextOptions<MyApplicationContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<Answer> Answers { get; set; }
        public DbSet<Exam> Exams { get; set; }
        public DbSet<ExamResult> ExamResults { get; set; }

       
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasData(
                new User { User_Id = 1, UserName = "Admin", Email = "admin@example.com", Password = "admin123", Role = "Admin" },
                new User { User_Id = 2, UserName = "User", Email = "user@example.com", Password = "user123", Role = "User" }
            );

            modelBuilder.Entity<Answer>()
                        .HasOne(a => a.Question)
                        .WithMany(q => q.Answers)
                        .HasForeignKey(a => a.Question_Id)
                        .OnDelete(DeleteBehavior.Cascade);



            base.OnModelCreating(modelBuilder);
        }

    }
}
