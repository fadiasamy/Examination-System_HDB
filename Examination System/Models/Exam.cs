using System.ComponentModel.DataAnnotations;

namespace Examination_System.Models
{
    public class Exam
    {
        [Key]
       public int Exam_Id { get; set; }
       public DateTime CreationDate { get; set; }

    }
}
