using Microsoft.Extensions.Diagnostics.HealthChecks;
using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace Examination_System.Models
{
    public class ExamResult
    {
        [Key]
        public int ExamResult_Id { get; set; }
        public int Score { get; set; }
        public DateTime ExamDate { get; set; }
        public int UserId { get; set; }
        public int ExamId { get; set; }
        public virtual User User { get; set; }
        public Exam Exam { get; set; }


    }
}
