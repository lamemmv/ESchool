﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ESchool.Data;
using ESchool.Data.Paginations;
using ESchool.Domain.DTOs.Examinations;
using ESchool.Domain.Entities.Examinations;
using ESchool.Domain.Extensions;
using ESchool.Services.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace ESchool.Services.Examinations
{
    public class QuestionService : BaseService, IQuestionService
    {
        public QuestionService(ObjectDbContext dbContext)
            : base(dbContext)
        {
        }

        public async Task<IList<int>> GetRandomQuestionsAsync(int qtagId, int numberOfRandomQuestion, int difficultLevel)
        {
            var dbQuery = Questions.AsNoTracking()
                .Include(q => q.QuestionTags)
                .Where(q => q.DifficultLevel == difficultLevel)
                .Select(question => new
                {
                    question,
                    QuestionTags = question.QuestionTags.Where(qt => qt.QTagId == qtagId)
                });

            var questions = await dbQuery.Select(q => q.question).ToListAsync();
            IList<int> randomQuestions = new List<int>();

            if (questions.Count >= numberOfRandomQuestion)
            {
                int randomIndex;
                Random random = new Random();

                while (randomQuestions.Count <= numberOfRandomQuestion)
                {
                    randomIndex = random.Next(questions.Count);
                    randomQuestions.Add(questions[randomIndex].Id);

                    questions.RemoveAt(randomIndex);
                }
            }

            return randomQuestions;
        }

        public async Task<QuestionDto> GetAsync(int id)
        {
            var entity = await Questions.AsNoTracking()
                .Include(q => q.QuestionTags)
                    .ThenInclude(qt => qt.QTag)
                .Include(q => q.Answers)
                .SingleOrDefaultAsync(q => q.Id == id);

            if (entity == null)
            {
                return null;
            }

            return entity.ToQuestionDto();
        }

        public async Task<IPagedList<QuestionDto>> GetListAsync(int page, int size)
        {
            var questions = Questions.AsNoTracking()
                .Include(q => q.QuestionTags)
                    .ThenInclude(qt => qt.QTag)
                .Include(q => q.Answers)
                .OrderBy(q => q.Id)
                .Select(q => q.ToQuestionDto());

            return await questions.GetListAsync(page, size);
        }

        public async Task<Question> CreateAsync(Question entity, string[] qtags)
        {
            if (qtags != null && qtags.Length > 0)
            {
                qtags = await AddQTags(qtags);

                entity.QuestionTags = qtags.Select(t => new QuestionTag { QTag = new QTag { Name = t } }).ToList();
            }

            await Questions.AddAsync(entity);
            await CommitAsync();

            return entity;
        }

        public async Task<int> UpdateAsync(Question entity, string[] qtags)
        {
            var updatedEntity = await Questions.FindAsync(entity.Id);

            if (updatedEntity == null)
            {
                throw new EntityNotFoundException("Question not found.");
            }

            // Delete current QuestionTags & Answers.
            DeleteQuestionTags(entity.Id);
            DeleteAnswers(entity.Id);

            // Update.
            updatedEntity.Content = entity.Content;
            updatedEntity.Description = entity.Description;
            updatedEntity.Type = entity.Type;
            updatedEntity.DifficultLevel = entity.DifficultLevel;

            if (qtags != null && qtags.Length > 0)
            {
                qtags = await AddQTags(qtags);

                updatedEntity.QuestionTags = qtags.Select(t => new QuestionTag { QTag = new QTag { Name = t } }).ToList();
            }

            updatedEntity.Answers = entity.Answers;

            return await CommitAsync();
        }

        public async Task<int> DeleteAsync(int id)
        {
            var dbSet = Questions;
            var entity = await dbSet
                .Include(q => q.QuestionTags)
                .Include(q => q.Answers)
                .SingleOrDefaultAsync(q => q.Id == id);

            if (entity == null)
            {
                throw new EntityNotFoundException("Question not found.");
            }

            dbSet.Remove(entity);

            return await CommitAsync();
        }

        private DbSet<Question> Questions
        {
            get
            {
                return _dbContext.Set<Question>();
            }
        }

        private void DeleteQuestionTags(int questionId)
        {
            var dbSet = _dbContext.Set<QuestionTag>();
            var questionTags = dbSet.Where(qt => qt.QuestionId == questionId);

            if (questionTags.Any())
            {
                dbSet.RemoveRange(questionTags);
            }
        }

        private void DeleteAnswers(int questionId)
        {
            var dbSet = _dbContext.Set<Answer>();
            var answers = dbSet.Where(a => a.QuestionId == questionId);

            if (answers.Any())
            {
                dbSet.RemoveRange(answers);
            }
        }

        private async Task<string[]> AddQTags(string[] qtags)
        {
            qtags = qtags.Distinct().Select(t => t.Trim()).ToArray();

            var dbSet = _dbContext.Set<QTag>();

            var existingQTags = await dbSet.Where(t => qtags.Contains(t.Name)).ToListAsync();

            var newQTags = qtags.Except(existingQTags.Select(t => t.Name));

            if (newQTags.Any())
            {
                await dbSet.AddRangeAsync(newQTags.Select(t => new QTag { Name = t }));

                await _dbContext.SaveChangesAsync();
            }

            return qtags;
        }
    }
}
