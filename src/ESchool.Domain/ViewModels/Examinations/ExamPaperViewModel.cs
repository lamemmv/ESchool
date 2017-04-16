using System.ComponentModel.DataAnnotations;

namespace ESchool.Domain.ViewModels.Examinations
{
    public class ExamPaperViewModel
    {
        [Required]
        public string Name { get; set; }

        public int[] QuestionIds { get; set; }
    }
}
