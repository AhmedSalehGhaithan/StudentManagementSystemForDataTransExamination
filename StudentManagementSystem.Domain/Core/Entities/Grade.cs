namespace StudentManagementSystem.Domain.Core.Entities;

public class Grade
{
    public Guid GradeId { get; set; }

    public decimal Score { get; set; }

  
    public Guid StudentId { get; set; }
    public Guid SubjectId { get; set; }

    public virtual Student Student { get; set; }
    public virtual Subject Subject { get; set; }
}
