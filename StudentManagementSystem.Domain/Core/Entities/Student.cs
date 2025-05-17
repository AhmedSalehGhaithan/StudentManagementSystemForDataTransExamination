using StudentManagementSystem.Domain.Core.Entities;

namespace StudentManagementSystem.Domain.Core.Entities;

public class Student
{
    public Guid StudentId { get; set; }
    public string FirstName { get; set; }

    public string LastName { get; set; }

    public string PhoneNumber { get; set; }

    public Guid ClassId { get; set; }


    public virtual Class Class { get; set; }
    public virtual ICollection<Grade> Grades { get; set; }
}
