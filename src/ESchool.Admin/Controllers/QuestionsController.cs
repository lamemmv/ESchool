using System.Threading.Tasks;
using ESchool.Data.Paginations;
using ESchool.Domain.DTOs.Examinations;
using ESchool.Domain.Extensions;
using ESchool.Domain.ViewModels.Examinations;
using ESchool.Services.Examinations;
using ESchool.Services.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace ESchool.Admin.Controllers
{
    public class QuestionsController : AdminController
    {
        private readonly IQTagService _qtagService;
        private readonly IQuestionService _questionService;

        public QuestionsController(IQTagService qtagService, IQuestionService questionService)
        {
            _qtagService = qtagService;
            _questionService = questionService;
        }

        [HttpGet("{id}")]
        public async Task<QuestionDto> Get(int id)
        {
            var questionDto = await _questionService.GetAsync(id);

            if (questionDto != null)
            {
                questionDto.QTag = await _qtagService.GetAsync(questionDto.QTagId);
            }

            return questionDto;
        }

        [HttpGet]
        public async Task<IPagedList<QuestionDto>> Get(int? page, int? size)
        {
            return await _questionService.GetListAsync(page ?? DefaultPage, size ?? DefaultSize);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody]QuestionViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var entity = viewModel.ToQuestion();
                await _questionService.CreateAsync(entity);

                return Created("Post", entity.Id);
            }

            return BadRequest(ModelState);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody]QuestionViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var entity = viewModel.ToQuestion(id);
                await _questionService.UpdateAsync(entity);

                return NoContent();
            }

            return BadRequest(ModelState);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            if (id > 0)
            {
                await _questionService.DeleteAsync(id);

                return NoContent();
            }

            return BadRequestErrorDto(ErrorCode.InvalidEntityId, "Invalid Question Id.");
        }
    }
}
