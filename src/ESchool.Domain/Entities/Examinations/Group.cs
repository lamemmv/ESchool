using System.Collections.Generic;

namespace ESchool.Domain.Entities.Examinations
{
    public class Group : BaseEntity
    {
        public string Name { get; set; }

        public virtual ICollection<QTag> QTags { get; set; }
    }
}
