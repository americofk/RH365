using D365_API_Nomina.Infrastructure;
using D365_API_Nomina.Core.Application;
using Microsoft.AspNetCore.Mvc;
using D365_API_Nomina.Core.Application.Common.Model;
using D365_API_Nomina.Core.Application.Common.Helper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using D365_API_Nomina.Core.Application.Common.Interface;
using D365_API_Nomina.WebUI.Services;

namespace D365_API_Nomina.WebUI
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
            services.AddInfrastructure(Configuration);

            //Registro de servicio para obtener el id del usuario 
            services.AddScoped<ICurrentUserInformation, CurrentUserInformation>();
            services.AddHttpContextAccessor();

            //Función que evalua los modelos y envia los mensajes de error customizados
            //Se sustituye el valor de BadRequestObjectResult con un objeto personalizado
            services.PostConfigure<ApiBehaviorOptions>(o =>
            {
                o.InvalidModelStateResponseFactory = actionContext =>
                    new BadRequestObjectResult(new Response<string>
                    {
                        Succeeded = false,
                        StatusHttp = 400,
                        Errors = actionContext.ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage)

                    });
            });

            //Register config for jwt
            services.Configure<AppSettings>(Configuration.GetSection("AppSettings"));

            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Configuration["AppSettings:Secret"])),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });
            //End section jwt

            //Ver método de CQRS
            services.AddApplication();

            //Add attribute validation
            //services.AddScoped<AuthorizePrivilegeAttribute>();


            services.AddControllers(o => o.Conventions.Add(new GroupForVersioningConvention()))
                .AddJsonOptions(
                    o => o.JsonSerializerOptions.Converters.Add(new TimeSpanConverter())
                );

            ////Cors
            //services.AddCors(options => 
            //    options.AddPolicy("signalPolicy",
            //        builder => builder.AllowAnyOrigin()
            //    )
            //);
            ////End Cors

            // Register the Swagger generator
            services.AddSwaggerDocumentation();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            // Enable swagger
            app.UseSwaggerDocumentation();

            app.UseRouting();


            //Configure middlewre of asp.net core for security
            app.UseAuthentication();

            app.UseAuthorization();

            ////Cors
            //app.UseCors("signalPolicy");
            ////End cors
            app.UseAPIKeyAuthentication();

            app.UseEndpoints(endpoints =>
            {
                //SignalR test
                //endpoints.MapHub<HubNotification>("/batchnotification");

                endpoints.MapControllers();
            });
        }
    }
}
