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
            entity.QuestionExamPapers = await GetQuestionExamPapers(viewModel);

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

        private async Task<IList<QuestionExamPaper>> GetQuestionExamPapers(ExamPaperViewModel viewModel)
        {
            List<QuestionExamPaper> questionExamPapers = new List<QuestionExamPaper>();

            foreach (var part in viewModel.Parts)
            {
                questionExamPapers.AddRange(await _questionService.GetRandomQuestionsAsync(
                    part.QTagId,
                    viewModel.Specialized,
                    viewModel.FromDate,
                    viewModel.ToDate,
                    viewModel.ExceptList,
                    part.TotalGrade,
                    part.TotalQuestion
                ));
            }

            int orderNumber = 0;

            foreach (var item in questionExamPapers)
            {
                item.Order = ++orderNumber;
            }

            return questionExamPapers;
        }
    }
}
