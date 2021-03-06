﻿using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OpenIdConnect;
using Owin;
using System.IdentityModel.Tokens.Jwt;

namespace SsoTestFramework472
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
            app.SetDefaultSignInAsAuthenticationType(CookieAuthenticationDefaults.AuthenticationType);

            app.UseKentorOwinCookieSaver();
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = CookieAuthenticationDefaults.AuthenticationType,
            });
            app.UseOpenIdConnectAuthentication(new OpenIdConnectAuthenticationOptions
            {
                Authority = "http://localhost:6102", //ID Server
                ClientId = "MenuManagement_Web",
                ResponseType = "id_token code",
                SignInAsAuthenticationType = "Cookies",
                RedirectUri = "http://localhost:44309/signin-oidc", //URL of website
                Scope = "email openid profile role phone address MenuManagement offline_access",
                RequireHttpsMetadata = false,
            });
        }
    }
}