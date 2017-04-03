using System.ComponentModel.DataAnnotations;

namespace ESchool.Domain.ViewModels.Examinations
{
    public class QTagUpdateViewModel : QTagCreateViewModel
    {
        [Range(1, int.MaxValue)]
        public int Id { get; set; }
    }
}
