using System;
using System.Data.Entity.Validation;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Owin;
using PetCare_system.Models;

namespace PetCare_system
{
    public partial class Startup
    {
        public void ConfigureAuth(IAppBuilder app)
        {
            // Set up database context and user manager
            app.CreatePerOwinContext(ApplicationDbContext.Create);
            app.CreatePerOwinContext<ApplicationUserManager>(ApplicationUserManager.Create);
            app.CreatePerOwinContext<ApplicationSignInManager>(ApplicationSignInManager.Create);

            // Configure cookie authentication
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                LoginPath = new PathString("/Account/Login"),
                Provider = new CookieAuthenticationProvider
                {
                    OnValidateIdentity = SecurityStampValidator.OnValidateIdentity<ApplicationUserManager, ApplicationUser>(
                        validateInterval: TimeSpan.FromMinutes(30),
                        regenerateIdentity: (manager, user) => user.GenerateUserIdentityAsync(manager))
                }
            });

            app.UseExternalSignInCookie(DefaultAuthenticationTypes.ExternalCookie);
            app.UseTwoFactorSignInCookie(DefaultAuthenticationTypes.TwoFactorCookie, TimeSpan.FromMinutes(5));
            app.UseTwoFactorRememberBrowserCookie(DefaultAuthenticationTypes.TwoFactorRememberBrowserCookie);

            // Ensure admin account is created after authentication is set up
            CreateAdminAccount().Wait();
        }

        private async Task CreateAdminAccount()
        {
            using (var context = new ApplicationDbContext())
            {
                var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
                var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));

                // Ensure Admin role exists
                if (!await roleManager.RoleExistsAsync("Admin"))
                {
                    await roleManager.CreateAsync(new IdentityRole("Admin"));
                }

                const string adminEmail = "admin@example.com";
                const string adminPassword = "AdminPassword123!";

                // Check if the admin user already exists
                var existingAdmin = await userManager.FindByEmailAsync(adminEmail);
                if (existingAdmin == null)
                {
                    var adminUser = new ApplicationUser
                    {
                        UserName = adminEmail,
                        Email = adminEmail,
                        EmailConfirmed = true
                    };

                    try
                    {
                        var result = await userManager.CreateAsync(adminUser, adminPassword);
                        if (result.Succeeded)
                        {
                            await userManager.AddToRoleAsync(adminUser.Id, "Admin");
                            System.Diagnostics.Debug.WriteLine("Admin account created successfully.");
                        }
                        else
                        {
                            foreach (var error in result.Errors)
                            {
                                System.Diagnostics.Debug.WriteLine($"Admin creation failed: {error}");
                            }
                        }
                    }
                    catch (DbEntityValidationException ex)
                    {
                        foreach (var validationErrors in ex.EntityValidationErrors)
                        {
                            foreach (var validationError in validationErrors.ValidationErrors)
                            {
                                System.Diagnostics.Debug.WriteLine($"Property: {validationError.PropertyName} Error: {validationError.ErrorMessage}");
                            }
                        }
                    }
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine("Admin account already exists.");
                }
            }
        }
    }
}
