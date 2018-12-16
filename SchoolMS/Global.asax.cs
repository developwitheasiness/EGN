using System;
using System.Globalization;
using System.Net;
using System.Security.Cryptography;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace SchoolMS
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            CultureInfo newCulture = (CultureInfo)System.Threading.Thread.CurrentThread.CurrentCulture.Clone();
            newCulture.DateTimeFormat.ShortDatePattern = "dd-MM-yyyy";
            newCulture.DateTimeFormat.DateSeparator = "-";
            Thread.CurrentThread.CurrentCulture = newCulture;

            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        //This method checks if we have an AJAX request or not
        private bool IsAjaxRequest()
        {
            //The easy way
            bool isAjaxRequest = (Request["X-Requested-With"] == "XMLHttpRequest")
            || ((Request.Headers != null)
            && (Request.Headers["X-Requested-With"] == "XMLHttpRequest"));

            //If we are not sure that we have an AJAX request or that we have to return JSON 
            //we fall back to Reflection
            if (!isAjaxRequest)
            {
                try
                {
                    //The controller and action
                    string controllerName = Request.RequestContext.
                                            RouteData.Values["controller"].ToString();
                    string actionName = Request.RequestContext.
                                        RouteData.Values["action"].ToString();

                    //We create a controller instance
                    DefaultControllerFactory controllerFactory = new DefaultControllerFactory();
                    Controller controller = controllerFactory.CreateController(
                    Request.RequestContext, controllerName) as Controller;

                    //We get the controller actions
                    ReflectedControllerDescriptor controllerDescriptor =
                    new ReflectedControllerDescriptor(controller.GetType());
                    ActionDescriptor[] controllerActions =
                    controllerDescriptor.GetCanonicalActions();

                    //We search for our action
                    foreach (ReflectedActionDescriptor actionDescriptor in controllerActions)
                    {
                        if (actionDescriptor.ActionName.ToUpper().Equals(actionName.ToUpper()))
                        {
                            //If the action returns JsonResult then we have an AJAX request
                            if (actionDescriptor.MethodInfo.ReturnType
                            .Equals(typeof(JsonResult)))
                                return true;
                        }
                    }
                }
                catch
                {

                }
            }

            return isAjaxRequest;
        }

        //snip

        protected void Application_Error(object sender, EventArgs e)
        {
            var application = (HttpApplication)sender;

            Exception exception = Server.GetLastError();
            Response.Clear();

            //log.Error("Application_Error", exception);


            if ((exception as CryptographicException) != null)
            {
                //FederatedAuthentication.SessionAuthenticationModule.SignOut();
                Response.Redirect(Request.RawUrl);
            }
            else if ((exception as HttpException) != null)
            {
                HttpException httpException = exception as HttpException;

                if (IsAjaxRequest())
                {
                    Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    Response.StatusDescription = httpException.Message;
                }
                else
                {
                    string action;

                    switch (httpException.GetHttpCode())
                    {
                        case 404:
                            // page not found
                            action = "NotFound";
                            Response.Redirect(String.Format("~/Error/{0}", action));
                            break;
                        case 500:
                            // server error
                            action = "Error404";
                            Response.Redirect(String.Format("~/Error/{0}/?code={0}&message={1}", action, (int)Response.StatusCode, (exception.Message != null ? exception.Message : exception.InnerException.Message) + " Please contact Administrator."));
                            break;
                        default:
                            action = "General";
                            Response.Redirect(String.Format("~/Error/{0}/?code={0}&message={1}", action, (int)Response.StatusCode, (exception.Message != null ? exception.Message : exception.InnerException.Message) + " Please contact Administrator."));
                            break;
                    }
                }

                Server.ClearError();
            }
            else
            {
                Response.Redirect(String.Format("~/Error/General/?code={0}&message={1}", (int)Response.StatusCode, (exception.Message != null ? exception.Message : exception.InnerException.Message) + " Please contact Administrator."));
            }
        }

        //protected void Application_Error()
        //{
        //    var error = Server.GetLastError();
        //    var code = (error is HttpException) ? (error as HttpException).GetHttpCode() : 500;

        //    if (code != 404)
        //    {
        //        //// Generate email with error details and send to administrator
        //        //// I'm using RazorMail which can be downloaded from the Nuget Gallery
        //        //// I also have an extension method on type Exception that creates a string representation
        //        //var email = new RazorMailMessage("Website Error");
        //        //email.To.Add("errors@wduffy.co.uk");
        //        //email.Templates.Add(error.GetAsHtml(new HttpRequestWrapper(Request)));
        //        //Kernel.Get<IRazorMailSender>().Send(email);
        //    }

        //    Response.Clear();
        //    Server.ClearError();

        //    string path = Request.Path;
        //    Context.RewritePath(string.Format("~/Error/ErrorPage/{0}", code), false);
        //    IHttpHandler httpHandler = new MvcHttpHandler();
        //    httpHandler.ProcessRequest(Context);
        //    //Above line throws an error
        //    //System.InvalidOperationException: ''HttpContext.SetSessionStateBehavior' can only be invoked before 'HttpApplication.AcquireRequestState' event is raised.'
        //    Context.RewritePath(path, false);
        //}

        //protected void Application_Error()
        //{
        //    var exception = Server.GetLastError();
        //    System.Diagnostics.Debug.WriteLine(exception);
        //    //It will set true when error coming from http
        //    if (exception is HttpException)
        //    {
        //        var httpException = (HttpException)exception;
        //        Response.StatusCode = httpException.GetHttpCode();

        //        Response.RedirectToRoute("/Error/Error");
        //        //Response.Redirect("Error");
        //    }
        //    else
        //    {
        //        //either it will call from anywhere except http
        //        HttpContext.Current.Response.RedirectToRoute("/Error/ErrorPage", new { id = 404 });
        //    }
        //}
    }
}
