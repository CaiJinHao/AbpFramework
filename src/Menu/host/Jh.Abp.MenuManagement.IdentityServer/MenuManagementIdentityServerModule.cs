using Jh.Abp.MenuManagement.MultiTenancy;
using Jh.Abp.QuickComponents.Cors;
using Jh.Abp.QuickComponents.Localization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using StackExchange.Redis;
using System;
using System.Linq;
using Volo.Abp;
using Volo.Abp.Account;
using Volo.Abp.Account.Web;
using Volo.Abp.AspNetCore.Authentication.JwtBearer;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.UI.MultiTenancy;
using Volo.Abp.AspNetCore.Mvc.UI.Theme.Basic;
using Volo.Abp.AspNetCore.Mvc.UI.Theme.Shared;
using Volo.Abp.AspNetCore.Serilog;
using Volo.Abp.Auditing;
using Volo.Abp.AuditLogging.EntityFrameworkCore;
using Volo.Abp.Autofac;
using Volo.Abp.Caching;
using Volo.Abp.Caching.StackExchangeRedis;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.SqlServer;
using Volo.Abp.FeatureManagement;
using Volo.Abp.FeatureManagement.EntityFrameworkCore;
using Volo.Abp.Identity;
using Volo.Abp.Identity.EntityFrameworkCore;
using Volo.Abp.IdentityServer.EntityFrameworkCore;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Volo.Abp.MultiTenancy;
using Volo.Abp.PermissionManagement;
using Volo.Abp.PermissionManagement.EntityFrameworkCore;
using Volo.Abp.PermissionManagement.HttpApi;
using Volo.Abp.PermissionManagement.Identity;
using Volo.Abp.SettingManagement.EntityFrameworkCore;
using Volo.Abp.Swashbuckle;
using Volo.Abp.TenantManagement;
using Volo.Abp.TenantManagement.EntityFrameworkCore;
using Volo.Abp.Threading;
using Volo.Abp.UI.Navigation.Urls;
using IdentityUser = Volo.Abp.Identity.IdentityUser;

namespace Jh.Abp.MenuManagement
{
    [DependsOn(
        typeof(AbpAccountWebIdentityServerModule),
        typeof(AbpAccountApplicationModule),
        typeof(AbpAspNetCoreMvcUiMultiTenancyModule),
        typeof(AbpAspNetCoreMvcModule),
        typeof(AbpAspNetCoreMvcUiBasicThemeModule),
        typeof(AbpAuditLoggingEntityFrameworkCoreModule),
        typeof(AbpAutofacModule),
        typeof(AbpCachingStackExchangeRedisModule),
        typeof(AbpEntityFrameworkCoreSqlServerModule),
        typeof(AbpIdentityEntityFrameworkCoreModule),
        typeof(AbpIdentityApplicationModule),
        typeof(AbpIdentityHttpApiModule),
        typeof(AbpIdentityServerEntityFrameworkCoreModule),
        typeof(AbpPermissionManagementDomainIdentityModule),
        typeof(AbpPermissionManagementEntityFrameworkCoreModule),
        typeof(AbpPermissionManagementApplicationModule),
        typeof(AbpPermissionManagementHttpApiModule),
        typeof(AbpSettingManagementEntityFrameworkCoreModule),
        typeof(AbpFeatureManagementEntityFrameworkCoreModule),
        typeof(AbpFeatureManagementApplicationModule),
        typeof(AbpFeatureManagementHttpApiModule),
        typeof(AbpTenantManagementEntityFrameworkCoreModule),
        typeof(AbpTenantManagementApplicationModule),
        typeof(AbpTenantManagementHttpApiModule),
        typeof(AbpAspNetCoreAuthenticationJwtBearerModule),
        typeof(MenuManagementApplicationContractsModule),
        typeof(AbpAspNetCoreSerilogModule),
        typeof(AbpSwashbuckleModule)
        )]
    public class MenuManagementIdentityServerModule : AbpModule
    {
        private const string DefaultCorsPolicyName = "Default";
        public override void PreConfigureServices(ServiceConfigurationContext context)
        {
            PreConfigure<IIdentityServerBuilder>(builder => {
                builder.Services.AddTransient<IObjectAccessor<IUserClaimsPrincipalFactory<IdentityUser>>, ObjectAccessor<JhUserClaimsPrincipalFactory>>();
            });
        }

        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            var hostingEnvironment = context.Services.GetHostingEnvironment();
            var configuration = context.Services.GetConfiguration();

            Configure<AbpDbContextOptions>(options =>
            {
                options.UseSqlServer();
            });

