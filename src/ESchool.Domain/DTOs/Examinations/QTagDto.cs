using System.Collections.Generic;

namespace ESchool.Domain.DTOs.Examinations
{
    public class QTagDto
    {
        public GroupDto Group { get; set; }

        public int ParentId { get; set; }

        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public IList<QTagDto> SubQTags { get; set; }
    }
}
