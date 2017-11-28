using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using api.Models;
using api.Utils;

namespace api.Providers
{
    public class TokenProviderMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly JsonSerializerSettings _serializerSettings;

        public TokenProviderMiddleware(
            RequestDelegate next)
        {
            _next = next;
            _serializerSettings = new JsonSerializerSettings
            {
                Formatting = Formatting.Indented
            };
        }

        public Task Invoke(HttpContext context)
        {
            // If the request path doesn't match, skip
            if (!context.Request.Path.Equals("/api/token", StringComparison.Ordinal))
            {
                return _next(context);
            }

            // Request must be POST with Content-Type: application/x-www-form-urlencoded
            if (!context.Request.Method.Equals("POST")
               || !context.Request.HasFormContentType)
            {
                context.Response.StatusCode = 400;
                return context.Response.WriteAsync("Bad request.");
            }


            return GenerateToken(context);
        }

        private async Task GenerateToken(HttpContext context)
        {
            try
            {
                var username = context.Request.Form["username"].ToString();
                var password = context.Request.Form["password"];

                var signInManager = context.RequestServices.GetService<SignInManager<ApplicationUser>>();
                var userManager = context.RequestServices.GetService<UserManager<ApplicationUser>>();

                var result = await signInManager.PasswordSignInAsync(username, password, false, lockoutOnFailure: false);
                if (!result.Succeeded)
                {
                    context.Response.StatusCode = 400;
                    await context.Response.WriteAsync("Invalid username or password.");
                    return;
                }
                var user = await userManager.Users
                    .SingleAsync(i => i.UserName == username);
                if (!user.IsEnabled)
                {
                    context.Response.StatusCode = 400;
                    await context.Response.WriteAsync("Invalid username or password.");
                    return;
                }
                var db = context.RequestServices.GetService<ApiDbContext>();
                var response = GetLoginToken.Execute(user, db);

                // Serialize and return the response
                context.Response.ContentType = "application/json";
                await context.Response.WriteAsync(JsonConvert.SerializeObject(response, _serializerSettings));
            }
            catch (Exception ex)
            {
                //TODO log error
                //Logging.GetLogger("Login").Error("Erorr logging in", ex);
            }
        }

    }

}
