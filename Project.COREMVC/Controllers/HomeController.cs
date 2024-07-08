using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Project.COMMON.Tools;
using Project.COREMVC.Models;
using Project.COREMVC.Models.PageVms.AppUsers;
using Project.COREMVC.Models.PageVms.ForgotPassword;
using Project.COREMVC.Models.PageVms.ResetPassword;
using Project.COREMVC.Models.PureVms.AppUsers;
using Project.COREMVC.Models.PureVms.ForgotPassword;
using Project.ENTITIES.Models;
using System.Diagnostics;
using SignInResult = Microsoft.AspNetCore.Identity.SignInResult;

namespace Project.COREMVC.Controllers
{
    [AutoValidateAntiforgeryToken]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        readonly UserManager<AppUser> _userManager;
        readonly RoleManager<AppRole> _roleManager;
        readonly SignInManager<AppUser> _signInManager;


        public HomeController(ILogger<HomeController> logger, UserManager<AppUser> userManager, RoleManager<AppRole> roleManager, SignInManager<AppUser> signInManager)
        {
            _logger = logger;
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(UserRegisterPageVm userRegisterPageVm)
        {
            Guid specId = Guid.NewGuid();
            AppUser appUser = new()
            {
                UserName = userRegisterPageVm.UserRegisterModel.UserName,
                Email = userRegisterPageVm.UserRegisterModel.Email,
                ActivationCode =specId
            };
            IdentityResult result  = await _userManager.CreateAsync(appUser,userRegisterPageVm.UserRegisterModel.Password);

            if (result.Succeeded)
            {
                AppRole appRole  = await _roleManager.FindByNameAsync("Member");
                if (appRole == null)
                {
                    await _roleManager.CreateAsync(new() { Name = "Member" });
                }
                await _userManager.AddToRoleAsync(appUser, "Member");

                string body = $"Hesabýnýz oluþturulmuþtur. Lütfen üyeliðinizi onaylamak için http://localhost:5062/Home/ConfirmEmail?specId={specId}&id={appUser.Id} linkine týklayýný iyi günler dileriz...";

                MailService.Send(userRegisterPageVm.UserRegisterModel.Email, body : body);
                TempData["message"] = "Mailinizi kontrol ediniz";
                return RedirectToAction( "RedirectPanel");
            }

            foreach (IdentityError error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }

            
            return View(userRegisterPageVm);
        }

        public async Task<IActionResult> ConfirmEmail(Guid specId , int id)
        {
            AppUser user = await _userManager.FindByIdAsync(id.ToString());
            if (user == null)
            {
                TempData["message"] = "Kullanýcý bulunamadý";
                return RedirectToAction("RedirectPanel");
            }
            else if (user.ActivationCode == specId)
            {
                user.EmailConfirmed = true;
                await _userManager.UpdateAsync(user);
                TempData["message"] = "Emailiniz baþarýlý bir þekilde onaylanmýþtýr";
                return RedirectToAction("SignIn");
            }
            return RedirectToAction("Register");
        }
        public IActionResult RedirectPanel()
        {
            return View();
        }

        public IActionResult SignIn(string returnUrl)
        {
            UserSignInPageVm userSignInPageVm = new()
            {
                UserSignInRequestModel = new()
                {
                    ReturnUrl = returnUrl
                }
            };
            return View(userSignInPageVm);
        }
        [HttpPost]
        public async Task<IActionResult> SignIn(UserSignInPageVm pageVm)
        {
           
            
            AppUser appUser = await _userManager.FindByNameAsync(pageVm.UserSignInRequestModel.UserName);


            if (appUser != null)
            {
                SignInResult result = await _signInManager.PasswordSignInAsync(appUser, pageVm.UserSignInRequestModel.Password, pageVm.UserSignInRequestModel.RememberMe, true);


                if (result.Succeeded)
                {
                    if (!string.IsNullOrEmpty(pageVm.UserSignInRequestModel.ReturnUrl))
                    {
                        return Redirect(pageVm.UserSignInRequestModel.ReturnUrl);
                    }


                    IList<string> roles = await _userManager.GetRolesAsync(appUser);

                    if (roles.Contains("Admin"))
                    {
                        //                   Action ismi  Controller ismi  Route Value'sü
                        return RedirectToAction("GetCategories", "Category", new { Area = "Admin" });
                    }
                    else if (roles.Contains("Member"))
                    {
                        return RedirectToAction("Index" ,  "Shopping" );
                    }
                    return RedirectToAction("SignIn");
                }
                else if (result.IsLockedOut)
                {
                    DateTimeOffset? lockOutEndDate = await _userManager.GetLockoutEndDateAsync(appUser);
                    ModelState.AddModelError("", $"Hesabýnýz {(lockOutEndDate.Value.UtcDateTime -DateTime.UtcNow).TotalSeconds:0.00} boyunca kilitlenmiþtir lütfen bekleyiniz");
                }
                else if (result.IsNotAllowed) // Mail onaylý deðildir
                {
                    return RedirectToAction("MailPanel");
                }
                else 
                {
                    string message = "";
                    if(appUser!= null)
                    {
                        int maxFailed = _userManager.Options.Lockout.MaxFailedAccessAttempts;
                        message = $" Eðer {maxFailed - await _userManager.GetAccessFailedCountAsync(appUser)} kez daha yanlýþ girerseniz hesabýnýz {_userManager.Options.Lockout.DefaultLockoutTimeSpan} süreyle kapatýlacaktýr";
                    }
                    //else
                    //{
                    //    message = "Kullanýcý bulunamadý";
                    //}
                    ModelState.AddModelError("", message);
                }
               
            }
            
            return View(pageVm);

        }

        public IActionResult MailPanel()
        {
            return View();
        }

        public async  Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("SignIn", "Home");
        }
        public IActionResult AccessDenied()
        {

            return View();
        }



