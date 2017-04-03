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
    public class QTagsController : AdminController
    {
        private readonly IMapper _mapper;
        private readonly IQTagService _qtagService;

        public QTagsController(IMapper mapper, IQTagService qtagService)
        {
            _mapper = mapper;
            _qtagService = qtagService;
        }

        [HttpGet]
        public async Task<IEnumerable<QTag>> Get()
        {
            return await _qtagService.GetListAsync();
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody]QTagCreateViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var entity = _mapper.Map<QTag>(viewModel);
                var code = await _qtagService.CreateAsync(entity);

                return ServerErrorCode(code);
            }

            return BadRequestErrorCode(ModelState);
        }

        [HttpPut]
        public async Task<IActionResult> Put([FromBody]QTagUpdateViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var entity = await _qtagService.FindAsync(viewModel.Id);

                if (entity == null)
                {
                    return NotFoundErrorCode();
                }

                entity.Name = viewModel.Name.Trim();
                entity.Description = viewModel.Description.TrimNull();
                var code = await _qtagService.UpdateAsync(entity);

                return ServerErrorCode(code);
            }

            return BadRequestErrorCode(ModelState);
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id.HasValue && id.Value > 0)
            {
                var code = await _qtagService.DeleteAsync(id.Value);

                return ServerErrorCode(code);
            }

            return BadRequestErrorCode(ErrorCode.InvalidIdEntity);
        }
    }
}
