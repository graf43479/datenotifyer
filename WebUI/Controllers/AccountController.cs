using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Domain.Entities;
using UserStore.WEB.Models;
using UserStore.WEB.Services;
using Microsoft.EntityFrameworkCore;
using Domain;

namespace UserStore.WEB.Controllers
{
    [Authorize]
  // [ServiceFilter(typeof(ExceptionLoggerFilter))]
    public class AccountController : Controller
    {
        private readonly ILogger<AccountController> _logger;
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private ApplicationContext db;

        //public AccountController(ILogger<AccountController> logger, UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
        //{
        //    _logger = logger;
        //    _userManager = userManager;
        //    _signInManager = signInManager;
        //}
        public AccountController(ILogger<AccountController> logger, UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, ApplicationContext context)
        {
            _logger = logger;
            _userManager = userManager;
            _signInManager = signInManager;
            db = context;
        }


        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home"); //  View("Error", new string[] { "В доступе отказано" });
            }
            ViewBag.returnUrl = returnUrl;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginModel model, string returnUrl)
        {
            //Два способа подсчета неудачных попыток входа
            //1. Через встроенные средства Identity. Минус: работает только для зарегистрированного логина
            //2. Через сессию. Минус: сессия может сбрасываться роботом. 
            //Тем не менее совместим оба подхода. Можно будет ещё куки добавить       
            string key = "attempt";
            string keyCapthca = "Captcha";
            string tmp = HttpContext.Session.GetString(key);
            int attemptVal = 0;
            if (string.IsNullOrEmpty(HttpContext.Session.GetString(key)))
            {
                attemptVal = 0;
            }
            else
            {
                Int32.TryParse(HttpContext.Session.GetString(key), out attemptVal);
            }
            
            HttpContext.Session.SetString(key, (attemptVal + 1).ToString()); 
            //   Session["attempt"] = (Session["attempt"] == null) ? 0 : (int)Session["attempt"] + 1;

            //if ((Session["Captcha"] == null || Session["Captcha"].ToString() != model.Captcha) && (int)Session["attempt"] > 3)
            string captcha = HttpContext.Session.GetString(keyCapthca);

            //    if ((Session["Captcha"] == null || Session["Captcha"].ToString() != model.Captcha) && (int)Session["attempt"] > 3)
            if ((string.IsNullOrEmpty(captcha) || captcha != model.Captcha) && attemptVal > 3)
            {
                ModelState.AddModelError("Captcha", "Сумма введена неверно! Пожалуйста, повторите ещё раз!");
                return View(model);
            }

            if (ModelState.IsValid)
            {
                ClaimsIdentity claim = null;
                AppUser user = await _userManager.FindByEmailAsync(model.Email);
                if (user != null)
                {
                    var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, true, false);

                    if (result.Succeeded)
                    {
                        HttpContext.Session.SetInt32(key, 0);
                        if (String.IsNullOrEmpty(returnUrl) || returnUrl.Contains(Url.Action("Login", "Account")))
                        {
                            return RedirectToAction("Index", "Home", null);
                        }
                        else
                        {
                            Redirect(returnUrl);
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("", "Неправильный логин и (или) пароль");
                    }
                }
                

                //    UserDTO userDto = new UserDTO { Email = model.Email, Password = model.Password };
                //    ClaimsIdentity claim = await service.AuthenticateAsync(userDto);

                //    ClaimsIdentity claim = await _userManager.

                //    if (claim == null)
                //    {
                //        Session["attempt"] = await service.CheckForAttemptsAsync(model.Email);
                //        ModelState.AddModelError("", "Неверный логин или пароль.");

                //    }
                //    else
                //    {
                //        OperationDetails isConfirmed = await service.IsEmailConfirmedAsync(userDto);
                //        if (isConfirmed.Succedeed)
                //        {
                //            AuthenticationManager.SignOut();
                //            AuthenticationManager.SignIn(new AuthenticationProperties
                //            {
                //                IsPersistent = true
                //            }, claim);

                //            HttpContext.Session.SetInt32(key, 0);                        
                //            if (String.IsNullOrEmpty(returnUrl) || returnUrl.Contains(Url.Action("Login", "Account")))
                //            {
                //                return RedirectToAction("Index", "Home", null);
                //            }
                //            else
                //            {
                //                Redirect(returnUrl);
                //            }
                //        }
                //        else
                //        {
                //            AddErrorsFromResult(isConfirmed);
                //        }
                //    }
                //}
                //return View(model);
            }
            return View(model);
        }

        public ActionResult Logout()
        {
            _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Register()
        {
            return View();
        }

        
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                //AppUser user = new AppUser { Email = model.Email, UserName = model.Email, ClientProfile = new ClientProfile { Adress = "Titova5", Name = "Oleg" } };
                AppUser user = new AppUser { Email = model.Email, UserName = model.Email, ClientProfile = new ClientProfile() { Adress = model.Address, Name=model.Name} };
                // добавляем пользователя
                //ClientProfile profile 

               

                //var result = await _userManager.CreateAsync(user, model.Password);
                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    //string callbackUrlBase = Url.Action("ConfirmEmail", "Account", null, protocol: HttpContext.Request.Path);
                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    string callbackUrlBase = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code },
                        protocol: HttpContext.Request.Scheme);
                    //var callbackUrl = callbackUrlBase + "?userId=" + user.Id + "&code=" + HttpUtility.UrlEncode(code);

                    EmailService emailService = new EmailService();
                    string message = "Для завершения регистрации перейдите по ссылке:: <a href=\""
                                                       + callbackUrlBase + "\">завершить регистрацию</a>";

                    await emailService.SendEmailAsync(user.Email, "Подтверждение электронной почты", message);
                    
                    //await _userManager.AddToRoleAsync(user, "user");
                    
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }
            return View(model);
        }


        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> ConfirmEmail(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return View("Error");
            }
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return View("Error");
            }
            var result = await _userManager.ConfirmEmailAsync(user, code);
            if (result.Succeeded)
                return RedirectToAction("Index", "Home");
            else
                return View("Error");
        }



        //страница указания email для восстановления пароля
        [AllowAnonymous]
        public ActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
              

                AppUser user = await _userManager.FindByEmailAsync(model.Email);
                if (user == null || !(await _userManager.IsEmailConfirmedAsync(user)))
                {
                    ModelState.AddModelError(string.Empty, $"Пользователь с email {model.Email} не найден");
                }
                else
                {
                    string code = await _userManager.GeneratePasswordResetTokenAsync(user);

                    string callbackUrlBase = Url.Action("ResetPassword", "Account", new { code = code },
                           protocol: HttpContext.Request.Scheme);

                    EmailService emailService = new EmailService();
                    string message = "Для сброса пароля, перейдите по ссылке <a href=\"" + callbackUrlBase + "\">сбросить</a>" ;

                    await emailService.SendEmailAsync(user.Email, "Подтверждение электронной почты", message);

                    return RedirectToAction("ForgotPasswordConfirmation", "Account");
                }
               
                


            }
            return View(model);
        }


        //Страница сообщения о том, что на почту выслана ссылка для восстановления пароля
        [AllowAnonymous]
        public ActionResult ForgotPasswordConfirmation()
        {
            return View();
        }


        //Подтверждение установки нового пароля
        [AllowAnonymous]
        public ActionResult ResetPasswordConfirmation()
        {
            return View();
        }

        [AllowAnonymous]
        public ActionResult ResetPassword(string code)
        {
            return code == null ? View("Error") : View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await _userManager.FindByNameAsync(model.Email);
            if (user == null)
            {
                ModelState.AddModelError(string.Empty, $"Пользователь с email {model.Email} не найден");
            }

            var result =await _userManager.ResetPasswordAsync(user, model.Code, model.Password);
            if (result.Succeeded)
            {
                // Don't reveal that the user does not exist
                return RedirectToAction("ResetPasswordConfirmation", "Account");
            }
            return RedirectToAction("ResetPasswordConfirmation", "Account");
        }


        public async Task<ActionResult> EditAccount()
        {
            string userName = HttpContext.User.Identity.Name; // User.FindFirst(ClaimTypes.Email).Value; // HttpContext.User.Identity.Name;
            AppUser user = await _userManager.FindByNameAsync(userName);

            AppUser user2 = _userManager.Users.FirstOrDefault(x => x.Id == user.Id);
            ClientProfile profile = db.ClientProfiles.FirstOrDefault(x => x.ClientProfileID == user.Id);
           // ClientProfile profile = _userManager.

            AppUser user3 = await _userManager.FindByEmailAsync(user.Email);
            //взять имя из контекста, вернуть в модель,
            //приянть обновленную модель и переслать на update
            RegisterModel model = new RegisterModel()
            {
                Email = user.Email,
                Address = profile.Adress,
                Name = profile.Name
            };

            return View(model);
        }


        [HttpPost]
        public async Task<ActionResult> EditAccount(RegisterModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            string userName = HttpContext.User.Identity.Name;
            AppUser user = await db.Users.Include(x=>x.ClientProfile).AsNoTracking().FirstOrDefaultAsync(x => x.Email == userName); // 
          //  _userManager.FindByNameAsync(userName);
         //   ClientProfile profile = db.ClientProfiles.FirstOrDefault(x => x.ClientProfileID == user.Id);

            user.Email = model.Email;
            user.UserName = model.Email;
            user.ClientProfile.Name = model.Name;
            user.ClientProfile.Adress = model.Address;
            var validEmail = await _userManager.UserValidators.First().ValidateAsync(_userManager, user);

            if (!validEmail.Succeeded)
            {                ModelState.AddModelError(string.Empty, $"Новые данные пользователя не валидны");
            }

            IdentityResult validPass = null;
            if (model.Password != string.Empty)
            {
                validPass
                    = await _userManager.PasswordValidators.First().ValidateAsync(_userManager, user, model.Password);

                if (validPass.Succeeded)
                {
                    user.PasswordHash =
                        _userManager.PasswordHasher.HashPassword(user, model.Password);
                }
                else
                {
                    ModelState.AddModelError(string.Empty, $"Пароль некорректен");

                    //details.Messages.Append(validPass.Errors)
                    //AddErrorsFromResult(validPass);
                }

                if ((validEmail.Succeeded && validPass == null) ||
                    (validEmail.Succeeded && model.Password != string.Empty && validPass.Succeeded))
                {
                    //user.ClientProfile = profile;
                    IdentityResult result = await _userManager.UpdateAsync(user);
                    if (result.Succeeded)
                    {
                        return View("Success");
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, $"Не удалось обновить информацию о польователе");
                    }
                }
            }
            return View(model);
            }


        /*
        [AllowAnonymous]
        public ActionResult CaptchaImage(string prefix, bool noisy = true)
        {
            var rand = new Random((int)DateTime.Now.Ticks);

            //generate new question
            int a = rand.Next(10, 99);
            int b = rand.Next(0, 9);
            var captcha = string.Format("{0} + {1} = ?", a, b);

            //store answer
            Session["Captcha" + prefix] = a + b;

            //image stream
            FileContentResult img = null;

            using (var mem = new MemoryStream())
            using (var bmp = new Bitmap(130, 30))
            using (var gfx = Graphics.FromImage((Image)bmp))
            {
                gfx.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;
                gfx.SmoothingMode = SmoothingMode.AntiAlias;
                gfx.FillRectangle(Brushes.White, new Rectangle(0, 0, bmp.Width, bmp.Height));

                //add noise
                if (noisy)
                {
                    int i, r, x, y;
                    var pen = new Pen(Color.Yellow);
                    for (i = 1; i < 10; i++)
                    {
                        pen.Color = Color.FromArgb(
                            (rand.Next(0, 255)),
                            (rand.Next(0, 255)),
                            (rand.Next(0, 255)));

                        r = rand.Next(0, (130 / 3));
                        x = rand.Next(0, 130);
                        y = rand.Next(0, 30);

                        gfx.DrawEllipse(pen, x - r, y - r, r, r);
                    }
                }

                //add question
                gfx.DrawString(captcha, new Font("Tahoma", 15), Brushes.Gray, 2, 3);

                //render as Jpeg
                bmp.Save(mem, System.Drawing.Imaging.ImageFormat.Jpeg);
                img = this.File(mem.GetBuffer(), "image/Jpeg");
            }

            return img;
        }
        */

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}