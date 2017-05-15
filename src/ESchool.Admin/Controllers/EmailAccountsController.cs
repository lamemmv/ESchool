using System.Collections.Generic;
using System.Threading.Tasks;
using ESchool.Domain.Entities.Messages;
using ESchool.Domain.Extensions;
using ESchool.Domain.ViewModels.Messages;
using ESchool.Services.Exceptions;
using ESchool.Services.Messages;
using Microsoft.AspNetCore.Mvc;

namespace ESchool.Admin.Controllers
{
    public class EmailAccountsController : AdminController
    {
        private readonly IEmailAccountService _emailAccountService;

        public EmailAccountsController(IEmailAccountService emailAccountService)
        {
            _emailAccountService = emailAccountService;
        }

        [HttpGet("{id}")]
        public async Task<EmailAccount> Get(int id)
        {
            return await _emailAccountService.GetAsync(id);
        }

        [HttpGet()]
        public async Task<IList<EmailAccount>> Get()
        {
            return await _emailAccountService.GetListAsync();
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody]EmailAccountViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var entity = viewModel.ToEmailAccount();
                await _emailAccountService.CreateAsync(entity);

                return Created("Post", entity.Id);
            }

            return BadRequest(ModelState);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody]EmailAccountViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var entity = viewModel.ToEmailAccount(id);
                await _emailAccountService.UpdateAsync(entity);

                return NoContent();
            }

            return BadRequest(ModelState);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            if (id > 0)
            {
                await _emailAccountService.DeleteAsync(id);

                return NoContent();
            }

            return BadRequestErrorDto(ErrorCode.InvalidEntityId, "Invalid Email Account Id.");
        }
    }
}
