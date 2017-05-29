using System.Threading.Tasks;
using ESchool.Admin.ViewModels.Accounts;
using ESchool.Data.Entities.Accounts;
using ESchool.Services.Messages;
using ESchool.Services.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace ESchool.API.Controllers
{
    public class AccountsController : BaseController
    {
        private readonly ILogger<AccountsController> _logger;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IQueuedEmailService _queuedEmailService;

        public AccountsController(
            ILogger<AccountsController> logger,
            UserManager<ApplicationUser> userManager,
            IEmailAccountService emailAccountService,
            IQueuedEmailService queuedEmailService)
        {
            _logger = logger;
            _userManager = userManager;
            _queuedEmailService = queuedEmailService;
        }

        [HttpPost("forgotpassword")]
        public async Task<IActionResult> ForgotPassword([FromBody]ForgotPasswordViewModel viewModel)
        {
            string email = viewModel.Email.Trim();
            ApplicationUser user = await _userManager.FindByEmailAsync(email);

            if (user == null || !(await _userManager.IsEmailConfirmedAsync(user)))
            {
                // Don't reveal that the user does not exist or is not confirmed.
                return BadRequest(ApiErrorCode.Undefined);
            }

            // For more information on how to enable account confirmation and password reset please visit http://go.microsoft.com/fwlink/?LinkID=532713
            // Send an email with this link.
            string code = await _userManager.GeneratePasswordResetTokenAsync(user);
            string callbackUrl = $"{viewModel.Url.Trim()}?userId={user.Id}&code={code}";

            await _queuedEmailService.CreateAsync(
                email,
                "Reset Password",
                "Please reset your password by clicking here: <a href=\"" + callbackUrl + "\">link</a>");

            return NoContent();
        }

        [HttpPut("resetpassword")]
        public async Task<IActionResult> ResetPassword([FromBody]ResetPasswordViewModel viewModel)
        {
            ApplicationUser user = await _userManager.FindByEmailAsync(viewModel.Email.Trim());

            if (user == null)
            {
                // Don't reveal that the user does not exist.
                return BadRequest(ApiErrorCode.Undefined);
            }

            IdentityResult result = await _userManager.ResetPasswordAsync(user, viewModel.Code, viewModel.Password);

            if (result.Succeeded)
            {
                return NoContent();
            }

            return BadRequest(result.Errors);
        }
    }
}
