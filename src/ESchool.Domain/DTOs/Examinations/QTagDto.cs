using System.Collections.Generic;

namespace ESchool.Domain.DTOs.Examinations
{
    public class QTagDto
    {
        public int ParentId { get; set; }

        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public IList<IdNameDto> ParentQTags { get; set; }

        public IList<IdNameDto> SubQTags { get; set; }
    }
}