        public async Task<IActionResult> ForgotPassword()
        {
            return View();
        }



        [HttpPost]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordPageVm pageVm)
        {
            AppUser user = await _userManager.Users.FirstOrDefaultAsync(u => u.Email == pageVm.ForgotPasswordPureVm.Email);

            
           
             
            if (user != null)
            {
                string userToken = await _userManager.GeneratePasswordResetTokenAsync(user);

                string passwordTokenLink = Url.Action("ResetPassword", "Home", new { userId = user.Id, token = userToken }, HttpContext.Request.Scheme);

                MailService.Send(pageVm.ForgotPasswordPureVm.Email, subject: "Þifre yenile", body: $"Týklayarak þifrenizi sýfýrlayýn -> {passwordTokenLink}");

                TempData["message"] = "Þifre sýfýrlama e-postasý gönderildi. Lütfen e-posta gelen kutunuzu kontrol edin ve þifrenizi sýfýrlamak için talimatlarý izleyin..";

                return RedirectToAction("MailPanel");
            }
            TempData["message"] = "Lütfen Emailinizi doðru yazdýðýnýza emin olun";
            return View(pageVm);
        }


        public IActionResult ResetPassword(string userId, string token)
        {
            TempData["userId"] = userId;
            TempData["token"] = token;
            return View();
        }
        [HttpPost]

        
        public async Task<IActionResult> ResetPassword(ResetPasswordPageVm pageVm)
        {
            string userId = (string)TempData["userId"];

            string token = (string)TempData["token"];

            if (ModelState.IsValid)
            {
                AppUser user = await _userManager.FindByIdAsync(userId.ToString());

                IdentityResult result = await _userManager.ResetPasswordAsync(user, token, pageVm.ResetPasswordPureVm.NewPassword);

                //Þifre sýfýrlama iþlemi baþarýlýysa, kullanýcýya bir bildirim gösterilebilir

                if (result.Succeeded)
                {
                    TempData["message"] = "Þifreniz baþarýlý bir þekilde deðiþtirilmiþtir.";
                    return RedirectToAction("SignIn"); // Þifre sýfýrlama baþarýlýysa giriþ sayfasýna yönlendirilebilir
                }

                foreach (IdentityError error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }

            // ModelState.IsValid false ise, view'e geri dönülür ve hata mesajlarý gösterilir
            return View(pageVm);
        }

       


    }
} 
