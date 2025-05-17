using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using StudentManagementSystem.Domain.Core.AuthenticationEntities;
using StudentManagementSystem.Domain.Core.Entities;

namespace StudentManagementSystem.Infrastructre.Data;


public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public DbSet<Student> Students => Set<Student>();
    public DbSet<Teacher> Teachers => Set<Teacher>();
    public DbSet<Subject> Subjects => Set<Subject>();
    public DbSet<Grade> Grades => Set<Grade>();
    public DbSet<Schedule> Schedules => Set<Schedule>();
    public DbSet<Class> Classes => Set<Class>();
    public DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Student-Class relationship
        modelBuilder.Entity<Student>()
            .HasOne(s => s.Class)
            .WithMany(c => c.Students)
            .HasForeignKey(s => s.ClassId)
            .IsRequired(true)
            .OnDelete(DeleteBehavior.Restrict);

        // Class-Teacher relationship
        modelBuilder.Entity<Class>()
            .HasOne(c => c.Teacher)
            .WithMany(t => t.Classes)
            .HasForeignKey(c => c.TeacherId)
            .IsRequired(true)
            .OnDelete(DeleteBehavior.Restrict);

        // Subject-Teacher relationship
        modelBuilder.Entity<Subject>()
            .HasOne(s => s.Teacher)
            .WithMany(t => t.Subjects)
            .HasForeignKey(s => s.TeacherId)
            .IsRequired(true)
            .OnDelete(DeleteBehavior.Restrict);

        // Grade-Student relationship
        modelBuilder.Entity<Grade>()
            .HasOne(g => g.Student)
            .WithMany(s => s.Grades)
            .HasForeignKey(g => g.StudentId)
            .IsRequired(true)
            .OnDelete(DeleteBehavior.Restrict);

        // Grade-Subject relationship
        modelBuilder.Entity<Grade>()
            .HasOne(g => g.Subject)
            .WithMany(s => s.Grades)
            .HasForeignKey(g => g.SubjectId)
            .IsRequired(true)
            .OnDelete(DeleteBehavior.Restrict);

        // Schedule-Class relationship
        modelBuilder.Entity<Schedule>()
            .HasOne(s => s.Class)
            .WithMany(c => c.Schedules)
            .HasForeignKey(s => s.ClassId)
            .IsRequired(true)
            .OnDelete(DeleteBehavior.Restrict);

        // Schedule-Subject relationship
        modelBuilder.Entity<Schedule>()
            .HasOne(s => s.Subject)
            .WithMany(sub => sub.Schedules)
            .HasForeignKey(s => s.SubjectId)
            .IsRequired(true)
            .OnDelete(DeleteBehavior.Restrict);

        // Schedule-Teacher relationship
        modelBuilder.Entity<Schedule>()
            .HasOne(s => s.Teacher)
            .WithMany(t => t.Schedules)
            .HasForeignKey(s => s.TeacherId)
            .IsRequired(true)
            .OnDelete(DeleteBehavior.Restrict);

        // Configure DateTime precision for Schedule times
        modelBuilder.Entity<Schedule>()
            .Property(s => s.StartTime)
            .HasPrecision(0); // Remove fractional seconds

        modelBuilder.Entity<Schedule>()
            .Property(s => s.EndTime)
            .HasPrecision(0); // Remove fractional seconds
    }
}
