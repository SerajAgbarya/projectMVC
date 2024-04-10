using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;

public class OrderAuthorizeAttribute : AuthorizeAttribute, IAsyncAuthorizationFilter
{

    private readonly string _dbContextName;
    public OrderAuthorizeAttribute(string dbContextName)
    {
        _dbContextName = dbContextName;
    }

    public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
    {
        var dbContext = (OnlineShopDbContext)context.HttpContext.RequestServices.GetService(typeof(OnlineShopDbContext));

        if (!context.HttpContext.User.Identity.IsAuthenticated)
        {
            context.Result = new UnauthorizedResult();
            return;
        }

        var userNameClaim = context.HttpContext.User.FindFirst(ClaimTypes.Name);
        var userModel = await dbContext.Users.FirstOrDefaultAsync(u => u.UserName == userNameClaim.Value);
        // Get the orderId from the route data
        if (!context.RouteData.Values.TryGetValue("orderId", out var orderIdObj) || !int.TryParse(orderIdObj?.ToString(), out var orderId))
        {
            context.Result = new BadRequestResult(); // Return 400 Bad Request if orderId is missing or invalid
            return;
        }

        var order = await dbContext.Orders.FirstOrDefaultAsync(o => o.Id == orderId);


        if (order == null || order.UserId != userModel.Id)
        {
            context.Result = new ForbidResult(); // Return 403 Forbidden if the order does not belong to the user
            return;
        }


    }
}
