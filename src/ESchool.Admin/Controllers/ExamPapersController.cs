using System.Collections.Generic;
using System.Threading.Tasks;
using ESchool.Domain.DTOs.Examinations;
using ESchool.Domain.Entities.Examinations;
using ESchool.Domain.Enums;
using ESchool.Domain.Extensions;
using ESchool.Domain.ViewModels.Examinations;
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
                if (viewModel.QTags != null)
                {
                    foreach (var qtag in viewModel.QTags)
                    {

                    }
                }

                var code = await _examPaperService.CreateAsync(viewModel.Name);

                return PostResult(code, entity.Id);
            }

            return BadRequest(ModelState);
        }

        //[HttpPut("{id}")]
        //public async Task<IActionResult> Put(int id, [FromBody]ExamPaperViewModel viewModel)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        var entity = viewModel.ToExamPaper(id);
        //        var code = await _examPaperService.UpdateAsync(entity, viewModel.QuestionIds);

        //        return PutResult(code);
        //    }

        //    return BadRequest(ModelState);
        //}

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            if (id > 0)
            {
                var code = await _examPaperService.DeleteAsync(id);

                return DeleteResult(code);
            }

            return BadRequestErrorDto(ErrorCode.InvalidEntityId, "Invalid ExamPaper Id.");
        }
    }
}
