using System.Collections.Generic;

namespace ESchool.Data.DTOs.Examinations
{
    public class QTagDto
    {
        public int ParentId { get; set; }

        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public IList<QTagDto> ParentQTags { get; set; }

        public IList<QTagDto> SubQTags { get; set; }

        public int? SubQTagsCount { get; set; }
    }
}
