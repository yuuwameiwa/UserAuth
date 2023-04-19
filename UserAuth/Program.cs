using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Net.Http.Headers;

namespace UserAuth
{
    public class Program
    {
        public static void Main()
        {
            var builder = WebApplication.CreateBuilder();

            HttpClient httpClient = new HttpClient();

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            #region Store JWT in Cookie
            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultSignInScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = true;
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("yuuwameiwaniuuwa"))
                };
                options.SaveToken = true;
            });
            #endregion

            #region Http client tries
            /*
            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultSignInScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options => 
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = "https://localhost:7234/",
                        ValidAudience = "localhost",
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("yuuwameiwaniuuwa"))
                    };
                    options.RequireHttpsMetadata = true;
                    options.SaveToken = true;
                    /*
                    options.Events = new JwtBearerEvents
                    {
                        OnMessageReceived = context =>
                        {
                            bool connectionHandled = context.HttpContext.Items.ContainsKey("connectionHandled");

                            if (!connectionHandled)
                            {
                                HttpClient httpClient = context.HttpContext.RequestServices.GetRequiredService<HttpClient>();
                            
                                if (httpClient.DefaultRequestHeaders.Contains("Authorization"))
                                {
                                    if (!string.IsNullOrEmpty(httpClient.DefaultRequestHeaders.Authorization?.ToString()))
                                    {
                                        string token = httpClient.DefaultRequestHeaders.Authorization.ToString();
                                        context.Token = token;
                                        context.Request.Headers.Add("Authorization", "Bearer " + token);
                                    }
                                }

                                context.HttpContext.Items.Add("connectionHandled", true);
                            }

                            return Task.CompletedTask;
                        }
                    };
                });
            */
            #endregion

            builder.Services.AddAuthorization();

            var app = builder.Build();

            app.Use(async (context, next) =>
            {
                string jwtToken = context.Request.Cookies["jwt"];
                if (jwtToken != null)
                    context.Request.Headers.Add("Authorization", "Bearer " + jwtToken);

                await next();
            });

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}