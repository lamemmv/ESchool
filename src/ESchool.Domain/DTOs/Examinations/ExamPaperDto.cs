using System.Collections.Generic;

namespace ESchool.Domain.DTOs.Examinations
{
    public class ExamPaperDto
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public IEnumerable<QuestionDto> Questions { get; set; }
    }
}
