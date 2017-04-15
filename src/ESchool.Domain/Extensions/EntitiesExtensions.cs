using System.Linq;
using ESchool.Domain.DTOs;
using ESchool.Domain.DTOs.Examinations;
using ESchool.Domain.Entities.Examinations;

namespace ESchool.Domain.Extensions
{
    public static class EntitiesExtensions
    {
        public static QuestionDto ToQuestionDto(this Question entity)
        {
            if (entity == null)
            {
                return new QuestionDto();
            }

            return new QuestionDto
            {
                Id = entity.Id,
                Content = entity.Content,
                Description = entity.Description,
                Type = entity.Type,
                QTags = entity.QuestionTags.Select(qt => new IdNameDto { Id = qt.QTag.Id, Name = qt.QTag.Name }),
                Answers = entity.Answers.Select(a => a.ToAnswerDTo())
            };
        }

        public static AnswerDto ToAnswerDTo(this Answer entity)
        {
            return new AnswerDto
            {
                AnswerName = entity.AnswerName,
                Body = entity.Body,
                DSS = entity.DSS
            };
        }
    }
}
