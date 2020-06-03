using System;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using Api.Models.Configuration;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Options;

public class BasicAuthFilter : IAuthorizationFilter
{
    private readonly IOptions<WebApiConfig> _config;

    public BasicAuthFilter(IOptions<WebApiConfig> config)
    {
        _config = config;
    }

    public void OnAuthorization(AuthorizationFilterContext context)
    {
        try
        {
            string authHeader = context.HttpContext.Request.Headers["Authorization"];
            if (authHeader != null)
            {
                var authHeaderValue = AuthenticationHeaderValue.Parse(authHeader);
                if (authHeaderValue.Scheme.Equals(AuthenticationSchemes.Basic.ToString(), StringComparison.OrdinalIgnoreCase))
                {
                    var credentials = Encoding.UTF8
                        .GetString(Convert.FromBase64String(authHeaderValue.Parameter ?? string.Empty))
                        .Split(':', 2);
                    if (credentials.Length == 2)
                    {
                        if (IsAuthorized(context, credentials[0], credentials[1]))
                        {
                            return;
                        }
                    }
                }
            }

            ReturnUnauthorizedResult(context);
        }
        catch (FormatException)
        {
            ReturnUnauthorizedResult(context);
        }
    }

    public bool IsAuthorized(AuthorizationFilterContext context, string username, string password)
    {
        //var userService = context.HttpContext.RequestServices.GetRequiredService<IUserService>();
        return IsValidUser(username, password);
    }

    private bool IsValidUser(string username, string password)
    {
        if (username == _config.Value.ApiUsername && password == _config.Value.ApiPassword)
        {
            return true;
        }

        return false;
    }

    private void ReturnUnauthorizedResult(AuthorizationFilterContext context)
    {
        // Return 401 and a basic authentication challenge (causes browser to show login dialog)
        context.HttpContext.Response.Headers["WWW-Authenticate"] = "Basic realm=\"REALM\"";
        context.Result = new UnauthorizedResult();
    }
}