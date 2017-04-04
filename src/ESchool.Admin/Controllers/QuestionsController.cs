using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using ESchool.Domain;
using ESchool.Domain.Entities.Examinations;
using ESchool.Domain.ViewModels.Examinations;
using ESchool.Services.Examinations;
using ESchool.Services.Infrastructure.Extensions;
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

        [HttpPost]
        public async Task<IActionResult> Post([FromBody]QuestionCreateViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var entity = _mapper.Map<Question>(viewModel);
                var code = await _questionService.CreateAsync(entity, viewModel.QTagIds);

                if (code == ErrorCode.Success)
                {
                    return CreatedAtAction(nameof(Post), entity.Id);
                }

                return Ok(code);
            }

            return BadRequestErrorCode(ModelState);
        }

        [HttpPut]
        public async Task<IActionResult> Put([FromBody]QuestionUpdateViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var entity = await _questionService.FindAsync(viewModel.Id);

                if (entity == null)
                {
                    return NotFoundErrorCode();
                }

                entity.Content = viewModel.Content.Trim();
                entity.DSS = viewModel.DSS;
                entity.Description = viewModel.Description.TrimNull();
                var code = await _questionService.UpdateAsync(entity, viewModel.QTagIds);

                return ServerErrorCode(code);
            }

            return BadRequestErrorCode(ModelState);
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id.HasValue && id.Value > 0)
            {
                var entity = await _questionService.FindAsync(id.Value);

                if (entity == null)
                {
                    return NotFound();
                }

                var effectedRows = await _questionService.DeleteAsync(entity);

                return Accepted();
            }

            return BadRequest(ErrorCode.InvalidEntityId);
        }
    }
}
