using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Examination_System.Models
{
    public class Answer
    {
        [Key]
        public int Answer_Id { get; set; }

        public string Answer_text { get; set; }
        public bool Is_correct { get; set; }

        [Required,ForeignKey("Question")]
        public int Question_Id { get; set; }

        public Question Question { get; set; }
    }
}
