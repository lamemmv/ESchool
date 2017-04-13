using System.ComponentModel.DataAnnotations;

namespace ESchool.Domain.ViewModels.Examinations
{
    public class QTagViewModel
    {
        [Required]
        public string Name { get; set; }

        public string Description { get; set; }
    }
}
