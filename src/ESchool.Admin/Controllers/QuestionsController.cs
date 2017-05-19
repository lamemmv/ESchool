using System.Threading.Tasks;
using ESchool.Admin.ViewModels;
using ESchool.Admin.ViewModels.Examinations;
using ESchool.Data.DTOs.Examinations;
using ESchool.Data.Entities.Examinations;
using ESchool.Data.Paginations;
using ESchool.Services.Examinations;
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
        public async Task<IPagedList<Question>> Get(int? page, int? size)
        {
            return await _questionService.GetListAsync(page ?? DefaultPage, size ?? DefaultSize);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody]QuestionViewModel viewModel)
        {
            var entity = viewModel.ToQuestion();
            await _questionService.CreateAsync(entity);

            return Created("Post", entity.Id);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody]QuestionViewModel viewModel)
        {
            var entity = viewModel.ToQuestion(id);
            await _questionService.UpdateAsync(entity);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            if (id > 0)
            {
                await _questionService.DeleteAsync(id);

                return NoContent();
            }

            return BadRequestApiError("QuestionId", "'Question Id' should not be empty.");
        }
    }
}
