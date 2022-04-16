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
using CAPTeam14.Middleware;
using Microsoft.Owin.Security.VanLang;

namespace CAPTeam14.Controllers
{
   
    public class AccountController : Controller
    {
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;
        CP24Team14Entities model = new CP24Team14Entities();

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
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "None")]
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            if (AuthenticationManager.User.Identity.IsAuthenticated)
            {
                Session.Abandon();
                Request.Cookies.Clear();
                AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
                return RedirectToAction("Login");
            }

            // xóa hết những Session, Cookies và đăng xuất nếu đã đăng nhập từ trước

            Session.Abandon();
            Request.Cookies.Clear();
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
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
                Session.Abandon();
                Request.Cookies.Clear();
                AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
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

                    return RedirectToAction("Index1", "Home");
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
            // Vào trang đăng kí bên thứ ba và xóa bỏ, loại hết những Session, Cookies đang tồn tại
            Session.Abandon();
            Request.Cookies.Clear();
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);

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
        public async Task<ActionResult> ExternalLoginCallback(string returnUrl, string email)
        {
           
            var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync2();
          

            // Sign in the user with this external login provider if the user already has a login
            var result = await SignInManager.ExternalSignInAsync2(loginInfo, UserManager);
            ViewBag.LoginProvider = loginInfo.Login.LoginProvider;
            var user = model.AspNetUsers.FirstOrDefault(u => u.Email.Equals(loginInfo.Email));
            var giangVien = model.nguoiDungs.FirstOrDefault(u => u.AspNetUser.Email.Equals(loginInfo.Email));
           
            switch (result)
            {
                case SignInStatus.Success:
                    // nếu biến giangVien không có null, thì khởi tạo các Session
                    if (giangVien != null)
                    {
                        Session["user-id"] = User.Identity.GetUserId();
                        Session["hoten"] = giangVien.tenGV;
                        Session["role"] = giangVien.role;
                        Session["id"] = giangVien.ID_dsGV;
                        Session["id1"] = giangVien.ID;
                        if ((int)Session["role"] == 1)
                        {
                            Session["vaitro"] = "Admin";
                        }
                        else if ((int)Session["role"] == 2) 
                        {
                            Session["vaitro"] = "Ban chủ nhiệm khoa";
                        }
                        else if ((int)Session["role"] == 3)
                        {
                            Session["vaitro"] = "Bộ môn";
                        }
                        else if ((int)Session["role"] == 4)
                        {
                            Session["vaitro"] = "Giảng viên";
                        }
                        // nếu session role là null ~ chưa có kích hoạt
                        if ((Session["role"] == null))
                        {
                            //loại bỏ hết những hoạt động của người dùng hiện tại
                            Session.Abandon();
                            Request.Cookies.Clear();
                            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);

                            TempData["kichhoat"] = 1;
                            return View("Login");
                        }
                        // nếu session role là 0 ~ trạng thái đã bị admin chuyển thành chưa kích hoạt
                        if ((int)Session["role"] == 0)
                        {
                            // loại bỏ hết những hoạt động của người dùng hiện tại
                            Session.Abandon();
                            Request.Cookies.Clear();
                            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);

                            TempData["kichhoat"] = 1;

                            return View("Login");
                        }
                        TempData["dangnhap"] = 1;
                        return RedirectToAction("Index1", "Home");
                    }
                    else 
                    {
                        return RedirectToAction("Create");
                    }




                    


                
                default:
                    // If the user does not have an account, then prompt the user to create an account
                    ViewBag.ReturnUrl = returnUrl;
                    // Khi trường thông tin email chưa tồn tại, chuyển người dùng đến trang đăng kí, và lưu thông tin vào trong bảng AspNetUser

                    //Update 02/01/2022
                    // tự động đăng kí và lưu thông tin email. Người dùng không cần phải qua bước này

                    return View("ExternalLoginConfirmation", new AspNetUser { Email = loginInfo.Email });
            }
        }

        //
        // POST: /Account/ExternalLoginConfirmation

        //Update 02/01/2022
        // tự động đăng kí và lưu thông tin email. Người dùng không cần phải qua bước này
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ExternalLoginConfirmation(AspNetUser model)
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index1", "Home");
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
                        
                        // Nếu lưu thành công, thì chuyển người dùng tới Action Create - Tạo tài khoản
                        return RedirectToAction("Create");
                    }
                }
                AddErrors(result);
               
            }




            return View(model);
        }


        //Đăng kí
        // khởi tạo view
        [HttpGet]
        public ActionResult Create() 
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create( nguoiDung acc, danhsachGV gv)
        {
            // khởi tạo hàm string lấy ID của người dùng hiện tại
            string checkID = User.Identity.GetUserId();
           
            xacThuc(acc);
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
                    // kiểm tra nếu mã giảng viên có đúng với dữ liệu mã giảng viên thực tế
                    // nếu đúng thì lưu thông tin profile
                    var gv1 = model.danhsachGVs.FirstOrDefault(x => x.maGV == taikhoan.maGV);
                    if (gv1 != null)
                    {
                        
                        taikhoan.ID_dsGV = gv1.ID;
                    }
                  

                    model.nguoiDungs.Add(taikhoan);
                    model.SaveChanges();
                    TempData["ThongBao1"] = 1;

                    // sau khi xác nhận thông tin thành công. Trước khi quay về trang chủ phải xóa hết mọi hoạt động
                    Session.Abandon();
                    Request.Cookies.Clear();
                    AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
                    //


                    return RedirectToAction("Login");
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
            return View(acc);
            
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

        

        //Validate dữ liệu
        private void xacThuc(nguoiDung acc)
        {
            var code = model.nguoiDungs.FirstOrDefault(d => d.maGV == acc.maGV);
            var gv1 = model.danhsachGVs.FirstOrDefault(x => x.maGV == acc.maGV);
            //Test case bỏ trống họ tên
            if (acc.tenGV == null)
            {
                ModelState.AddModelError("tenGV", "Vui lòng nhập họ tên");
            }
            else
            {
                // Test case nhập khoảng trắng
                if (acc.tenGV.Trim() == "")
                {
                    ModelState.AddModelError("tenGV", "Không được nhập khoảng trắng");
                }
                else
                {
                    //Test Case nhập quá 30 kí tự
                    if (acc.tenGV.Length > 30)
                    {
                        ModelState.AddModelError("tenGV", "Họ tên không vượt quá 30 kí tự");
                    }

                    else
                    {
                        //Test case kiểm tra kí tự đặc biệt
                        if (Kytudacbiet(acc.tenGV.Trim()) == true)
                        {
                            ModelState.AddModelError("tenGV", "Họ tên không được có ký tự đặc biệt");
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
                    
                        //Test case kiểm tra kí tự đặc biệt
                        if (Kytudacbiet(acc.maGV.Trim()) == true)
                        {
                            ModelState.AddModelError("maGV", "Mã Giảng viên không được có ký tự đặc biệt");
                        }
                    
                    if (code != null)
                    {
                        ModelState.AddModelError("maGV", "Mã giảng viên đã tồn tại");
                    }
                    if (gv1 == null)
                    {

                        ModelState.AddModelError("maGV", "Mã giảng viên không tồn tại trên dữ liệu. Vui lòng liên hệ với người quản lý");
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
                    if (acc.sdt.ToString().Length < 9)
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
            Session.Abandon();
            Request.Cookies.Clear();
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            
           
            
             
            return RedirectToAction("Login", "Account");
        }

        public ActionResult Logout()
        {
            LogOff();
            return RedirectToAction("Login", "Account");
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