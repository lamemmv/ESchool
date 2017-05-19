using System;
using System.Collections.Generic;

namespace ESchool.Data.Entities.Examinations
{
    public class Student : BaseEntity
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public DateTime? Birthday { get; set; }

        public string PhoneNumber { get; set; }

        public virtual ICollection<StudentExam> StudentExams { get; set; }

        public virtual ICollection<StudentExamPaperResult> StudentExamPaperResults { get; set; }
    }
}
