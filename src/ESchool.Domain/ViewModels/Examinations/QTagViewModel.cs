using System.ComponentModel.DataAnnotations;

namespace ESchool.Domain.ViewModels.Examinations
{
    public class QTagViewModel
    {
        public int GroupId { get; set; }

        public int ParentId { get; set; }

        [Required]
        public string Name { get; set; }

        public string Description { get; set; }
    }
}
