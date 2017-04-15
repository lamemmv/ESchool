using System.ComponentModel.DataAnnotations;

namespace ESchool.Domain.ViewModels.Examinations
{
    public class AnswerViewModel
    {
        [Required]
        public string AnswerName { get; set; }

        [Required]
        public string Body { get; set; }

        public bool DSS { get; set; }
    }
}
