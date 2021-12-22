using System;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using CAPTeam14.Models;
using System.Text.RegularExpressions;

namespace CAPTeam14.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;
        CapTeam14Entities model = new CapTeam14Entities();

        public AccountController()
        {
        }

        public AccountController(ApplicationUserManager userManager, ApplicationSignInManager signInManager )
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set 
            { 
                _signInManager = value; 
            }
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        //
        // GET: /Account/Login
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        //
        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // This doesn't count login failures towards account lockout
            // To enable password failures to trigger account lockout, change to shouldLockout: true
            var result = await SignInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, shouldLockout: false);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(returnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.RequiresVerification:
                    return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = model.RememberMe });
                case SignInStatus.Failure:
                default:
                    ModelState.AddModelError("", "Invalid login attempt.");
                    return View(model);
            }
        }

        //
        // GET: /Account/VerifyCode
        [AllowAnonymous]
        public async Task<ActionResult> VerifyCode(string provider, string returnUrl, bool rememberMe)
        {
            // Require that the user has already logged in via username/password or external login
            if (!await SignInManager.HasBeenVerifiedAsync())
            {
                return View("Error");
            }
            return View(new VerifyCodeViewModel { Provider = provider, ReturnUrl = returnUrl, RememberMe = rememberMe });
        }

        //
        // POST: /Account/VerifyCode
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> VerifyCode(VerifyCodeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // The following code protects for brute force attacks against the two factor codes. 
            // If a user enters incorrect codes for a specified amount of time then the user account 
            // will be locked out for a specified amount of time. 
            // You can configure the account lockout settings in IdentityConfig
            var result = await SignInManager.TwoFactorSignInAsync(model.Provider, model.Code, isPersistent:  model.RememberMe, rememberBrowser: model.RememberBrowser);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(model.ReturnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.Failure:
                default:
                    ModelState.AddModelError("", "Invalid code.");
                    return View(model);
            }
        }

        //
        // GET: /Account/Register
        [AllowAnonymous]
        public ActionResult Register()
        {
            return View();
        }

        //
        // POST: /Account/Register
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
                var result = await UserManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    await SignInManager.SignInAsync(user, isPersistent:false, rememberBrowser:false);
                    
                    // For more information on how to enable account confirmation and password reset please visit https://go.microsoft.com/fwlink/?LinkID=320771
                    // Send an email with this link
                    // string code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
                    // var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
                    // await UserManager.SendEmailAsync(user.Id, "Confirm your account", "Please confirm your account by clicking <a href=\"" + callbackUrl + "\">here</a>");

                    return RedirectToAction("Index", "Home");
                }
                AddErrors(result);
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Account/ConfirmEmail
        [AllowAnonymous]
        public async Task<ActionResult> ConfirmEmail(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return View("Error");
            }
            var result = await UserManager.ConfirmEmailAsync(userId, code);
            return View(result.Succeeded ? "ConfirmEmail" : "Error");
        }

        //
        // GET: /Account/ForgotPassword
        [AllowAnonymous]
        public ActionResult ForgotPassword()
        {
            return View();
        }

        //
        // POST: /Account/ForgotPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await UserManager.FindByNameAsync(model.Email);
                if (user == null || !(await UserManager.IsEmailConfirmedAsync(user.Id)))
                {
                    // Don't reveal that the user does not exist or is not confirmed
                    return View("ForgotPasswordConfirmation");
                }

                // For more information on how to enable account confirmation and password reset please visit https://go.microsoft.com/fwlink/?LinkID=320771
                // Send an email with this link
                // string code = await UserManager.GeneratePasswordResetTokenAsync(user.Id);
                // var callbackUrl = Url.Action("ResetPassword", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);		
                // await UserManager.SendEmailAsync(user.Id, "Reset Password", "Please reset your password by clicking <a href=\"" + callbackUrl + "\">here</a>");
                // return RedirectToAction("ForgotPasswordConfirmation", "Account");
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Account/ForgotPasswordConfirmation
        [AllowAnonymous]
        public ActionResult ForgotPasswordConfirmation()
        {
            return View();
        }

        //
        // GET: /Account/ResetPassword
        [AllowAnonymous]
        public ActionResult ResetPassword(string code)
        {
            return code == null ? View("Error") : View();
        }

        //
        // POST: /Account/ResetPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = await UserManager.FindByNameAsync(model.Email);
            if (user == null)
            {
                // Don't reveal that the user does not exist
                return RedirectToAction("ResetPasswordConfirmation", "Account");
            }
            var result = await UserManager.ResetPasswordAsync(user.Id, model.Code, model.Password);
            if (result.Succeeded)
            {
                return RedirectToAction("ResetPasswordConfirmation", "Account");
            }
            AddErrors(result);
            return View();
        }

        //
        // GET: /Account/ResetPasswordConfirmation
        [AllowAnonymous]
        public ActionResult ResetPasswordConfirmation()
        {
            return View();
        }

        //
        // POST: /Account/ExternalLogin
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ExternalLogin(string provider, string returnUrl)
        {
            // Request a redirect to the external login provider
            return new ChallengeResult(provider, Url.Action("ExternalLoginCallback", "Account", new { ReturnUrl = returnUrl }));
        }

        //
        // GET: /Account/SendCode
        [AllowAnonymous]
        public async Task<ActionResult> SendCode(string returnUrl, bool rememberMe)
        {
            var userId = await SignInManager.GetVerifiedUserIdAsync();
            if (userId == null)
            {
                return View("Error");
            }
            var userFactors = await UserManager.GetValidTwoFactorProvidersAsync(userId);
            var factorOptions = userFactors.Select(purpose => new SelectListItem { Text = purpose, Value = purpose }).ToList();
            return View(new SendCodeViewModel { Providers = factorOptions, ReturnUrl = returnUrl, RememberMe = rememberMe });
        }

        //
        // POST: /Account/SendCode
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SendCode(SendCodeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            // Generate the token and send it
            if (!await SignInManager.SendTwoFactorCodeAsync(model.SelectedProvider))
            {
                return View("Error");
            }
            return RedirectToAction("VerifyCode", new { Provider = model.SelectedProvider, ReturnUrl = model.ReturnUrl, RememberMe = model.RememberMe });
        }

        //
        // GET: /Account/ExternalLoginCallback
        [AllowAnonymous]
        public async Task<ActionResult> ExternalLoginCallback(string returnUrl)
        {
            var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync();
            if (loginInfo == null)
            {
                return RedirectToAction("Login");
            }

            // Sign in the user with this external login provider if the user already has a login
            var result = await SignInManager.ExternalSignInAsync(loginInfo, isPersistent: false);
            ViewBag.LoginProvider = loginInfo.Login.LoginProvider;
            var user = model.AspNetUsers.FirstOrDefault(u => u.Email.Equals(loginInfo.Email));
            switch (result)
            {
                case SignInStatus.Success:
                  
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.RequiresVerification:
                    return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = false });
                case SignInStatus.Failure:
                default:
                    // If the user does not have an account, then prompt the user to create an account
                    ViewBag.ReturnUrl = returnUrl;
                    // Khi trường thông tin email chưa tồn tại, chuyển người dùng đến trang đăng kí, và lưu thông tin vào trong bảng AspNetUser
                    return View("ExternalLoginConfirmation", new AspNetUser { Email = loginInfo.Email });
            }
        }

        //
        // POST: /Account/ExternalLoginConfirmation
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ExternalLoginConfirmation(AspNetUser model)
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }

            if (ModelState.IsValid)
            {
                // Get the information about the user from the external login provider
                var info = await AuthenticationManager.GetExternalLoginInfoAsync();
                // Lưu thông tin vô database
                var user = new ApplicationUser
                {
                    UserName = model.Email,
                    Email = model.Email,

                };

                var result = await UserManager.CreateAsync(user);
                if (result.Succeeded)
                {
                    result = await UserManager.AddLoginAsync(user.Id, info.Login);
                    if (result.Succeeded)
                    {
                        await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                        
                        
                        return RedirectToAction("Create");
                    }
                }
                AddErrors(result);
               
            }




            return View(model);
        }
        //Đăng kí
        [HttpGet]
        public ActionResult Create() 
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create( nguoiDung acc)
        {
            string checkID = User.Identity.GetUserId();

            try
            {
                if (ModelState.IsValid)
                {
                    var taikhoan = new nguoiDung();
                    taikhoan.userID = checkID;
                    taikhoan.tenGV = acc.tenGV;
                    taikhoan.maGV = acc.maGV;
                    taikhoan.loaiGV = acc.loaiGV;
                    taikhoan.khoa = "Khoa Công Nghệ Thông Tin";
                    taikhoan.gioiTinh = acc.gioiTinh;
                    taikhoan.role = acc.role;
                    taikhoan.sdt = acc.sdt;

                    model.nguoiDungs.Add(taikhoan);
                    model.SaveChanges();
                }


                else
                {
                    string messages = string.Join("; ", ModelState.Values
                                        .SelectMany(x => x.Errors)
                                        .Select(x => x.ErrorMessage));
                    ModelState.AddModelError("", messages);
                }
            }
            catch (Exception)
            {
                ModelState.AddModelError("", "Không thể thực hiện hành động này, vui lòng kiểm tra lại các trường thông tin");
            }
            return RedirectToAction("Index", "Home");
        }

        //Hàm kiểm tra ký tự đặc biệt
        public static bool Kytudacbiet(string str)
        {
            //khai báo các ký tự đặc biệt
            string kytudacbiet = @"%!@#$%^&*()?/><:'\|}]{[_~`+=-" + "\"";
            //chuyển các ký tự đặc biệt sang dạng chuỗi
            char[] chuoikytudacbiet = kytudacbiet.ToCharArray();
            //kiểm tra trường thông tin người dùng nhập vào có chứa ký tự đặc biệt hay không
            int index = str.IndexOfAny(chuoikytudacbiet);
            // nếu index == -1 thì trả về false => không có ký tự đặc biệt
            if (index == -1)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        //Hàm kiểm tra định dạng Email
        public bool kiemtraEmail(string email)
        {
            // khai báo các regex
            // abc@gmail.com
            string pattern = @"^(?!\.)(""([^""\r\\]|\\[""\r\\])*""|" + @"([-a-z0-9!#$%&'*+/=?^_`{|}~]|(?<!\.)\.)*)(?<!\.)" + @"@[a-z0-9][\w\.-]*[a-z0-9]\.[a-z][a-z\.]*[a-z]$";
            // đưa các regex để chuyển đổi và bỏ qua ký tự viết in hoa hay viết thường
            var regex = new Regex(pattern, RegexOptions.IgnoreCase);
            return regex.IsMatch(email);
        }

        //Validate dữ liệu
        private void xacThuc(nguoiDung acc)
        {
            var code = model.nguoiDungs.FirstOrDefault(d => d.maGV == acc.maGV);
            //Test case bỏ trống họ tên
            if (acc.tenGV == null)
            {
                ModelState.AddModelError("UserName", "Vui lòng nhập họ tên");
            }
            else
            {
                // Test case nhập khoảng trắng
                if (acc.tenGV.Trim() == "")
                {
                    ModelState.AddModelError("UserName", "Không được nhập khoảng trắng");
                }
                else
                {
                    //Test Case nhập quá 30 kí tự
                    if (acc.tenGV.Length > 30)
                    {
                        ModelState.AddModelError("UserName", "Họ tên không vượt quá 30 kí tự");
                    }

                    else
                    {
                        //Test case kiểm tra kí tự đặc biệt
                        if (Kytudacbiet(acc.tenGV.Trim()) == true)
                        {
                            ModelState.AddModelError("UserName", "Họ tên không được có ký tự đặc biệt");
                        }
                    }

                }
            }


            //
            //Test case bỏ trống mã giảng viên
            if (acc.maGV == null)
            {
                ModelState.AddModelError("maGV", "Vui lòng nhập mã giảng viên");
            }
            else
            {
                // Test case nhập khoảng trắng
                if (acc.maGV.Trim() == "")
                {
                    ModelState.AddModelError("maGV", "Không được nhập khoảng trắng");
                }
                else
                {
                    //Test Case nhập quá 10 kí tự
                    if (acc.maGV.Length > 10)
                    {
                        ModelState.AddModelError("maGV", "Mã Giảng viên không vượt quá 10 kí tự");
                    }
                    //Test case nhập ít hơn 10 kí tự
                    if (acc.maGV.Length < 10)
                    {
                        ModelState.AddModelError("maGV", "Mã Giảng viên không được ít hơn 10 kí tự");
                    }
                    else
                    {
                        //Test case kiểm tra kí tự đặc biệt
                        if (Kytudacbiet(acc.maGV.Trim()) == true)
                        {
                            ModelState.AddModelError("maGV", "Mã Giảng viên không được có ký tự đặc biệt");
                        }
                    }
                    if (code != null)
                    {
                        ModelState.AddModelError("maGV", "Mã giảng viên đã tồn tại");
                    }

                }
            }

            ////
            //Test case bỏ trống sdt
            if (acc.sdt == null)
            {
                ModelState.AddModelError("sdt", "Vui lòng nhập số điện thoại");
            }
            else
            {
                // Test case nhập khoảng trắng
                if (acc.sdt.ToString().Trim() == "")
                {
                    ModelState.AddModelError("sdt", "Không được nhập khoảng trắng");
                }
                else
                {
                    //Test Case nhập quá 10 kí tự
                    if (acc.sdt.ToString().Length > 10)
                    {
                        ModelState.AddModelError("sdt", "Số điện thoại không vượt quá 10 kí tự");
                    }
                    //Test case nhập ít hơn 09 kí tự
                    if (acc.sdt.ToString().Length <= 9)
                    {
                        ModelState.AddModelError("sdt", "Số điện thoại không được ít hơn 10 kí tự");
                    }
                    else
                    {
                        //Test case kiểm tra kí tự đặc biệt
                        if (Kytudacbiet(acc.sdt.ToString().Trim()) == true)
                        {
                            ModelState.AddModelError("sdt", "Số điện thoại không được có ký tự đặc biệt");
                        }
                    }
                }
            }
        }
        //
        // POST: /Account/LogOff
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            return RedirectToAction("Index", "Home");
        }

        //
        // GET: /Account/ExternalLoginFailure
        [AllowAnonymous]
        public ActionResult ExternalLoginFailure()
        {
            return View();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_userManager != null)
                {
                    _userManager.Dispose();
                    _userManager = null;
                }

                if (_signInManager != null)
                {
                    _signInManager.Dispose();
                    _signInManager = null;
                }
            }

            base.Dispose(disposing);
        }

        #region Helpers
        // Used for XSRF protection when adding external logins
        private const string XsrfKey = "XsrfId";

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Home");
        }

        internal class ChallengeResult : HttpUnauthorizedResult
        {
            public ChallengeResult(string provider, string redirectUri)
                : this(provider, redirectUri, null)
            {
            }

            public ChallengeResult(string provider, string redirectUri, string userId)
            {
                LoginProvider = provider;
                RedirectUri = redirectUri;
                UserId = userId;
            }

            public string LoginProvider { get; set; }
            public string RedirectUri { get; set; }
            public string UserId { get; set; }

            public override void ExecuteResult(ControllerContext context)
            {
                var properties = new AuthenticationProperties { RedirectUri = RedirectUri };
                if (UserId != null)
                {
                    properties.Dictionary[XsrfKey] = UserId;
                }
                context.HttpContext.GetOwinContext().Authentication.Challenge(properties, LoginProvider);
            }
        }
        #endregion
    }
}