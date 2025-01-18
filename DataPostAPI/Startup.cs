using AutoMapper;
using CorePush.Apple;
using CorePush.Google;
using DataPostAPI.Data;
using DataPostAPI.Handlers;
using DataPostAPI.Helpers;
using DataPostAPI.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Design;
namespace DataPostAPI
{
    public class Startup
    {

        public Startup(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public IConfiguration configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddTransient<INotificationService, NotificationService>();
            services.AddHttpClient<FcmSender>();
            services.AddHttpClient<ApnSender>();
            services.AddSingleton<AnomalyDetectionSystem>();
            services.AddScoped<SecuritySystem>();
            services.AddScoped<INotificationService, NotificationService>();

            // Configure strongly typed settings objects
            var appSettingsSection = configuration.GetSection("FcmNotification");
            services.Configure<FcmNotificationSetting>(appSettingsSection);

            var appSettingsSection1 = configuration.GetSection("AppSettings");
            services.Configure<AppSettings>(appSettingsSection1);
            services.Configure<AppSettings>(appSettingsSection1);

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "DataPostAPI", Version = "v1" });
            });
            services.AddDbContext<ClientContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DevConnection")));
            services.AddCors();
            var appSettings = appSettingsSection1.Get<AppSettings>();
            var key = Encoding.ASCII.GetBytes(appSettings.Secret);
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
               .AddJwtBearer(x =>
               {
                   x.Events = new JwtBearerEvents
                   {
                       OnTokenValidated = context =>
                       {
                           var userService = context.HttpContext.RequestServices.GetRequiredService<IUserService>();
                           var userId = int.Parse(context.Principal.Identity.Name);
                           var user = userService.GetById(userId);
                           if (user == null)
                           {
                            // return unauthorized if user no longer exists
                            context.Fail("Unauthorized");
                           }
                           return Task.CompletedTask;
                       }
                   };
                   x.RequireHttpsMetadata = false;
                   x.SaveToken = true;
                   x.TokenValidationParameters = new TokenValidationParameters
                   {
                       ValidateIssuerSigningKey = true,
                       IssuerSigningKey = new SymmetricSecurityKey(key),
                       ValidateIssuer = false,
                       ValidateAudience = false
                   };
               });
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IClientService, ClientService>();
            services.AddScoped<ICameraService, CameraService>();

            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new AutoMapperProfile());
            });

            IMapper mapper = mappingConfig.CreateMapper();
            services.AddSingleton(mapper);

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "DataPostAPI v1"));
            }
            app.UseCors(options =>
                options.WithOrigins("http://localhost:4200")
   .            AllowAnyMethod()
   .            AllowAnyHeader());

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();
            app.UseAuthentication();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
            app.Run(async (context) =>
            {
                await context.Response.WriteAsync("404 Not Found. Try Again!");
            });

        }
    }
}
