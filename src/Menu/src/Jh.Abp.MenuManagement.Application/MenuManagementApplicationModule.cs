using Jh.Abp.Identity.Application;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Application;
using Volo.Abp.AutoMapper;
using Volo.Abp.Modularity;

namespace Jh.Abp.MenuManagement
{
    [DependsOn(
        typeof(JhAbpIdentityApplicationModule),
        typeof(MenuManagementDomainModule),
        typeof(MenuManagementApplicationContractsModule),
        typeof(AbpDddApplicationModule),
        typeof(AbpAutoMapperModule)
        )]
    public class MenuManagementApplicationModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddAutoMapperObjectMapper<MenuManagementApplicationModule>();
            Configure<AbpAutoMapperOptions>(options =>
            {
                options.AddMaps<MenuManagementApplicationModule>(validate: true);
            });
        }
    }
}
