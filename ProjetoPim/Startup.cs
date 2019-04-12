using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ProjetoPim.Data;
using ProjetoPim.Models;
using ProjetoPim.Services;

namespace ProjetoPim
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)

            
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseMySql(Configuration.GetConnectionString("DefaultConnection"), builder =>
            builder.MigrationsAssembly("ProjetoPim")));


            //services.AddScoped<ChamadoService>();
            services.AddScoped<SeedingService>();
           // services.AddScoped<DepartmentService>();




            services.AddIdentity<ApplicationUser, IdentityRole>(options =>
            {
                //Bloqueio do usuário
                options.Lockout.AllowedForNewUsers = true;
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromDays(30);

                //Restrições de senha
                options.Password.RequireDigit = false; //Obrigatório número
                options.Password.RequiredLength = 5; // Tamanho mínimo
                options.Password.RequireUppercase = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireNonAlphanumeric = false;


                //Token
                //options.Tokens.AuthenticatorTokenProvider 
                //options.Tokens.ChangeEmailTokenProvider
                //options.Tokens.ChangePhoneNumberTokenProvider
                //options.Tokens.EmailConfirmationTokenProvider
                //options.Tokens.PasswordResetTokenProvider

                //User
                //options.User.AllowedUserNameCharacters = ""; //Define os caracteres permitidos no Username
                options.User.RequireUniqueEmail = true; //Determina Email único
            })
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            // Add application services.
            services.ConfigureApplicationCookie(options =>
            {
               // options.AccessDeniedPath = "/Account/AcessDenied"; //Define o redirecionamento de acesso negado
                //options.ClaimsIssuer = "";
                //options.Cookie.Domain = "";
                //options.Cookie.Expiration = "";
                //options.Cookie.HttpOnly = true;
                //options.Cookie.Name = "AspNetCore.Cookies";
                //options.Cookie.Path = "";
                //options.Cookie.SameSite = "";
                //options.CookieManager = "";
                //options.DataProtectionProvider
                //options.Events
                //options.EventsType
                //options.ExpireTimeSpan = TimeSpan.FromSeconds(15); //Define o tempo de expiração do login
                options.LoginPath = "/Account/Login"; //Define o redirecionamento do login
                options.LogoutPath = "/Account/Logout"; //Define o redirecionamento do logout
                options.ReturnUrlParameter = "ReturnUrl"; //Define o parâmetro de retorno da Url
                //options.SessionStore =
                options.SlidingExpiration = true; //Cria um novo cookie para manter ativo o login
                //options.TicketDataFormat =
            });

            services.AddTransient<IEmailSender, EmailSender>();

            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, SeedingService seedingService)
        {
            if (env.IsDevelopment())
            {
                app.UseBrowserLink();
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
                //seedingService.Seed();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            app.UseAuthentication();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
