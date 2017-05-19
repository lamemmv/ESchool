namespace ESchool.Data.Entities.Examinations
{
    public class StudentExam : BaseEntity
    {
        public double Grade { get; set; }

        public string Comment { get; set; }

        public int StudentId { get; set; }

        public virtual Student Student { get; set; }

        public int ExamId { get; set; }

        public virtual Exam Exam { get; set; }
    }
}
