using System;
using System.Configuration;
using System.IdentityModel.Tokens.Jwt;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.Notifications;
using Microsoft.Owin.Security.OpenIdConnect;
using Owin;

[assembly: OwinStartup(typeof(ContosoUniversity.Startup))]

namespace ContosoUniversity
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }

        public void ConfigureAuth(IAppBuilder app)
        {
            JwtSecurityTokenHandler.DefaultMapInboundClaims = false;

            app.SetDefaultSignInAsAuthenticationType(CookieAuthenticationDefaults.AuthenticationType);
            app.UseCookieAuthentication(new CookieAuthenticationOptions());

            var instance = ConfigurationManager.AppSettings["AzureAd:Instance"];
            var tenantId = ConfigurationManager.AppSettings["AzureAd:TenantId"];
            var clientId = ConfigurationManager.AppSettings["AzureAd:ClientId"];
            var callbackPath = ConfigurationManager.AppSettings["AzureAd:CallbackPath"];
            var authority = string.Format("{0}{1}/v2.0", instance, tenantId);

            app.UseOpenIdConnectAuthentication(new OpenIdConnectAuthenticationOptions
            {
                ClientId = clientId,
                Authority = authority,
                RedirectUri = callbackPath,
                PostLogoutRedirectUri = "/",
                ResponseType = OpenIdConnectResponseType.IdToken,
                Scope = OpenIdConnectScope.OpenIdProfile,
                TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    NameClaimType = "name"
                },
                Notifications = new OpenIdConnectAuthenticationNotifications
                {
                    AuthenticationFailed = context =>
                    {
                        context.HandleResponse();
                        context.Response.Redirect("/Home/Unauthorized");
                        return Task.FromResult(0);
                    }
                }
            });
        }
    }
}
