using System.ComponentModel.DataAnnotations;

namespace ESchool.Domain.ViewModels.Examinations
{
    public class ExamPaperViewModel
    {
        [Required]
        public string Name { get; set; }

        public ExamPaperQTagViewModel[] QTags { get; set; }
    }

    public class ExamPaperQTagViewModel
    {
        public int Id { get; set; }

        public int NumberOfQuestion { get; set; }

        public int DifficultLevel { get; set; }
    }
}
