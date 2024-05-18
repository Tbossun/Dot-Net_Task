using DotNet_Task.Models;
using Microsoft.EntityFrameworkCore;

namespace DotNet_Task.Data
{

    public class ApplicationDbContext : DbContext
    {
        public DbSet<Question> Questions { get; set; }
        public DbSet<Candidate> Candidates { get; set; }
        public DbSet<Answer> Answers { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Ensure each entity has a primary key
            modelBuilder.Entity<Question>().HasKey(q => q.Id);
            modelBuilder.Entity<Candidate>().HasKey(c => c.Id);
            modelBuilder.Entity<Answer>().HasKey(a => a.Id);

            // Configure relationships if necessary
            modelBuilder.Entity<Candidate>()
                .HasMany(c => c.Answers)
                .WithOne()
                .HasForeignKey(a => a.QuestionId);
        }
    }

}
