using DevPortal.Identity.Data;
using DevPortal.Model;
using DevPortal.Web.Library;
using DevPortal.Web.Library.Model;
using FluentValidation.AspNetCore;
using jsreport.AspNetCore;
using jsreport.Binary;
using jsreport.Local;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;

namespace DevPortal.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public static void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("DevPortalCors",
                builder =>
                {
                    builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
                });
            });

            services.AddJsReport(new LocalReporting().UseBinary(JsReportBinary.GetBinary()).KillRunningJsReportProcesses().AsUtility().Create());

            //Identity
            services.AddIdentityCore<User>(config =>
            {
                config.Password.RequiredLength = ConstIntegers.MinPasswordLength;
                config.Password.RequireUppercase = false;
                config.Password.RequireNonAlphanumeric = false;
                config.Password.RequireLowercase = true;
                config.User.RequireUniqueEmail = true;
            }).AddErrorDescriber<TurkishIdentityErrorDescriber>()
                .AddDefaultTokenProviders();

            //Authentication
            services.AddAuthentication("Identity.Application").
                AddCookie("Identity.Application", config =>
                {
                    config.Cookie.Name = "DevPortal.Cookie";
                    config.ExpireTimeSpan = TimeSpan.FromMinutes(ConstIntegers.ConfigExpireTimeSpan);
                    config.AccessDeniedPath = new PathString($"/{ControllerNames.Error}/{HttpStatusCode.Forbidden}");
                    config.LoginPath = new PathString($"/{ControllerNames.Account}/{AccountControllerActionNames.Login}");
                    config.LogoutPath = new PathString($"/{ControllerNames.Account}/{AccountControllerActionNames.LogOut}");
                    config.ReturnUrlParameter = "returnUrl";
                    config.Events.OnRedirectToLogin = (context) =>
                    {
                        object controller;
                        context.Request.RouteValues.TryGetValue("controller", out controller);
                        if (IsExternalModuleRequest(controller))
                        {
                            context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                        }
                        else
                        {
                            context.Response.Redirect(context.RedirectUri);
                        }
                        return Task.FromResult(0);
                    };
                });

            //Authorization
            services.AddAuthorization(config =>
            {
                config.AddPolicy($"{Policy.Admin}", policyBuilder =>
                policyBuilder.RequireClaim(ClaimTypes.Role, $"{(int)UserTypes.Yonetici}"));

                config.AddPolicy($"{Policy.AdminDeveloper}", policyBuilder =>
                policyBuilder.RequireClaim(ClaimTypes.Role, new[] { $"{(int)UserTypes.Yonetici}", $"{(int)UserTypes.Gelistirici}" }));
            });

            // Adds the ability to use session variables.
            services.AddSession(options =>
            {
                options.Cookie.HttpOnly = true;
            });
            services.AddMvc(options =>
            {
                options.Filters.Add<GlobalExceptionFilterAttribute>();
                options.Filters.Add(new AuthorizeFilter());
            }).AddFluentValidation();

            services.AddControllersWithViews();
            services.AddMemoryCache();
            services.AddHttpContextAccessor();
            services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();

#if (DEV || LOCAL)
            services.AddRazorPages().AddRazorRuntimeCompilation();
#endif
        }

        static bool IsExternalModuleRequest(object controller)
        {
            return controller.ToString().ToLowerInvariant() == ControllerNames.Nuget
                || controller.ToString().ToLowerInvariant() == ControllerNames.Jenkins;
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public static void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            SetEnvironmentConfiguration(app, env);

            SetConfigurationBuilder(env);

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();

            app.UseCors("DevPortalCors");

            app.UseCookiePolicy();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseSession();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
            });
        }

        static void SetEnvironmentConfiguration(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsEnvironment("Local"))
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseStatusCodePagesWithReExecute("/" + ControllerNames.Error + "/{0}");

                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
        }

        static void SetConfigurationBuilder(IWebHostEnvironment env)
        {
            new ConfigurationBuilder()
                  .SetBasePath(env.ContentRootPath)
                  .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                  .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                  .AddEnvironmentVariables();
        }
    }
}