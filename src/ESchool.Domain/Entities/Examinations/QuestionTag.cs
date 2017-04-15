namespace ESchool.Domain.Entities.Examinations
{
    public class QuestionTag : BaseEntity
    {
        public int QTagId { get; set; }

        public virtual QTag QTag { get; set; }

        public int QuestionId { get; set; }

        public virtual Question Question { get; set; }
    }
}
