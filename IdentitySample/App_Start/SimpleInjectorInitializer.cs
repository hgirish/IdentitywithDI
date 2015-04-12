
using System.Collections.Generic;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using IdentitySample.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.DataProtection;
using Owin;
using SimpleInjector;
using SimpleInjector.Advanced;
using SimpleInjector.Integration.Web.Mvc;

namespace IdentitySample
{
    public static class SimpleInjectorInitializer
    {
        public static Container Initialize(IAppBuilder app)
        {
            var container = GetInitializeContainer(app);

            container.Verify();

            DependencyResolver.SetResolver(
                new SimpleInjectorDependencyResolver(container));

            return container;
        }

        public static Container GetInitializeContainer(IAppBuilder app)
        {
            var container = new Container();
            container.RegisterSingle(app);

            container.RegisterPerWebRequest<ApplicationUserManager>();

            container.RegisterPerWebRequest(
                () => new ApplicationDbContext("Foo"));


            container.RegisterPerWebRequest<IUserStore<ApplicationUser>>(
                () =>
                new UserStore<ApplicationUser>(
                  container.GetInstance<ApplicationDbContext>()));

            container.RegisterInitializer<ApplicationUserManager>(
                manager => InitializeUserManager(manager, app));

            container.RegisterPerWebRequest<SignInManager<ApplicationUser,string>,ApplicationSignInManager>();

            container.RegisterPerWebRequest<IAuthenticationManager>(
                ()=> AdvancedExtensions.IsVerifying(container)
                ? new OwinContext(new Dictionary<string, object>()).Authentication
                : HttpContext.Current.GetOwinContext().Authentication);

            container.RegisterPerWebRequest<ApplicationRoleManager>();

            container.RegisterPerWebRequest<IRoleStore<IdentityRole,string>>(
                ()=> new RoleStore<IdentityRole>(
                    container.GetInstance<ApplicationDbContext>()));

            container.RegisterMvcControllers(Assembly.GetExecutingAssembly());

            return container;

        }

        private static void InitializeUserManager(ApplicationUserManager manager, IAppBuilder app)
        {
            manager.UserValidator = new UserValidator<ApplicationUser>(manager)
            {
                AllowOnlyAlphanumericUserNames = false,
                RequireUniqueEmail = true
            };

            manager.PasswordValidator = new PasswordValidator()
            {
                RequiredLength = 6,
                RequireNonLetterOrDigit = false,
                RequireDigit = true,
                RequireLowercase = true,
                RequireUppercase = true
            };

            var dataProtectionProvider = app.GetDataProtectionProvider();

            if (dataProtectionProvider != null)
            {
                manager.UserTokenProvider = 
                    new DataProtectorTokenProvider<ApplicationUser, string>(
                        dataProtectionProvider.Create("ASP.NET Identity"));
            }
        }
    }
}