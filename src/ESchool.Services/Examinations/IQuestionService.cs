﻿using System.Threading.Tasks;
using ESchool.Data.Paginations;
using ESchool.Domain.DTOs.Examinations;
using ESchool.Domain.Entities.Examinations;
using ESchool.Domain.Enums;

namespace ESchool.Services.Examinations
{
    public interface IQuestionService : IService<Question>
    {
        Task<IPagedList<QuestionDto>> GetListAsync(int page, int size);

        Task<ErrorCode> CreateAsync(Question entity, string[] qtags);
    }
}
