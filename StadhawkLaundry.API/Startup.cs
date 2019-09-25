using System;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Stadhawk.Laundry.Utility.Handler;
using Stadhawk.Laundry.Utility.IHandler;
using Stadhawk.Laundry.Utility.Model;
using StadhawkLaundry.API.Data;
using StadhawkLaundry.API.Handler;
using StadhawkLaundry.API.Mappings;
using StadhawkLaundry.API.Models;
using StadhawkLaundry.BAL.Core;
using StadhawkLaundry.BAL.Persistence;
using StadhawkLaundry.DataModel;

namespace StadhawkLaundry.API
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
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });
            string connection = Configuration.GetConnectionString("DatabaseConnection");
            services.AddDbContext<LaundryContext>(options => options.UseSqlServer(connection, b => b.UseRowNumberForPaging()));
            services.AddDbContextPool<ApplicationDbContext>(options => options.UseSqlServer(connection));
            services.AddIdentity<ApplicationUser, ApplicationRole>().AddEntityFrameworkStores<ApplicationDbContext>().AddDefaultTokenProviders();
            var appSettingsSection = Configuration.GetSection("AppSettings");
            services.Configure<AppSettings>(appSettingsSection);

            // configure jwt authentication

            var appSettings = appSettingsSection.Get<AppSettings>();
            var key = Encoding.ASCII.GetBytes(appSettings.Secret);
            var keySmsHas = Encoding.ASCII.GetBytes(appSettings.SmsHasKey);
            var keySmsVendorUrl = Encoding.ASCII.GetBytes(appSettings.SmsVendorUrl);
            var databsesecon = Encoding.ASCII.GetBytes(appSettings.DataBaseCon);
            var razorpayKey = Encoding.ASCII.GetBytes(appSettings.RazorpayKey);
            var razorpaySecret = Encoding.ASCII.GetBytes(appSettings.RazorpaySecret);
            var PGType = Encoding.ASCII.GetBytes(appSettings.PGType);
            var RazorPayReturnUrl = Encoding.ASCII.GetBytes(appSettings.RazorPayReturnUrl);
            var RazorPayColor = Encoding.ASCII.GetBytes(appSettings.RazorPayColor);
            var RazorPayDescription = Encoding.ASCII.GetBytes(appSettings.RazorPayDescription);
            var RazorPayLogo = Encoding.ASCII.GetBytes(appSettings.RazorPayLogo);

            //PAYTM PAYMENT GATEWAY CONSTANTS
            var PAYTM_MID = Encoding.ASCII.GetBytes(appSettings.PAYTM_MID);
            var PAYTM_MERCHANT_KEY = Encoding.ASCII.GetBytes(appSettings.PAYTM_MERCHANT_KEY);
            var PAYTM_INDUSTRY_TYPE_ID = Encoding.ASCII.GetBytes(appSettings.PAYTM_INDUSTRY_TYPE_ID);
            var PAYTM_CHANNEL_ID = Encoding.ASCII.GetBytes(appSettings.PAYTM_CHANNEL_ID);
            var PAYTM_WEBSITE = Encoding.ASCII.GetBytes(appSettings.PAYTM_WEBSITE);
            var PAYTM_STAGING_URL = Encoding.ASCII.GetBytes(appSettings.PAYTM_STAGING_URL);
            var PAYTM_LIVE_URL = Encoding.ASCII.GetBytes(appSettings.PAYTM_LIVE_URL);
            var PAYTM_TRANSACTION_STATUS_STAGING_URL = Encoding.ASCII.GetBytes(appSettings.PAYTM_TRANSACTION_STATUS_STAGING_URL);
            var PAYTM_TRANSACTION_STATUS_LIVE_URL = Encoding.ASCII.GetBytes(appSettings.PAYTM_TRANSACTION_STATUS_LIVE_URL);

            var PgVisibility = Encoding.ASCII.GetBytes(appSettings.PgVisibility);
            var PaytmVisibility = Encoding.ASCII.GetBytes(appSettings.PaytmVisibility);
            var CODVisibility = Encoding.ASCII.GetBytes(appSettings.CODVisibility);

            //company name/ payment environment
            var Company = Encoding.ASCII.GetBytes(appSettings.Company);
            var IsLivePayment = Encoding.ASCII.GetBytes(appSettings.IsLivePayment);
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
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });
            services.AddSingleton<IConfiguration>(Configuration);
            services.AddTransient<IUnitOfWork, UnitOfWork>();
            services.AddTransient<IImageHandler, ImageHandler>();
            services.AddTransient<ImageWriter.Interface.IImageWriter, ImageWriter.Classes.ImageWriter>();
            services.AddScoped<IDirectionHandler, DirectionHandler>();
            services.AddSingleton<ISmsHandler<SmsResponseModel>, SmsHandler<SmsResponseModel>>();

            services.AddScoped<IUrlHelper>(implementationFactory =>
            {
                ActionContext actionContext = implementationFactory.GetService<IActionContextAccessor>().ActionContext;
                return new UrlHelper(actionContext);
            });
            Mapper.Initialize(cfg => cfg.AddProfile<MappingProfile>());

            services.AddAutoMapper();

            services.AddMvc(options => {options.ModelBinderProviders.Insert(0, new DateTimeModelBinderProvider());})
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_2)
                .AddJsonOptions(options =>
                {
                    options.SerializerSettings.ContractResolver = new Newtonsoft.Json.Serialization.DefaultContractResolver();
                });

            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                    builder => builder.AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowCredentials()
                        .WithExposedHeaders("X-Pagination"));
            });
        }
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }
            app.UseDefaultFiles();
            app.UseStaticFiles();
            app.UseCookiePolicy();
            app.UseAuthentication();
            app.UseCors("CorsPolicy");

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
