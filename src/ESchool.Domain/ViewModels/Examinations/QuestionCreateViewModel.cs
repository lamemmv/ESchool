using System.ComponentModel.DataAnnotations;

namespace ESchool.Domain.ViewModels.Examinations
{
    public class QuestionCreateViewModel
    {
        [Required]
        public string Content { get; set; }

        public string Description { get; set; }

        public int Type { get; set; }

        public int[] QTagIds { get; set; }

        public AnswerCreateViewModel[] Answers { get; set; }
    }
}
