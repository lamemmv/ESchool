using System.Collections.Generic;
using System.Threading.Tasks;
using ESchool.Admin.ViewModels;
using ESchool.Admin.ViewModels.Examinations;
using ESchool.Data.Entities.Examinations;
using ESchool.Data.Paginations;
using ESchool.Services.Examinations;
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
        public async Task<ExamPaper> Get(int id)
        {
            return await _examPaperService.GetAsync(id);
        }

        [HttpGet]
        public async Task<IPagedList<ExamPaper>> Get(int? page, int? size)
        {
            return await _examPaperService.GetListAsync(page ?? DefaultPage, size ?? DefaultSize);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody]ExamPaperViewModel viewModel)
        {
            var entity = viewModel.ToExamPaper();
            entity.QuestionExamPapers = GetQuestionExamPapers(viewModel.Parts);

            await _examPaperService.CreateAsync(entity);

            return Created("Post", entity.Id);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            if (id > 0)
            {
                await _examPaperService.DeleteAsync(id);

                return NoContent();
            }

            return BadRequestApiError("ExamPaperId", "'ExamPaper Id' should not be empty.");
        }

        private IList<QuestionExamPaper> GetQuestionExamPapers(QuestionExamPaperViewModel[] parts)
        {
            throw new System.NotImplementedException();
            //List<int> questionIds = new List<int>();

            //int qtagsLength = qtags != null ? qtags.Length : 0;

            //if (qtagsLength > 0)
            //{
            //    ExamPaperQTagViewModel qtag;
            //    var randomQuestionTasks = new Task<IList<int>>[qtagsLength];

            //    for (int i = 0; i < qtagsLength; i++)
            //    {
            //        qtag = qtags[i];

            //        randomQuestionTasks[i] = _questionService.GetRandomQuestionsAsync(qtag.Id, qtag.NumberOfQuestion, qtag.DifficultLevel);
            //    }

            //    var randomQuestionResults = await Task.WhenAll(randomQuestionTasks);

            //    foreach (var item in randomQuestionResults)
            //    {
            //        questionIds.AddRange(item);
            //    }
            //}

            //return questionIds;
        }
    }
}
