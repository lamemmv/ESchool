using System.Collections.Generic;
using System.Threading.Tasks;
using ESchool.Domain.DTOs.Examinations;
using ESchool.Domain.Extensions;
using ESchool.Domain.ViewModels.Examinations;
using ESchool.Services.Examinations;
using ESchool.Services.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace ESchool.Admin.Controllers
{
    public class ExamPapersController : AdminController
    {
        private readonly IExamPaperService _examPaperService;
        private readonly IQuestionService _questionService;

        public ExamPapersController(IExamPaperService examPaperService, IQuestionService questionService)
        {
            _examPaperService = examPaperService;
            _questionService = questionService;
        }

        [HttpGet("{id}")]
        public async Task<ExamPaperDto> Get(int id)
        {
            return await _examPaperService.GetAsync(id);
        }

        [HttpGet]
        public async Task<IEnumerable<ExamPaperDto>> Get(int? page, int? size)
        {
            return await _examPaperService.GetListAsync(page ?? DefaultPage, size ?? DefaultSize);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody]ExamPaperViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var entity = viewModel.ToExamPaper();

                var questionIds = await GetQuestionIds(viewModel.QTags);
                await _examPaperService.CreateAsync(entity, questionIds);

                return Created("Post", entity.Id);
            }

            return BadRequest(ModelState);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody]ExamPaperViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var entity = viewModel.ToExamPaper(id);

                var questionIds = await GetQuestionIds(viewModel.QTags);
                await _examPaperService.UpdateAsync(entity, questionIds);

                return NoContent();
            }

            return BadRequest(ModelState);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            if (id > 0)
            {
                await _examPaperService.DeleteAsync(id);

                return NoContent();
            }

            return BadRequestErrorDto(ErrorCode.InvalidEntityId, "Invalid ExamPaper Id.");
        }

        private async Task<IList<int>> GetQuestionIds(ExamPaperQTagViewModel[] qtags)
        {
            List<int> questionIds = new List<int>();

            int qtagsLength = qtags != null ? qtags.Length : 0;

            if (qtagsLength > 0)
            {
                ExamPaperQTagViewModel qtag;
                var randomQuestionTasks = new Task<IList<int>>[qtagsLength];

                for (int i = 0; i < qtagsLength; i++)
                {
                    qtag = qtags[i];

                    randomQuestionTasks[i] = _questionService.GetRandomQuestionsAsync(qtag.Id, qtag.NumberOfQuestion, qtag.DifficultLevel);
                }

                var randomQuestionResults = await Task.WhenAll(randomQuestionTasks);

                foreach (var item in randomQuestionResults)
                {
                    questionIds.AddRange(item);
                }
            }

            return questionIds;
        }
    }
}
