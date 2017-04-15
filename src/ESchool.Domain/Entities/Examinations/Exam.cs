using System;
using System.Collections.Generic;

namespace ESchool.Domain.Entities.Examinations
{
    public class Exam : BaseEntity
    {
        public string Name { get; set; }

        public DateTime ExamDate { get; set; }

        public virtual ICollection<StudentExam> StudentExams { get; set; }
    }
}
