﻿using System.Linq;
using System.Threading.Tasks;
using ESchool.Data;
using ESchool.Data.Paginations;
using ESchool.Domain.DTOs.Examinations;
using ESchool.Domain.Entities.Examinations;
using ESchool.Domain.Enums;
using ESchool.Domain.Extensions;
using Microsoft.EntityFrameworkCore;

namespace ESchool.Services.Examinations
{
    public class QuestionService : BaseService<Question>, IQuestionService
    {
        public QuestionService(ObjectDbContext dbContext)
            : base(dbContext)
        {
        }

        public override async Task<Question> FindAsync(int id)
        {
            return await DbSetNoTracking
                .Include(q => q.QuestionTags)
                    .ThenInclude(qt => qt.QTag)
                .Include(q => q.Answers)
                .SingleOrDefaultAsync(q => q.Id == id);
        }

        public async Task<IPagedList<QuestionDto>> GetListAsync(int page, int size)
        {
            var questions = DbSetNoTracking
                .Include(q => q.QuestionTags)
                    .ThenInclude(qt => qt.QTag)
                .Include(q => q.Answers)
                .OrderBy(q => q.Id)
                .Select(q => q.ToQuestionDto());

            return await questions.GetListAsync(page, size);
        }

        public async Task<ErrorCode> CreateAsync(Question entity, string[] qtags)
        {
            if (qtags != null && qtags.Length > 0)
            {
                qtags = await AddQTags(qtags);

                entity.QuestionTags = qtags.Select(t => new QuestionTag { QTag = new QTag { Name = t } }).ToList();
            }

            return await base.CreateAsync(entity);
        }

        public override async Task<ErrorCode> UpdateAsync(int id, Question entity)
        {
            var updatedEntity = await base.FindAsync(id);

            if (updatedEntity == null)
            {
                return ErrorCode.NotFound;
            }

            updatedEntity.Content = entity.Content;
            updatedEntity.Description = entity.Description;
            updatedEntity.Type = entity.Type;

            // Delete current QuestionTags & Answers.
            DeleteQuestionTags(id);
            DeleteAnswers(id);

            updatedEntity.QuestionTags = entity.QuestionTags;
            updatedEntity.Answers = entity.Answers;

            return await CommitAsync();
        }

        public override async Task<ErrorCode> DeleteAsync(int id)
        {
            var entity = await DbSet
                .Include(q => q.QuestionTags)
                .Include(q => q.Answers)
                .SingleOrDefaultAsync(q => q.Id == id);

            if (entity == null)
            {
                return ErrorCode.NotFound;
            }

            return await base.DeleteAsync(entity);
        }

        private void DeleteQuestionTags(int questionId)
        {
            var questionTagsDbSet = _dbContext.QuestionTags;
            var questionTags = questionTagsDbSet.Where(qt => qt.QuestionId == questionId);

            if (questionTags.Any())
            {
                questionTagsDbSet.RemoveRange(questionTags);
            }
        }

        private void DeleteAnswers(int questionId)
        {
            var answersDbSet = _dbContext.Answers;
            var answers = answersDbSet.Where(a => a.QuestionId == questionId);

            if (answers.Any())
            {
                answersDbSet.RemoveRange(answers);
            }
        }

        private async Task<string[]> AddQTags(string[] qtags)
        {
            qtags = qtags.Distinct().Select(t => t.Trim()).ToArray();

            var qtagsDbSet = _dbContext.QTags;

            var existingQTags = qtagsDbSet.Where(t => qtags.Contains(t.Name));

            var newQTags = qtags.Except(existingQTags.Select(t => t.Name));

            if (newQTags.Any())
            {
                await qtagsDbSet.AddRangeAsync(newQTags.Select(t => new QTag { Name = t }));

                await _dbContext.SaveChangesAsync();
            }

            return await Task.FromResult(qtags);
        }
    }
}
