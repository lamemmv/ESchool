using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AspNet.Security.OpenIdConnect.Extensions;
using AspNet.Security.OpenIdConnect.Primitives;
using AspNet.Security.OpenIdConnect.Server;
using ESchool.Admin.ViewModels.Accounts;
using ESchool.Data.Entities.Accounts;
using ESchool.Data.Entities.Messages;
using ESchool.Data.Enums;
using ESchool.Services.Messages;
using ESchool.Services.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using OpenIddict.Core;
using OpenIddict.Models;
using MimeKit;
using MailKit.Net.Smtp;

namespace ESchool.API.Controllers
{
    [Route("api/[controller]")]
    public class AuthorizationController : Controller
    {
        private readonly ILogger<AuthorizationController> _logger;

        private readonly OpenIddictApplicationManager<OpenIddictApplication> _applicationManager;
        private readonly IOptions<IdentityOptions> _identityOptions;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;

        private readonly IEmailAccountService _emailAccountService;
        private readonly IQueuedEmailService _queuedEmailService;

        public AuthorizationController(
            ILogger<AuthorizationController> logger,
            OpenIddictApplicationManager<OpenIddictApplication> applicationManager,
            IOptions<IdentityOptions> identityOptions,
            SignInManager<ApplicationUser> signInManager,
            UserManager<ApplicationUser> userManager,
            IEmailAccountService emailAccountService,
            IQueuedEmailService queuedEmailService)
        {
            _logger = logger;

            _applicationManager = applicationManager;
            _identityOptions = identityOptions;
            _signInManager = signInManager;
            _userManager = userManager;

            _emailAccountService = emailAccountService;
            _queuedEmailService = queuedEmailService;
        }

        [Authorize, HttpGet("~/connect/authorize")]
        public async Task<IActionResult> Authorize(OpenIdConnectRequest request)
        {
            // Retrieve the application details from the database.
            var application = await _applicationManager.FindByClientIdAsync(request.ClientId, HttpContext.RequestAborted);

            if (application == null)
            {
                return BadRequest(new
                {
                    Error = OpenIdConnectConstants.Errors.InvalidClient,
                    ErrorDescription = "Details concerning the calling client application cannot be found in the database"
                });
            }

            // Flow the request_id to allow OpenIddict to restore
            // the original authorization request from the cache.
            return Ok(new
            {
                ApplicationName = application.DisplayName,
                RequestId = request.RequestId,
                Scope = request.Scope
            });
        }

        [Authorize, HttpPost("~/connect/logout")]
        public async Task<IActionResult> Logout()
        {
            // Ask ASP.NET Core Identity to delete the local and external cookies created
            // when the user agent is redirected from the external identity provider
            // after a successful authentication flow (e.g Google or Facebook).
            await _signInManager.SignOutAsync();

            // Returning a SignOutResult will ask OpenIddict to redirect the user agent
            // to the post_logout_redirect_uri specified by the client application.
            return SignOut(OpenIdConnectServerDefaults.AuthenticationScheme);
        }

