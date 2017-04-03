using System.ComponentModel.DataAnnotations;

namespace ESchool.Domain.ViewModels.Examinations
{
    public class QuestionUpdateViewModel : QuestionCreateViewModel
    {
        [Range(1, int.MaxValue)]
        public int Id { get; set; }
    }
}
