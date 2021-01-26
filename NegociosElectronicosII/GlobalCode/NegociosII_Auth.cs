using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace NegociosElectronicosII.GlobalCode
{
    public class NegociosII_Auth : AuthorizeAttribute
    {
        private String MODE = String.Empty;

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            if (httpContext == null) throw new ArgumentNullException("httpContext");

            if (Settings.LoggedUser == null)return false;
            
            if (this.Roles != Settings.LoggedUser.RolId.ToString()) return false;

            return true;
        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
                filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary
                                   {
                                       { "action", "Index" },
                                       { "controller", "Login" }
                                   });
            
              
        }
    }
}