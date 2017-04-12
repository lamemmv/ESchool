﻿using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using ESchool.Domain.Entities.Examinations;
using ESchool.Domain.Enums;
using ESchool.Domain.ViewModels.Examinations;
using ESchool.Services.Examinations;
using Microsoft.AspNetCore.Mvc;

namespace ESchool.Admin.Controllers
{
    public class QuestionsController : AdminController
    {
        private readonly IMapper _mapper;
        private readonly IQuestionService _questionService;

        public QuestionsController(IMapper mapper, IQuestionService questionService)
        {
            _mapper = mapper;
            _questionService = questionService;
        }

        [HttpGet]
        public async Task<IEnumerable<Question>> Get(int? page, int? size)
        {
            return await _questionService.GetListAsync(page ?? DefaultPage, size ?? DefaultSize);
        }

        [HttpGet("{id}")]
        public async Task<Question> Get(int id)
        {
            return await _questionService.FindAsync(id);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody]QuestionCreateViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var entity = _mapper.Map<Question>(viewModel);
                var code = await _questionService.CreateAsync(entity, viewModel.QTagIds);

                return PostResult(code, entity.Id);
            }

            return BadRequest(ModelState);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody]QuestionCreateViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var entity = _mapper.Map<Question>(viewModel);
                var code = await _questionService.UpdateAsync(id, entity, viewModel.QTagIds);

                return PutResult(code);
            }

            return BadRequest(ModelState);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            if (id > 0)
            {
                var code = await _questionService.DeleteAsync(id);

                return DeleteResult(code);
            }

            return BadRequest(ErrorCode.InvalidEntityId);
        }
    }
}