        [HttpPost("~/connect/token"), Produces("application/json")]
        public async Task<IActionResult> Exchange(OpenIdConnectRequest request)
        {
            if (request.IsPasswordGrantType())
            {
                return await ProcessPasswordGrantType(request);
            }

            if (request.IsRefreshTokenGrantType())
            {
                return await ProcessRefreshTokenGrantType(request);
            }

            return BadRequest(new OpenIdConnectResponse
            {
                Error = OpenIdConnectConstants.Errors.UnsupportedGrantType,
                ErrorDescription = "The specified grant type is not supported."
            });
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

            await SendEmailAsync(
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

        [NonAction]
        private async Task<IActionResult> ProcessPasswordGrantType(OpenIdConnectRequest request)
        {
            // Retrieve the application details from the database.
            var application = await _applicationManager.FindByClientIdAsync(request.ClientId, HttpContext.RequestAborted);

            if (application == null)
            {
                return BadRequest(new OpenIdConnectResponse
                {
                    Error = OpenIdConnectConstants.Errors.InvalidClient,
                    ErrorDescription = "Details concerning the calling client application cannot be found in the database."
                });
            }

            ApplicationUser user = await _userManager.FindByNameAsync(request.Username);

            if (user == null)
            {
                return BadRequest(new OpenIdConnectResponse
                {
                    Error = OpenIdConnectConstants.Errors.InvalidGrant,
                    ErrorDescription = "The username/password couple is invalid."
                });
            }

            // Ensure the user is allowed to sign in.
            if (!await _signInManager.CanSignInAsync(user))
            {
                return BadRequest(new OpenIdConnectResponse
                {
                    Error = OpenIdConnectConstants.Errors.InvalidGrant,
                    ErrorDescription = "The specified user is not allowed to sign in."
                });
            }

            // Reject the token request if two-factor authentication has been enabled by the user.
            if (_userManager.SupportsUserTwoFactor && await _userManager.GetTwoFactorEnabledAsync(user))
            {
                return BadRequest(new OpenIdConnectResponse
                {
                    Error = OpenIdConnectConstants.Errors.InvalidGrant,
                    ErrorDescription = "The specified user is not allowed to sign in."
                });
            }

            // Ensure the user is not already locked out.
            if (_userManager.SupportsUserLockout && await _userManager.IsLockedOutAsync(user))
            {
                return BadRequest(new OpenIdConnectResponse
                {
                    Error = OpenIdConnectConstants.Errors.InvalidGrant,
                    ErrorDescription = "The username/password couple is invalid."
                });
            }

            // Ensure the password is valid.
            if (!await _userManager.CheckPasswordAsync(user, request.Password))
            {
                if (_userManager.SupportsUserLockout)
                {
                    await _userManager.AccessFailedAsync(user);
                }

                return BadRequest(new OpenIdConnectResponse
                {
                    Error = OpenIdConnectConstants.Errors.InvalidGrant,
                    ErrorDescription = "The username/password couple is invalid."
                });
            }

            if (_userManager.SupportsUserLockout)
            {
                await _userManager.ResetAccessFailedCountAsync(user);
            }

            // Create a new authentication ticket.
            AuthenticationTicket ticket = await CreateTicketAsync(request, user);

            return SignIn(ticket.Principal, ticket.Properties, ticket.AuthenticationScheme);
        }

        [NonAction]
        private async Task<IActionResult> ProcessRefreshTokenGrantType(OpenIdConnectRequest request)
        {
            // Retrieve the claims principal stored in the refresh token.
            AuthenticateInfo info = await HttpContext.Authentication.GetAuthenticateInfoAsync(OpenIdConnectServerDefaults.AuthenticationScheme);

            // Retrieve the user profile corresponding to the refresh token.
            // Note: if you want to automatically invalidate the refresh token
            // when the user password/roles change, use the following line instead:
            // var user = _signInManager.ValidateSecurityStampAsync(info.Principal);
            ApplicationUser user = await _userManager.GetUserAsync(info.Principal);

            if (user == null)
            {
                return BadRequest(new OpenIdConnectResponse
                {
                    Error = OpenIdConnectConstants.Errors.InvalidGrant,
                    ErrorDescription = "The refresh token is no longer valid."
                });
            }

            // Ensure the user is still allowed to sign in.
            if (!await _signInManager.CanSignInAsync(user))
            {
                return BadRequest(new OpenIdConnectResponse
                {
                    Error = OpenIdConnectConstants.Errors.InvalidGrant,
                    ErrorDescription = "The user is no longer allowed to sign in."
                });
            }

            // Create a new authentication ticket, but reuse the properties stored
            // in the refresh token, including the scopes originally granted.
            AuthenticationTicket ticket = await CreateTicketAsync(request, user, info.Properties);

            return SignIn(ticket.Principal, ticket.Properties, ticket.AuthenticationScheme);
        }

        [NonAction]
        private async Task<AuthenticationTicket> CreateTicketAsync(
            OpenIdConnectRequest request,
            ApplicationUser user,
            AuthenticationProperties properties = null)
        {
            // Create a new ClaimsPrincipal containing the claims that
            // will be used to create an id_token, a token or a code.
            ClaimsPrincipal principal = await _signInManager.CreateUserPrincipalAsync(user);

            // Create a new authentication ticket holding the user identity.
            AuthenticationTicket ticket = new AuthenticationTicket(principal, properties, OpenIdConnectServerDefaults.AuthenticationScheme);

            if (!request.IsRefreshTokenGrantType())
            {
                // Set the list of scopes granted to the client application.
                // Note: the offline_access scope must be granted
                // to allow OpenIddict to return a refresh token.
                ticket.SetScopes(new[]
                {
                    OpenIdConnectConstants.Scopes.OpenId,
                    OpenIdConnectConstants.Scopes.Email,
                    OpenIdConnectConstants.Scopes.Profile,
                    OpenIdConnectConstants.Scopes.OfflineAccess,
                    OpenIddictConstants.Scopes.Roles
                }.Intersect(request.GetScopes()));
            }

            ticket.SetResources("eschool.api");

            // Note: by default, claims are NOT automatically included in the access and identity tokens.
            // To allow OpenIddict to serialize them, you must attach them a destination, that specifies
            // whether they should be included in access tokens, in identity tokens or in both.
            foreach (var claim in ticket.Principal.Claims)
            {
                // Never include the security stamp in the access and identity tokens, as it's a secret value.
                if (claim.Type == _identityOptions.Value.ClaimsIdentity.SecurityStampClaimType)
                {
                    continue;
                }

                IList<string> destinations = new List<string>
                {
                    OpenIdConnectConstants.Destinations.AccessToken
                };

                // Only add the iterated claim to the id_token if the corresponding scope was granted to the client application.
                // The other claims will only be added to the access_token, which is encrypted when using the default format.
                if ((claim.Type == OpenIdConnectConstants.Claims.Name && ticket.HasScope(OpenIdConnectConstants.Scopes.Profile)) ||
                    (claim.Type == OpenIdConnectConstants.Claims.Email && ticket.HasScope(OpenIdConnectConstants.Scopes.Email)) ||
                    (claim.Type == OpenIdConnectConstants.Claims.Role && ticket.HasScope(OpenIddictConstants.Claims.Roles)))
                {
                    destinations.Add(OpenIdConnectConstants.Destinations.IdentityToken);
                }

                claim.SetDestinations(destinations);
            }

            return ticket;
        }

        [NonAction]
        private async Task SendEmailAsync(string email, string subject, string message)
        {
            EmailAccount emailAccount = await _emailAccountService.GetDefaultAsync();

            if (emailAccount == null)
            {
                _logger.LogWarning(
                    new EventId((int)ApiErrorCode.Undefined), 
                    $"[{nameof(AuthorizationController)} » {nameof(SendEmailAsync)}] Default Email Account is null.");

                return;
            }


            var msg = new MimeMessage();
            msg.From.Add(new MailboxAddress("ESchool Web", "eschoolapi@gmail.com"));
            msg.To.Add(new MailboxAddress("Lam Mai", email));
            msg.Subject = subject;
            msg.Body = new TextPart("plain")
            {
                Text = message
            };

            using (var client = new SmtpClient())
            {
                client.Connect("smtp.gmail.com", 587, false);
                client.AuthenticationMechanisms.Remove("XOAUTH2");
                client.Authenticate("eschoolapi@gmail.com", "1qaw3(OLP_");
                // Note: since we don't have an OAuth2 token, disable 	// the XOAUTH2 authentication mechanism.     client.Authenticate("anuraj.p@example.com", "password");
                client.Send(msg);
                client.Disconnect(true);
            }


            QueuedEmail queuedEmail = new QueuedEmail
            {
                From = emailAccount.Email,
                FromName = emailAccount.DisplayName,
                To = email,
                Subject = subject,
                Body = message,
                CreatedOnUtc = DateTime.UtcNow,
                Priority = (int)QueuedEmailPriority.High,
                EmailAccountId = emailAccount.Id
            };

            await _queuedEmailService.CreateAsync(queuedEmail);
        }
    }
}
