using System.ComponentModel.DataAnnotations;

namespace ESchool.Domain.ViewModels.Examinations
{
    public class QTagCreateViewModel
    {
        [Required]
        public string Name { get; set; }

        public string Description { get; set; }
    }
}
