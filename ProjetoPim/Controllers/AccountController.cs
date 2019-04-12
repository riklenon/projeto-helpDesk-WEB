using System;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ProjetoPim.Data;
using ProjetoPim.Models;
using ProjetoPim.Models.AccountViewModels;
using ProjetoPim.Services;


namespace ProjetoPim.Controllers
{

    //Controlador de contas do usuário
    [Authorize]
    [Route("[controller]/[action]")]
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager; //Declarando o item que gerencia os dados de usuário
        private readonly SignInManager<ApplicationUser> _signInManager;//Declarando o item que gerencia o Login
        private readonly ApplicationDbContext _context;
        private readonly IEmailSender _emailSender;
        private readonly ILogger _logger;
        //private readonly DepartmentService _departmentService;





        //Método construtor do AccountController
        public AccountController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IEmailSender emailSender,
            ILogger<AccountController> logger,
            ApplicationDbContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _emailSender = emailSender;
            _logger = logger;
            _context = context;
        }

        [TempData]
        public string ErrorMessage { get; set; }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Login(string returnUrl = null)
        {
            // Clear the existing external cookie to ensure a clean login process
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            if (ModelState.IsValid)
            {
                // This doesn't count login failures towards account lockout
                // To enable password failures to trigger account lockout, set lockoutOnFailure: true
                var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, lockoutOnFailure: false);
                if (result.Succeeded)
                {
                    _logger.LogInformation("User logged in.");
                    return RedirectToLocal(returnUrl);
                }
               /* if (result.RequiresTwoFactor)
                {
                    return RedirectToAction(nameof(LoginWith2fa), new { returnUrl, model.RememberMe });
                }
                if (result.IsLockedOut)       POSSO DELETAR
                {
                    _logger.LogWarning("User account locked out.");
                    return RedirectToAction(nameof(Lockout));
                }*/
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                    return View(model);
                }
            }

            // Se chegou até aqui é problema no formulário
            return View(model);
        }

        [Authorize(Roles = "Nivel 3")]
        public ActionResult UserIndex()
        {
            return View();
        }

        [Authorize(Roles = "Nivel 3")]
        public ActionResult Consultar()
        {
            return View();
        }

        [Authorize(Roles = "Nivel 3")]
        public ActionResult Departamento()
        {
            return View();
        }

        /* [HttpGet]
         [AllowAnonymous]
         public async Task<IActionResult> LoginWith2fa(bool rememberMe, string returnUrl = null)
         {
             // Ensure the user has gone through the username & password screen first
             var user = await _signInManager.GetTwoFactorAuthenticationUserAsync();

             if (user == null)
             {
                 throw new ApplicationException($"Unable to load two-factor authentication user.");
             }

             var model = new LoginWith2faViewModel { RememberMe = rememberMe };
             ViewData["ReturnUrl"] = returnUrl;

             return View(model);
         }*/

        /*[HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LoginWith2fa(LoginWith2faViewModel model, bool rememberMe, string returnUrl = null)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await _signInManager.GetTwoFactorAuthenticationUserAsync();
            if (user == null)
            {
                throw new ApplicationException($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            var authenticatorCode = model.TwoFactorCode.Replace(" ", string.Empty).Replace("-", string.Empty);

            var result = await _signInManager.TwoFactorAuthenticatorSignInAsync(authenticatorCode, rememberMe, model.RememberMachine);

            if (result.Succeeded)
            {
                _logger.LogInformation("User with ID {UserId} logged in with 2fa.", user.Id);
                return RedirectToLocal(returnUrl);
            }
            else if (result.IsLockedOut)
            {
                _logger.LogWarning("User with ID {UserId} account locked out.", user.Id);
                return RedirectToAction(nameof(Lockout));
            }
            else
            {
                _logger.LogWarning("Invalid authenticator code entered for user with ID {UserId}.", user.Id);
                ModelState.AddModelError(string.Empty, "Invalid authenticator code.");
                return View();
            }
        }*/

        /* [HttpGet]
         [AllowAnonymous]
         public async Task<IActionResult> LoginWithRecoveryCode(string returnUrl = null)
         {
             // Ensure the user has gone through the username & password screen first
             var user = await _signInManager.GetTwoFactorAuthenticationUserAsync();
             if (user == null)
             {
                 throw new ApplicationException($"Unable to load two-factor authentication user.");
             }

             ViewData["ReturnUrl"] = returnUrl;

             return View();
         }*/

        /* [HttpPost]
         [AllowAnonymous]
         [ValidateAntiForgeryToken]
         public async Task<IActionResult> LoginWithRecoveryCode(LoginWithRecoveryCodeViewModel model, string returnUrl = null)
         {
             if (!ModelState.IsValid)
             {
                 return View(model);
             }

             var user = await _signInManager.GetTwoFactorAuthenticationUserAsync();
             if (user == null)
             {
                 throw new ApplicationException($"Unable to load two-factor authentication user.");
             }

             var recoveryCode = model.RecoveryCode.Replace(" ", string.Empty);

             var result = await _signInManager.TwoFactorRecoveryCodeSignInAsync(recoveryCode);

             if (result.Succeeded)
             {
                 _logger.LogInformation("User with ID {UserId} logged in with a recovery code.", user.Id);
                 return RedirectToLocal(returnUrl);
             }
             if (result.IsLockedOut)
             {
                 _logger.LogWarning("User with ID {UserId} account locked out.", user.Id);
                 return RedirectToAction(nameof(Lockout));
             }
             else
             {
                 _logger.LogWarning("Invalid recovery code entered for user with ID {UserId}", user.Id);
                 ModelState.AddModelError(string.Empty, "Invalid recovery code entered.");
                 return View();
             }
         }*/

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Lockout()
        {
            return View();
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Register(string returnUrl = null)
        {
            RegisterViewModel R = new RegisterViewModel();
            ViewData["ReturnUrl"] = returnUrl;
            return View(R);
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            if (ModelState.IsValid)
            {                                    //apagar
                var user = new ApplicationUser { UserName = model.Email, Email = model.Email, PhoneNumber = model.PhoneNumber, Nome = model.Nome, Departamento = model.Departamentos, Cpf = model.Cpf };
                var result = await _userManager.CreateAsync(user, model.Password);
                await _userManager.AddToRoleAsync(user, model.Role="Nivel 1");
                if (result.Succeeded)
                {
                    _logger.LogInformation("Usuário Criado.");

                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    var callbackUrl = Url.EmailConfirmationLink(user.Id, code, Request.Scheme);
                    await _emailSender.SendEmailConfirmationAsync(model.Email, callbackUrl);

                    await _signInManager.SignInAsync(user, isPersistent: false);
                    _logger.LogInformation("Usuário Criado.");
                    return RedirectToLocal(returnUrl);
                }
                AddErrors(result);
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult RegisterAdmin(string returnUrl = null)
        {
            RegisterViewModel R = new RegisterViewModel();
            R.getRoles(_context);
            ViewData["ReturnUrl"] = returnUrl;
            return View(R);
        }



        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RegisterAdmin(RegisterViewModel model, string returnUrl = null)
        {

            // RegisterViewModel R = new RegisterViewModel();

            ViewData["ReturnUrl"] = returnUrl;
            if (ModelState.IsValid)
            {                                    //apagar
                var user = new ApplicationUser { UserName = model.Email, Email = model.Email, PhoneNumber = model.PhoneNumber, Nome = model.Nome, Cpf = model.Cpf };
                var result = await _userManager.CreateAsync(user, model.Password);
                await _userManager.AddToRoleAsync(user, model.Role);
                if (result.Succeeded)
                {
                    _logger.LogInformation("User created a new account with password.");

                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    var callbackUrl = Url.EmailConfirmationLink(user.Id, code, Request.Scheme);
                    await _emailSender.SendEmailConfirmationAsync(model.Email, callbackUrl);

                    await _signInManager.SignInAsync(user, isPersistent: false);
                    _logger.LogInformation("User created a new account with password.");
                    return RedirectToLocal(returnUrl);
                }
                AddErrors(result);
            }
            return View(model);
        }

        [Authorize(Roles = "Nivel 3")]
        public ActionResult UserList()
        {
            return View(_context.Users.ToList());
        }


        [Authorize(Roles = "Nivel 3")]
        public ActionResult UserListDepartamentoAdm()
        {
            return View(_context.Users.Where(x => x.Departamento == Models.Enums.EnumDepartamento.Administrativo).ToList());
        }


        [Authorize(Roles = "Nivel 3")]
        public ActionResult UserListDepartamentoCon()
        {
            return View(_context.Users.Where(x => x.Departamento == Models.Enums.EnumDepartamento.Contabil).ToList());
        }


        [Authorize(Roles = "Nivel 3")]
        public ActionResult UserListDepartamentoSup()
        {
            return View(_context.Users.Where(x => x.Departamento == Models.Enums.EnumDepartamento.Suporte).ToList());
        }


        [Authorize(Roles = "Nivel 3")]
        public ActionResult UserListDepartamentoSeg()
        {
            return View(_context.Users.Where(x => x.Departamento == Models.Enums.EnumDepartamento.Seguranca).ToList());
        }


        [Authorize(Roles = "Nivel 3")]
        public ActionResult UserDelete(string id)
        {
            var user = _context.Users.Where(u => u.Id == id).FirstOrDefault();
            return View(user);
        }

        [Authorize(Roles = "Nivel 3")]
        [HttpPost]
        public ActionResult UserDelete(ApplicationUser appuser, RegisterViewModel role)
        {
            var user = _context.Users.Where(u => u.Id == appuser.Id).FirstOrDefault();
            //user.RoleName = role.Role;
            _context.Users.Remove(user);

            try
            {
                _context.SaveChanges();
                //var user = context.Users.Where(u => u.Id == id.ToString()).FirstOrDefault();
                return RedirectToAction("UserIndex");
            }
            catch (DbUpdateException e)
            {
                return RedirectToAction(nameof(Error), new { message = e.Message });
            }
           
        }

        [Authorize(Roles="Nivel 3")]
        public async Task<ActionResult> AtribuirNivel3(ApplicationUser appuser, RegisterViewModel model)
        {
            var user = _context.Users.Where(u => u.Id == appuser.Id).FirstOrDefault();

            var result = await _userManager.UpdateAsync(user);

            //await _userManager.RemoveFromRoleAsync(user, model.Role);
            await _userManager.AddToRoleAsync(user, model.Role="Nivel 3");

            if (result.Succeeded)
            {
                return RedirectToAction("UserIndex");
            }
            return View(model);
        }

        [Authorize(Roles = "Nivel 3")]
        public async Task<ActionResult> AtribuirNivel2(ApplicationUser appuser, RegisterViewModel model)
        {
            var user = _context.Users.Where(u => u.Id == appuser.Id).FirstOrDefault();

            var result = await _userManager.UpdateAsync(user);

            //await _userManager.RemoveFromRoleAsync(user, model.Role);
            await _userManager.AddToRoleAsync(user, model.Role = "Nivel 2");

            if (result.Succeeded)
            {
                return RedirectToAction("UserIndex");
            }
            return View(model);
        }




        //GET Usuário
        [Authorize(Roles = "Nivel 3")]
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }


            var user = await _context.Users.FirstOrDefaultAsync(obj => obj.Id == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }


        //Ação de Error
        public IActionResult Error (string message)
        {
            var viewModel = new ErrorViewModel
            {
                Message = message="O usuário possui chamados em aberto e não pode ser excluído!",
                RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
            };
            return View(viewModel);
        }



        [HttpGet]
        [AllowAnonymous]
        public IActionResult UserEdit(string returnUrl = null)
        {
            RegisterViewModel R = new RegisterViewModel();
            R.getRoles(_context);
            ViewData["ReturnUrl"] = returnUrl;
            //R.Role = User.Identity.AuthenticationType;
            return View(R);
        }


        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UserEdit(ApplicationUser appuser, string id, RegisterViewModel model, string returnUrl = null)
        {
            RegisterViewModel R = new RegisterViewModel();
            R.getRoles(_context);
            ViewData["ReturnUrl"] = returnUrl;
            if (ModelState.IsValid)
            {                                    //apagar
                //appuser = new ApplicationUser { UserName = model.Email, Email = model.Email, PhoneNumber = model.PhoneNumber, Nome = model.Nome };
                appuser = _context.Users.Where(u => u.Id == id).FirstOrDefault();
                var result = await _userManager.UpdateAsync(appuser);
                await _userManager.AddToRoleAsync(appuser, model.Role);
                if (result.Succeeded)
                {
                    _logger.LogInformation("User created a new account with password.");

                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(appuser);
                    var callbackUrl = Url.EmailConfirmationLink(appuser.Id, code, Request.Scheme);
                    await _emailSender.SendEmailConfirmationAsync(model.Email, callbackUrl);

                    await _signInManager.SignInAsync(appuser, isPersistent: false);
                    _logger.LogInformation("User created a new account with password.");
                    return RedirectToLocal(returnUrl);
                }
                AddErrors(result);
            }
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            _logger.LogInformation("User logged out.");
            return RedirectToAction(nameof(HomeController.Index), "Home");
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public IActionResult ExternalLogin(string provider, string returnUrl = null)
        {
            // Request a redirect to the external login provider.
            var redirectUrl = Url.Action(nameof(ExternalLoginCallback), "Account", new { returnUrl });
            var properties = _signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);
            return Challenge(properties, provider);
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> ExternalLoginCallback(string returnUrl = null, string remoteError = null)
        {
            if (remoteError != null)
            {
                ErrorMessage = $"Error from external provider: {remoteError}";
                return RedirectToAction(nameof(Login));
            }
            var info = await _signInManager.GetExternalLoginInfoAsync();
            if (info == null)
            {
                return RedirectToAction(nameof(Login));
            }

            // Sign in the user with this external login provider if the user already has a login.
            var result = await _signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, isPersistent: false, bypassTwoFactor: true);
            if (result.Succeeded)
            {
                _logger.LogInformation("User logged in with {Name} provider.", info.LoginProvider);
                return RedirectToLocal(returnUrl);
            }
            if (result.IsLockedOut)
            {
                return RedirectToAction(nameof(Lockout));
            }
            else
            {
                // If the user does not have an account, then ask the user to create an account.
                ViewData["ReturnUrl"] = returnUrl;
                ViewData["LoginProvider"] = info.LoginProvider;
                var email = info.Principal.FindFirstValue(ClaimTypes.Email);
                return View("ExternalLogin", new ExternalLoginViewModel { Email = email });
            }
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ExternalLoginConfirmation(ExternalLoginViewModel model, string returnUrl = null)
        {
            if (ModelState.IsValid)
            {
                // Get the information about the user from the external login provider
                var info = await _signInManager.GetExternalLoginInfoAsync();
                if (info == null)
                {
                    throw new ApplicationException("Error loading external login information during confirmation.");
                }
                var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
                var result = await _userManager.CreateAsync(user);
                if (result.Succeeded)
                {
                    result = await _userManager.AddLoginAsync(user, info);
                    if (result.Succeeded)
                    {
                        await _signInManager.SignInAsync(user, isPersistent: false);
                        _logger.LogInformation("User created an account using {Name} provider.", info.LoginProvider);
                        return RedirectToLocal(returnUrl);
                    }
                }
                AddErrors(result);
            }

            ViewData["ReturnUrl"] = returnUrl;
            return View(nameof(ExternalLogin), model);
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> ConfirmEmail(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return RedirectToAction(nameof(HomeController.Index), "Home");
            }
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                throw new ApplicationException($"Unable to load user with ID '{userId}'.");
            }
            var result = await _userManager.ConfirmEmailAsync(user, code);
            return View(result.Succeeded ? "ConfirmEmail" : "Error");
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user == null || !(await _userManager.IsEmailConfirmedAsync(user)))
                {
                    // Don't reveal that the user does not exist or is not confirmed
                    return RedirectToAction(nameof(ForgotPasswordConfirmation));
                }

                // For more information on how to enable account confirmation and password reset please
                // visit https://go.microsoft.com/fwlink/?LinkID=532713
                var code = await _userManager.GeneratePasswordResetTokenAsync(user);
                var callbackUrl = Url.ResetPasswordCallbackLink(user.Id, code, Request.Scheme);
                await _emailSender.SendEmailAsync(model.Email, "Reset Password",
                   $"Please reset your password by clicking here: <a href='{callbackUrl}'>link</a>");
                return RedirectToAction(nameof(ForgotPasswordConfirmation));
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult ForgotPasswordConfirmation()
        {
            return View();
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult ResetPassword(string code = null)
        {
            if (code == null)
            {
                throw new ApplicationException("A code must be supplied for password reset.");
            }
            var model = new ResetPasswordViewModel { Code = code };
            return View(model);
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                // Don't reveal that the user does not exist
                return RedirectToAction(nameof(ResetPasswordConfirmation));
            }
            var result = await _userManager.ResetPasswordAsync(user, model.Code, model.Password);
            if (result.Succeeded)
            {
                return RedirectToAction(nameof(ResetPasswordConfirmation));
            }
            AddErrors(result);
            return View();
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult ResetPasswordConfirmation()
        {
            return View();
        }


        [HttpGet]
        public IActionResult AccessDenied()
        {
            return View();
        }

        #region Helpers

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }

        private IActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction(nameof(HomeController.Index), "Home");
            }
        }

        #endregion
    }
}
