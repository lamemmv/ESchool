using System.ComponentModel.DataAnnotations;

namespace ESchool.Domain.ViewModels.Examinations
{
    public class ExamPaperViewModel
    {
        [Required]
        public string Name { get; set; }

        public ExamPaperQTagViewModel[] QTags { get; set; }

        public int Level { get; set; }

        public int Total { get; set; }
    }

    public class ExamPaperQTagViewModel
    {
        public int Id { get; set; }

        public int Percent { get; set; }
    }
}
