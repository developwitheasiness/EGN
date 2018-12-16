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
using SchoolMS.Common;
using SchoolMS.Models;
using SchoolMS.Models.DB;
using SchoolSM.Models;

namespace SchoolMS.Controllers
{
    public class AccountController : Controller
    {

        SchoolMSEntities _db = new SchoolMSEntities();

        //
        // GET: /Account/Login
        [AllowAnonymous]
        [Route("admin")]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        //
        // POST: /Account/Login
        //[HttpPost]
        //[AllowAnonymous]
        //[ValidateAntiForgeryToken]
        //public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return View(model);
        //    }

        //    // This doesn't count login failures towards account lockout
        //    // To enable password failures to trigger account lockout, change to shouldLockout: true
        //    var result = await SignInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, shouldLockout: false);
        //    switch (result)
        //    {
        //        case SignInStatus.Success:
        //            return RedirectToLocal(returnUrl);
        //        case SignInStatus.LockedOut:
        //            return View("Lockout");
        //        case SignInStatus.RequiresVerification:
        //            return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = model.RememberMe });
        //        case SignInStatus.Failure:
        //        default:
        //            ModelState.AddModelError("", "Invalid login attempt.");
        //            return View(model);
        //    }
        //}

        [HttpPost]
        [Route("stafflogin")]
        public JsonResult Login(Login LoginModel)
        {
            JsonResponse<string> Response = new JsonResponse<string>();
            try
            {
                var EncryptPassword = Common.Common.Encrypt(LoginModel.Password);

                var UserDetail = _db.Users.Where(u => u.UserName == LoginModel.UserName && u.Password == EncryptPassword).FirstOrDefault();

                //var UserDetail = _db.Users.Where(u => u.LoginId == LoginModel.UserName).FirstOrDefault();

                if (UserDetail == null)
                {
                    Response.IsError = true;
                    Response.Message = "User not found";
                    return Json(Response, JsonRequestBehavior.AllowGet);
                }

                var DecryptPassword = Common.Common.Decrypt(UserDetail.Password);

                if (UserDetail != null)
                {
                    ApplicationSession.Current.IsLoggedIn = true;
                    ApplicationSession.Current.SessionID = System.Web.HttpContext.Current.Session.SessionID;
                    ApplicationSession.Current.UserID = UserDetail.Id;

                    var LoginUserRoles = _db.UserInRoles.Where(ur => ur.UserId == UserDetail.Id).ToList();

                    if (LoginUserRoles.Count() > 0)
                    {
                        foreach (var item in LoginUserRoles)
                        {
                            RoleIds role = new RoleIds();
                            role.id = item.Id;
                            role.name = item.UserRole != null ? item.UserRole.Role : "";

                            if (UserRoles.Admin == (UserRoles)item.Id)
                            {
                                ApplicationSession.Current.IsSuperAdmin = true;
                            }

                            ApplicationSession.Current.RoleIDs.Add(role);
                        }
                    }
                    else
                    {
                        Response.IsError = true;
                        Response.Message = "User not found";
                        return Json(Response, JsonRequestBehavior.AllowGet);
                    }

                    //if (Url.IsLocalUrl(returnUrl) && returnUrl.Length > 1 && returnUrl.StartsWith("/")
                    //            && !returnUrl.StartsWith("//") && !returnUrl.StartsWith("/\\"))
                    //{
                    //    return Json(, JsonRequestBehavior.AllowGet);
                    //}

                    Response.IsError = false;
                    return Json(Response, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    Response.IsError = true;
                    Response.Message = "User id or password is wrong";
                    return Json(Response, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                Response.IsError = true;
                Response.Message = ex.Message != null ? ex.Message : ex.InnerException.Message;
                return Json(Response, JsonRequestBehavior.AllowGet);
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
        // GET: /Account/ForgotPassword
        [AllowAnonymous]
        public ActionResult ForgotPassword()
        {
            return View();
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
        // POST: /Account/ExternalLogin
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ExternalLogin(string provider, string returnUrl, string LoginType)
        {
            // Request a redirect to the external login provider
            return new ChallengeResult(provider, Url.Action("ExternalLoginCallback", "Account", new { ReturnUrl = returnUrl, LoginType = LoginType }));
        }

        //
        // GET: /Account/ExternalLoginCallback
        [AllowAnonymous]
        public async Task<ActionResult> ExternalLoginCallback(string returnUrl, string LoginType)
        {
            var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync();
            if (loginInfo == null)
            {
                return RedirectToAction("Student");
            }

            // Sign in the user with this external login provider if the user already has a login

            SignInStatus status = SignInStatus.Failure;
            Student stud = new Student();
            Teacher teach = new Teacher();

            //if (loginInfo.Login.LoginProvider == "Facebook")
            //{
            //    var identity = AuthenticationManager.GetExternalIdentity(DefaultAuthenticationTypes.ExternalCookie);
            //    var access_token = identity.FindFirstValue("FacebookAccessToken");
            //    var fb = new FacebookClient(access_token);
            //    dynamic myInfo = fb.Get("/me?fields=email,first_name,last_name"); // specify the email field
            //    stud.Email = myInfo.email;
            //    stud.Name = myInfo.first_name;
            //    stud.Name += myInfo.last_name;
            //}

            if (loginInfo.Login.LoginProvider == "Google")
            {
                var identity = AuthenticationManager.GetExternalIdentity(DefaultAuthenticationTypes.ExternalCookie);
                var access_token = identity.FindFirstValue("urn:google:accesstoken");

                if (LoginType == "student")
                {
                    stud.Email = identity.FindFirstValue("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress");
                    stud.Name = identity.FindFirstValue("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/givenname");
                    stud.Name += !string.IsNullOrEmpty(stud.Name) ? " " + identity.FindFirstValue("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/surname") : identity.FindFirstValue("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/surname");
                }

                else
                {
                    teach.Email = identity.FindFirstValue("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress");
                    teach.Name = identity.FindFirstValue("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/givenname");
                    teach.Name += !string.IsNullOrEmpty(stud.Name) ? " " + identity.FindFirstValue("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/surname") : identity.FindFirstValue("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/surname");
                }
                
            }

            if (LoginType == "student")
            {
                using (var _db = new SchoolMSEntities())
                {
                    stud = _db.Students.Where(s => s.Email == stud.Email).FirstOrDefault();
                }
                if (stud == null)
                {
                    using (var _db = new SchoolMSEntities())
                    {
                        stud = new Student();
                        stud.Id = Guid.NewGuid();
                        stud.Name = loginInfo.DefaultUserName;
                        stud.Email = loginInfo.Email;
                        stud.CreateDate = DateTime.Now;
                        stud.IsDelete = false;
                        stud.Isblock = false;
                        status = SignInStatus.Success;

                        _db.Students.Add(stud);
                        _db.SaveChanges();
                    }
                }
                else
                {
                    if ((bool)stud.Isblock)
                    {
                        status = SignInStatus.RequiresVerification;
                    }
                    else
                    {
                        status = SignInStatus.Success;
                    }
                }
            }
            else
            {
                using (var _db = new SchoolMSEntities())
                {
                    teach = _db.Teachers.Where(s => s.Email == teach.Email).FirstOrDefault();
                }
                if (teach == null)
                {
                    using (var _db = new SchoolMSEntities())
                    {
                        teach = new Teacher();
                        teach.Id = Guid.NewGuid();
                        teach.Name = loginInfo.DefaultUserName;
                        teach.Email = loginInfo.Email;
                        teach.CreateDate = DateTime.Now;
                        teach.IsDelete = false;
                        teach.IsBlock = false;
                        status = SignInStatus.Success;

                        _db.Teachers.Add(teach);
                        _db.SaveChanges();
                    }
                }
                else
                {
                    if ((bool)teach.IsBlock)
                    {
                        status = SignInStatus.RequiresVerification;
                    }
                    else
                    {
                        status = SignInStatus.Success;
                    }
                }
            }
            

            

            //var result = await SignInManager.ExternalSignInAsync(loginInfo, isPersistent: false);
            switch (status)
            {
                case SignInStatus.Success:
                    ApplicationSession.Current.IsLoggedIn = true;
                    ApplicationSession.Current.UserName = !string.IsNullOrEmpty(loginInfo.DefaultUserName) ? loginInfo.DefaultUserName : loginInfo.Email;
                    //return RedirectToAction("Profile", "Settings");
                    if (LoginType == "student")
                    {
                        ApplicationSession.Current.LoginUser = "Student";
                        ApplicationSession.Current.UserID = Convert.ToString(stud.Id);
                        return RedirectToAction("StudentProfile", "Student");
                    }
                    else
                    {
                        ApplicationSession.Current.LoginUser = "Teacher";
                        ApplicationSession.Current.UserID = teach.Id.ToString();
                        return RedirectToAction("TeacherProfile", "Teacher");
                    }
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.RequiresVerification:
                    ApplicationSession.Current.IsLoggedIn = false;
                    ApplicationSession.Current.UserName = string.Empty;
                    ApplicationSession.Current.UserID = string.Empty;
                    return RedirectToAction("UnauthrizedAccess");
                    //return Response.Redirect(String.Format("~/Error/{0}", "UnauthrozedAccess"));
                case SignInStatus.Failure:
                default:
                    // If the user does not have an account, then prompt the user to create an account
                    ApplicationSession.Current.IsLoggedIn = false;
                    ApplicationSession.Current.UserName = string.Empty;
                    ApplicationSession.Current.UserID = string.Empty;
                    ViewBag.ReturnUrl = returnUrl;
                    ViewBag.LoginProvider = loginInfo.Login.LoginProvider;
                    //return View("ExternalLoginConfirmation", new ExternalLoginConfirmationViewModel { Email = loginInfo.Email });
                    return View("Student");
            }
        }

        //
        // POST: /Account/LogOff
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            ApplicationSession.Current.UserID = string.Empty;
            ApplicationSession.Current.IsLoggedIn = false;
            return RedirectToAction("Dashboard", "Home");
        }

        //
        // GET: /Account/ExternalLoginFailure
        [AllowAnonymous]
        public ActionResult ExternalLoginFailure()
        {
            return View();
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