using System.Collections.Generic;
using System.Linq;
using ESchool.Domain.DTOs.Examinations;
using ESchool.Domain.Entities.Examinations;

namespace ESchool.Domain.Extensions
{
    public static class EntitiesExtensions
    {
        public static GroupDto ToGroupDto(this Group entity)
        {
            return new GroupDto
            {
                Id = entity.Id,
                Name = entity.Name
            };
        }

        public static QTagDto ToQTagDto(this QTag entity)
        {
            return new QTagDto
            {
                ParentId = entity.ParentId,
                Id = entity.Id,
                Name = entity.Name,
                Description = entity.Description,
                SubQTags = new List<QTagDto>()
            };
        }

        public static QuestionDto ToQuestionDto(this Question entity)
        {
            return new QuestionDto
            {
                Id = entity.Id,
                Content = entity.Content,
                Description = entity.Description,
                Type = entity.Type,
                DifficultLevel = entity.DifficultLevel,
                Specialized = entity.Specialized,
                Month = entity.Month,
                QTagId = entity.QTagId,
                Answers = entity.Answers.Select(a => a.ToAnswerDto())
            };
        }

        public static AnswerDto ToAnswerDto(this Answer entity)
        {
            return new AnswerDto
            {
                AnswerName = entity.AnswerName,
                Body = entity.Body,
                DSS = entity.DSS
            };
        }

        public static ExamPaperDto ToExamPaperDto(this ExamPaper entity)
        {
            return new ExamPaperDto
            {
                Id = entity.Id,
                Name = entity.Name,
                Questions = entity.QuestionExamPapers.Select(qep => qep.Question.ToQuestionDto())
            };
        }
    }
}
