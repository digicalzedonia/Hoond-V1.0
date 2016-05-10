using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Web.WebPages.OAuth;
using DotNetOpenAuth.AspNet.Clients;
using Hoond.Models;

using Hoond.App_Start;

namespace Hoond
{
    public static class AuthConfig
    {
        public static void RegisterAuth()
        {
            //OAuthWebSecurity.RegisterMicrosoftClient(
            //    clientId: "",
            //    clientSecret: "");

            //OAuthWebSecurity.RegisterTwitterClient(
            //    consumerKey: "",
            //    consumerSecret: "");

            OAuthWebSecurity.RegisterFacebookClient(
                appId: "443001282566998",
                appSecret: "d3e4df6354b39a347f390335e59d5cc4");

            OAuthWebSecurity.RegisterClient(new GooglePlusClient("869383521031-blv4s4jkr6n0v2s92vdoae96kv0bi53a.apps.googleusercontent.com", "12MQ-dky7zdn_ikLCeooagKg"), "Google", null);
        }
    }
}
