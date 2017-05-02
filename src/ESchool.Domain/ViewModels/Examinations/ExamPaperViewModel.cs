using System.ComponentModel.DataAnnotations;

namespace ESchool.Domain.ViewModels.Examinations
{
    public class ExamPaperViewModel
    {
        public int GroupId { get; set; }

        [Required]
        public string Name { get; set; }
    }
}
