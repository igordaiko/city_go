using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Web.Mvc;
using WebMatrix.WebData;
using System.Threading;
using City_Go.Models;
using System.Web.Security;

namespace City_Go.Filters
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
    public sealed class InitializeSimpleMemberShipAttribute : ActionFilterAttribute
    {
        public static SimpleMemberShipInitializer _initializer;
        public static object _initializerLock = new object();
        public static bool _IsInitialized;

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            LazyInitializer.EnsureInitialized(ref _initializer, ref _IsInitialized, ref _initializerLock);
        }

        public class SimpleMemberShipInitializer
        {
            public SimpleMemberShipInitializer()
            {
                Database.SetInitializer<UsersContext>(null);

                try
                {
                    using (var context = new UsersContext())
                    {
                        if(!context.Database.Exists())
                        {
                            ((IObjectContextAdapter)context).ObjectContext.CreateDatabase();
                        }
                    }

                    WebSecurity.InitializeDatabaseConnection("DefaultConnection", "UserProfile", "ID", "Login", autoCreateTables: true);
                    SimpleRoleProvider roles = (SimpleRoleProvider)Roles.Provider;
                    SimpleMembershipProvider membership = (SimpleMembershipProvider)Membership.Provider;

                    if (!roles.RoleExists("moderator"))
                        roles.CreateRole("moderator");
                    if (!roles.RoleExists("admin"))
                        roles.CreateRole("admin");
                    

                }
                catch (Exception)
                {

                    throw new InvalidOperationException();
                }
            }
        }
    }
}