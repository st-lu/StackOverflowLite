using Microsoft.EntityFrameworkCore;
using Stackoverflow_Lite.Entities;
using Stackoverflow_Lite.models;

namespace Stackoverflow_Lite.Configurations;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }
    
    public DbSet<User> Users { get; set; }
    public DbSet<OidcUserMapping> OidcUserMappings { get; set; }
    
    public DbSet<Answer> Answers { get; set; }
    
    public DbSet<Question> Questions { get; set; }
    public DbSet<UserQuestionView> UserQuestionViews { get; set; }



    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<OidcUserMapping>()
            .HasOne(oum => oum.User)
            .WithOne(u => u.OidcUserMapping)
            .HasForeignKey<OidcUserMapping>(oum => oum.UserId);

        modelBuilder.Entity<User>()
            .HasMany(u => u.Questions)
            .WithOne(q => q.User)
            .HasForeignKey(q => q.UserId);
        
        modelBuilder.Entity<Question>()
            .HasMany(q => q.Answers)
            .WithOne(a => a.Question)
            .HasForeignKey(a => a.QuestionId);
        
        modelBuilder.Entity<UserQuestionView>()
            .HasKey(uqv => new { uqv.UserId, uqv.QuestionId });

        modelBuilder.Entity<UserQuestionView>()
            .HasOne(uqv => uqv.User)
            .WithMany(u => u.UserQuestionViews)
            .HasForeignKey(uqv => uqv.UserId);

        modelBuilder.Entity<UserQuestionView>()
            .HasOne(uqv => uqv.Question)
            .WithMany(q => q.UserQuestionViews)
            .HasForeignKey(uqv => uqv.QuestionId);
    }
}