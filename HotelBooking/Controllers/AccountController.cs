using HotelBooking.Models;
using HotelBooking.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NETCore.MailKit.Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace HotelBooking.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly IEmailService emailService;
        private readonly ILogger<AccountController> logger;
        private readonly IWebHostEnvironment hostingEnvironment;

        public AccountController(UserManager<ApplicationUser> userManager,
                                 SignInManager<ApplicationUser> signInManager,
                                 IEmailService emailService,
                                 ILogger<AccountController> logger,
                                 IWebHostEnvironment hostingEnvironment)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.emailService = emailService;
            this.logger = logger;
            this.hostingEnvironment = hostingEnvironment;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                string uniqueFileName = ProccessUploadedFile(model);

                ApplicationUser user = new ApplicationUser()
                {
                    Name = model.Name,
                    Surname = model.Surname,
                    UserName = model.Email,
                    Email = model.Email,
                    PhotoPath = uniqueFileName
                };

                var result = await userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    //Generate email confirmation token
                    string token = await userManager.GenerateEmailConfirmationTokenAsync(user);

                    //Build confirmation link, user clicks this link to confirm the email, this link executes VerifyEmail action in Account controller
                    //Request.Scheme generates absolute URL
                    string link = Url.Action(nameof(VerifyEmail), "Account", new { userId = user.Id, token = token }, Request.Scheme);

                    //Sends the email to user with whole information
                    await emailService.SendAsync(user.Email, "Email verify", $"<p>To confirm your email please click -> <a href=\"{link}\">Verify your Email</a></p>", true);

                    return RedirectToAction("EmailVerification", "Account");
                }

                foreach (var error in result.Errors)
                {
                    logger.LogError($"{error.Description}");
                    logger.LogError($"{error}");
                    ModelState.AddModelError("", error.Description);
                }
            }

            //if Model.State is not valid so we want to rerender the register view and display any validation errors
            return View(model);
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model, string returnUrl)
        {

            if (ModelState.IsValid)
            {
                var user = await userManager.FindByEmailAsync(model.Email);

                if (user != null && !user.EmailConfirmed && await userManager.CheckPasswordAsync(user, model.Password))
                {
                    ModelState.AddModelError("", "Email not confirmed yet");
                    return View(model);
                }

                var result = await signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, false);

                if (result.Succeeded)
                {
                    if (!String.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                    {
                        return Redirect(returnUrl);
                    }
                    else
                    {
                        return RedirectToAction("index", "home");
                    }
                }

                ModelState.AddModelError("", "Invalid Login Attempt");
            }

            return View(model);
        }

        [HttpGet]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await userManager.FindByEmailAsync(model.Email);

                if (user != null && await userManager.IsEmailConfirmedAsync(user))
                {
                    //Generate password reset token 
                    string token = await userManager.GeneratePasswordResetTokenAsync(user);
                    //Build password reset link
                    string passwordResetLink = Url.Action(nameof(ResetPassword), "Account", new { email = model.Email, token = token }, Request.Scheme);

                    //Sends the email to user with whole information
                    await emailService.SendAsync(user.Email, "Password reset", $"<p>Here your password reset link click -> <a href=\"{passwordResetLink}\">Reset your Password</a></p>", true);


                    return View("ForgotPasswordConfirmation");
                }

                return View("ForgotPasswordConfirmation");

            }
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();

            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult ResetPassword(string email, string token)
        {
            if (email == null || token == null)
            {
                logger.LogWarning("Invalid password reset token during password reset.");
                ModelState.AddModelError("", "Invalid password reset token");
            }
            //Неявно присвает полям модели значение параметров имена которых совпадают
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await userManager.FindByEmailAsync(model.Email);

                if (user != null)
                {
                    var result = await userManager.ResetPasswordAsync(user, model.Token, model.ConfirmPassword);

                    if (result.Succeeded)
                    {
                        return View("ResetPasswordConfirmation");
                    }
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                    return View(model);
                }
                return View("ResetPasswordConfirmation");
            }
            return View(model);
        }
        public IActionResult EmailVerification()
        {
            return View();
        }

        public async Task<IActionResult> VerifyEmail(string userId, string token)
        {
            ViewData["ErrorMessage"] = "Something went wrong";
            if (userId == null || token == null)
            {
                return View("NotFound");
            }

            var user = await userManager.FindByIdAsync(userId);
            
            if (user == null)
            {
                return View("NotFound");
            }

            var result = await userManager.ConfirmEmailAsync(user, token);

            if (result.Succeeded)
            {
                return View();
            }

            return View("NotFound");
        }

        private string ProccessUploadedFile(RegisterViewModel model)
        {
            string uniqueFileName = null;

            if (model.Photo != null)
            {

                string uploadsFolder = Path.Combine(/*path to wwwroot folder*/
                                                     hostingEnvironment.WebRootPath, "img\\user"); //Gat path wwwroot/img
                uniqueFileName = Guid.NewGuid() + "_" + model.Photo.FileName; //unique img file
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                try
                {
                    using (FileStream fs = new FileStream(filePath, FileMode.Create))
                    {
                        //Copy the photo to our server, into wwwroot/img folder
                        model.Photo.CopyTo(fs);
                    }
                }
                catch (Exception ex)
                {
                    logger.LogError("Something went wrong when photo was coping to the server.");
                    logger.LogError(ex.ToString());
                }

            }

            return uniqueFileName;
        }
    }
}
