using System;
using System.ComponentModel.DataAnnotations;

namespace ESchool.Domain.ViewModels.Examinations
{
    public class ExamPaperViewModel
    {
        [Required]
        public string Name { get; set; }

        public int GroupId { get; set; }

        public int Duration { get; set; }

        public bool Specialized { get; set; }

        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }

        public int[] ExceptList { get; set; }

        public QuestionExamPaperViewModel[] Parts { get; set; }
    }

    public class QuestionExamPaperViewModel
    {
        public int QTagId { get; set; }

        public int TotalQuestion { get; set; }

        public float TotalGrade { get; set; }
    }
}
