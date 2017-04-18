using System.ComponentModel.DataAnnotations;

namespace ESchool.Domain.ViewModels.Examinations
{
    public class ExamPaperCreateViewModel
    {
        [Required]
        public string Name { get; set; }

        public ExamPaperCreateQTagViewModel[] QTags { get; set; }

        public int Level { get; set; }

        public int Total { get; set; }
    }

    public class ExamPaperCreateQTagViewModel
    {
        public int Id { get; set; }

        public int Percent { get; set; }
    }
}
