using System.Threading.Tasks;
using ESchool.Domain.Entities.Accounts;
using ESchool.Domain.Extensions;
using ESchool.Domain.ViewModels.Accounts;
using ESchool.Services.Messages;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace ESchool.Admin.Controllers
{
    public class AccountsController : AdminController
    {
        private readonly ILogger<AccountsController> _logger;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IEmailAccountService _emailAccountService;
        private readonly IQueuedEmailService _queuedEmailService;

        public AccountsController(
            ILogger<AccountsController> logger,
            UserManager<ApplicationUser> userManager, 
            IEmailAccountService emailAccountService,
            IQueuedEmailService queuedEmailService)
        {
            _logger = logger;
            _userManager = userManager;
            _emailAccountService = emailAccountService;
            _queuedEmailService = queuedEmailService;
        }

        [HttpGet("{id}")]
        public async Task<ApplicationUser> Get(string id)
        {
            return await _userManager.FindByIdAsync(id);
        }

        //[HttpGet]
        //public async Task<IPagedList<ApplicationUser>> Get()
        //{
        //    return await _groupService.GetListAsync();
        //}

        [HttpPost]
        public async Task<IActionResult> Post([FromBody]AccountViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var user = viewModel.ToApplicationUser();
                var result = await _userManager.CreateAsync(user, viewModel.Password);

                if (result.Succeeded)
                {
                    if (viewModel.Roles != null && viewModel.Roles.Length > 0)
                    {
                        result = await _userManager.AddToRolesAsync(user, viewModel.Roles);

                        if (!result.Succeeded)
                        {
                            return BadRequest(result);
                        }
                    }

                    return Created("Post", user.Id);
                }
                else
                {
                    return BadRequest(result);
                }
            }

            return BadRequest(ModelState);
        }
    }
}
