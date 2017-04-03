using System.Collections.Generic;

namespace ESchool.Domain.Entities.Examinations
{
    public class QTag : BaseEntity
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public virtual ICollection<QuestionTag> QuestionTags { get; set; }
    }
}
