using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using projectMVC.Models;

public class CustomAuthorizeAttribute : AuthorizeAttribute, IAsyncAuthorizationFilter
{
    private readonly string _role;

    public CustomAuthorizeAttribute(string role)
    {
        _role = role;
    }

    public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
    {
        if (!context.HttpContext.User.Identity.IsAuthenticated)
        {
            context.Result = new UnauthorizedResult();
            return;
        }

        var user = context.HttpContext.User;

        if (!user.HasClaim(c => c.Type == ClaimTypes.Role && c.Value == _role))
        {
            context.Result = new ForbidResult();
            return;
        }
    }

}
