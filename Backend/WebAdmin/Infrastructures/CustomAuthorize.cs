using System;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using WebAdmin.Models;

public class CustomAuthorize: ActionFilterAttribute
{
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        string token = context.HttpContext.Request.Cookies["token"];
        if(!string.IsNullOrEmpty(token))
        {
            var client = RequestHelper.GetHttpClient();
            client.DefaultRequestHeaders.Add("Authorization", token);
            try{
                var result = client.GetAsync("/token").Result;
            }
            catch(Exception ex)
            {
                System.Console.WriteLine(ex.StackTrace);
                context.HttpContext.Response.Cookies.Delete("token");
                context.Result = new RedirectToRouteResult(GetLoginRouting());
            }
        }
        else{
            context.Result = new RedirectToRouteResult(GetLoginRouting());
        }
        base.OnActionExecuting(context);
    }

    private RouteValueDictionary GetLoginRouting()
    {
        return  new RouteValueDictionary {{ "Controller", "Login" },
                                      { "Action", "" }};
    }

}