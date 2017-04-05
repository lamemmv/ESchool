using System.ComponentModel.DataAnnotations;
using ESchool.Domain.Entities.Examinations;

namespace ESchool.Domain.ViewModels.Examinations
{
    public class QuestionCreateViewModel
    {
        [Required]
        public string Content { get; set; }

        [Required]
        public int DSS { get; set; }

        public string Description { get; set; }

        public int[] QTagIds { get; set; }

        public Answer[] Answers { get; set; }
    }
}
