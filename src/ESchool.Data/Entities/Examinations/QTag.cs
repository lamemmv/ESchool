using System.Collections.Generic;

namespace ESchool.Data.Entities.Examinations
{
    public class QTag : BaseEntity
    {
        public int ParentId { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public int GroupId { get; set; }

        public virtual Group Group { get; set; }

        public virtual ICollection<Question> Questions { get; set; }
    }
}
