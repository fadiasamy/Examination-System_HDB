using Examination_System.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace Examination_System.Models
{
    public class Question
    {
        [Key]
        public int Question_Id { get; set; }

        [Required(ErrorMessage = "Please enter the question text.")]
        public string Question_Text { get; set; }

        [Required(ErrorMessage = "Please enter the question score.")]
        [Range(1, 100, ErrorMessage = "Score must be between 1 and 100.")]
        public int Question_Score { get; set; }
        public virtual ICollection<Answer> Answers { get; set; }


    }

}

