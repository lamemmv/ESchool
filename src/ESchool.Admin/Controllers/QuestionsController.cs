using System.Collections.Generic;
using System.Threading.Tasks;
using ESchool.Domain.DTOs.Examinations;
using ESchool.Domain.Enums;
using ESchool.Domain.Extensions;
using ESchool.Domain.ViewModels.Examinations;
using ESchool.Services.Examinations;
using Microsoft.AspNetCore.Mvc;

namespace ESchool.Admin.Controllers
{
    public class QuestionsController : AdminController
    {
        private readonly IQuestionService _questionService;

        public QuestionsController(IQuestionService questionService)
        {
            _questionService = questionService;
        }

        [HttpGet("{id}")]
        public async Task<QuestionDto> Get(int id)
        {
            return await _questionService.GetAsync(id);
        }

        [HttpGet]
        public async Task<IEnumerable<QuestionDto>> Get(int? page, int? size)
        {
            return await _questionService.GetListAsync(page ?? DefaultPage, size ?? DefaultSize);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody]QuestionViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var entity = viewModel.ToQuestion();
                var code = await _questionService.CreateAsync(entity, viewModel.QTags);

                return PostResult(code, entity.Id);
            }

            return BadRequest(ModelState);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody]QuestionViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var entity = viewModel.ToQuestion(id);
                var code = await _questionService.UpdateAsync(entity, viewModel.QTags);

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

            return BadRequestErrorDto(ErrorCode.InvalidEntityId, "Invalid Question Id.");
        }
    }
}
