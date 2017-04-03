namespace ESchool.Domain.Entities.Examinations
{
    public class Answer : BaseEntity
    {
        public string Content { get; set; }

        public string Body { get; set; }

        public int? QuestionId { get; set; }

        public virtual Question Question { get; set; }
    }
}