            context.Services.AddSwaggerGen(
                options =>
                {
                    options.SwaggerDoc("v1", new OpenApiInfo { Title = "MenuManagement API", Version = "v1" });
                    options.DocInclusionPredicate((docName, description) => true);
                    options.CustomSchemaIds(type => type.FullName);
                });

            Configure<AbpLocalizationOptions>(options =>
            {
                options.Languages.Add(new LanguageInfo("cs", "cs", "Čeština"));
                options.Languages.Add(new LanguageInfo("en", "en", "English"));
                options.Languages.Add(new LanguageInfo("fr", "fr", "Français"));
                options.Languages.Add(new LanguageInfo("hu", "hu", "Magyar"));
                options.Languages.Add(new LanguageInfo("pt-BR", "pt-BR", "Português"));
                options.Languages.Add(new LanguageInfo("ru", "ru", "Русский"));
                options.Languages.Add(new LanguageInfo("tr", "tr", "Türkçe"));
                options.Languages.Add(new LanguageInfo("zh-Hans", "zh-Hans", "简体中文"));
                options.Languages.Add(new LanguageInfo("zh-Hant", "zh-Hant", "繁體中文"));
            });

            Configure<AbpAuditingOptions>(options =>
            {
                //options.IsEnabledForGetRequests = true;
                options.ApplicationName = "AuthServer";
                options.IsEnabledForAnonymousUsers = false;
                options.EntityHistorySelectors.AddAllEntities();
            });

            Configure<AppUrlOptions>(options =>
            {
                options.Applications["MVC"].RootUrl = configuration["App:SelfUrl"];
            });

            context.Services.AddAuthentication()
                .AddJwtBearer(options =>
                {
                    options.Authority = configuration["AuthServer:Authority"];
                    options.RequireHttpsMetadata = Convert.ToBoolean(configuration["AuthServer:RequireHttpsMetadata"]);
                    options.Audience = configuration["AuthServer:ApiName"];
                });

            Configure<AbpDistributedCacheOptions>(options =>
            {
                options.KeyPrefix = "MenuManagement:";
            });

            Configure<AbpMultiTenancyOptions>(options =>
            {
                options.IsEnabled = MultiTenancyConsts.IsEnabled;
            });

            if (!hostingEnvironment.IsDevelopment())
            {
                var redis = ConnectionMultiplexer.Connect(configuration["Redis:Configuration"]);
                context.Services
                    .AddDataProtection()
                    .PersistKeysToStackExchangeRedis(redis, "MenuManagement-Protection-Keys");
            }

            context.Services.AddCors(options =>
            {
                options.AddPolicy(DefaultCorsPolicyName, builder =>
                {
                    builder
                        .WithOrigins(
                            configuration["App:CorsOrigins"]
                                .Split(",", StringSplitOptions.RemoveEmptyEntries)
                                .Select(o => o.RemovePostFix("/"))
                                .ToArray()
                        )
                        .WithAbpExposedHeaders()
                        .SetIsOriginAllowedToAllowWildcardSubdomains()
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowCredentials();
                });
            });

            context.Services.AddSameSiteCookiePolicy();
            context.Services.AddLocalizationComponent();
        }

        public override void OnApplicationInitialization(ApplicationInitializationContext context)
        {
            var app = context.GetApplicationBuilder();
            var env = context.GetEnvironment();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseErrorPage();
                //app.UseHsts();
            }

            //app.UseHttpsRedirection();
            app.UseCookiePolicy();
            app.UseCorrelationId();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseCors(DefaultCorsPolicyName);
            app.UseAuthentication();
            app.UseJwtTokenMiddleware();

            if (MultiTenancyConsts.IsEnabled)
            {
                app.UseMultiTenancy();
            }

            app.UseJhRequestLocalization();
            app.UseIdentityServer();
            app.UseAuthorization();
            app.UseSwagger();
            app.UseAbpSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "Support APP API");
            });
            app.UseAuditing();
            app.UseAbpSerilogEnrichers();
            app.UseConfiguredEndpoints();

            SeedData(context);
        }

        private void SeedData(ApplicationInitializationContext context)
        {
            AsyncHelper.RunSync(async () =>
            {
                using (var scope = context.ServiceProvider.CreateScope())
                {
                    var data = scope.ServiceProvider
                        .GetRequiredService<IDataSeeder>();
                    var context = new DataSeedContext();
                    context["AdminEmail"] = "531003539@qq.com";
                    context["AdminPassword"] = "KimHo@123";
                    await data.SeedAsync(context);
                }
            });
        }
    }
}
