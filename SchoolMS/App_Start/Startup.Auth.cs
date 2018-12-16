using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.Facebook;
using Microsoft.Owin.Security.Google;
using Owin;
using SchoolMS.Models;

namespace SchoolMS
{
    public partial class Startup
    {
        // For more information on configuring authentication, please visit https://go.microsoft.com/fwlink/?LinkId=301864
        public void ConfigureAuth(IAppBuilder app)
        {
            // Configure the db context, user manager and signin manager to use a single instance per request
            //app.CreatePerOwinContext(ApplicationDbContext.Create);
            //app.CreatePerOwinContext<ApplicationUserManager>(ApplicationUserManager.Create);
            //app.CreatePerOwinContext<ApplicationSignInManager>(ApplicationSignInManager.Create);

            // Enable the application to use a cookie to store information for the signed in user
            // and to use a cookie to temporarily store information about a user logging in with a third party login provider
            // Configure the sign in cookie
            //app.UseCookieAuthentication(new CookieAuthenticationOptions
            //{
            //    AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
            //    LoginPath = new PathString("/Account/Login"),
            //    Provider = new CookieAuthenticationProvider
            //    {
            //        // Enables the application to validate the security stamp when the user logs in.
            //        // This is a security feature which is used when you change a password or add an external login to your account.  
            //        OnValidateIdentity = SecurityStampValidator.OnValidateIdentity<ApplicationUserManager, ApplicationUser>(
            //            validateInterval: TimeSpan.FromMinutes(30),
            //            regenerateIdentity: (manager, user) => user.GenerateUserIdentityAsync(manager))
            //    }
            //});            
            app.UseExternalSignInCookie(DefaultAuthenticationTypes.ExternalCookie);

            // Enables the application to temporarily store user information when they are verifying the second factor in the two-factor authentication process.
            app.UseTwoFactorSignInCookie(DefaultAuthenticationTypes.TwoFactorCookie, TimeSpan.FromMinutes(5));

            // Enables the application to remember the second login verification factor such as phone or email.
            // Once you check this option, your second step of verification during the login process will be remembered on the device where you logged in from.
            // This is similar to the RememberMe option when you log in.
            app.UseTwoFactorRememberBrowserCookie(DefaultAuthenticationTypes.TwoFactorRememberBrowserCookie);

            // Uncomment the following lines to enable logging in with third party login providers
            //app.UseMicrosoftAccountAuthentication(
            //    clientId: "",
            //    clientSecret: "");

            //app.UseTwitterAuthentication(
            //   consumerKey: "",
            //   consumerSecret: "");

            var facebookOptions = new FacebookAuthenticationOptions()
            {
                AppId = "374889686384709",
                AppSecret = "6881ca72ca8ffd2a7c5212111c8da83a",
                Provider = new FacebookAuthenticationProvider()
                {
                    OnAuthenticated = (context) =>
                    {
                        context.Identity.AddClaim(new Claim("FacebookAccessToken", context.AccessToken));
                        return Task.FromResult(0);
                    }
                },
                UserInformationEndpoint = "https://graph.facebook.com/v2.4/me?fields=id,name,email,first_name,last_name,location"
            };
            facebookOptions.Scope.Add("public_profile");
            facebookOptions.Scope.Add("email");
            //facebookOptions.Scope.Add("user_friends");
            app.UseFacebookAuthentication(facebookOptions);

            //article link
            //https://docs.microsoft.com/en-us/aspnet/core/security/authentication/social/google-logins?view=aspnetcore-2.1&tabs=aspnetcore2x
            //app.UseGoogleAuthentication(new GoogleOAuth2AuthenticationOptions()
            //{
            //    ClientId = "768831552649-l1i5jettpju5fk6ru4ivomm98edu710u.apps.googleusercontent.com",
            //    ClientSecret = "4jO58PTRbZrQxrSX2NuUJ23L"
            //});
            //above working in localhost and but on live

            var googleOptions = new GoogleOAuth2AuthenticationOptions()
            {
                ClientId = "768831552649-l1i5jettpju5fk6ru4ivomm98edu710u.apps.googleusercontent.com",
                ClientSecret = "4jO58PTRbZrQxrSX2NuUJ23L",
                Provider = new GoogleOAuth2AuthenticationProvider()
                {
                    OnAuthenticated = (context) =>
                    {
                        context.Identity.AddClaim(new System.Security.Claims.Claim("urn:google:accesstoken", context.AccessToken, ClaimValueTypes.String, "Google"));
                        return Task.FromResult(0);
                    }
                },
            };
            app.UseGoogleAuthentication(googleOptions);
        }
    }
}