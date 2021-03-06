﻿using Volo.Abp.Modularity;
using Volo.Abp.Localization;
using Jh.Abp.MenuManagement.Localization;
using Volo.Abp.Localization.ExceptionHandling;
using Volo.Abp.Validation;
using Volo.Abp.Validation.Localization;
using Volo.Abp.VirtualFileSystem;

namespace Jh.Abp.MenuManagement
{
    [DependsOn(
        typeof(AbpLocalizationModule),
        typeof(AbpValidationModule)
    )]
    public class MenuManagementDomainSharedModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<AbpVirtualFileSystemOptions>(options =>
            {
                options.FileSets.AddEmbedded<MenuManagementDomainSharedModule>();
            });

            Configure<AbpLocalizationOptions>(options =>
            {
                options.Resources
                    .Add<MenuManagementResource>("zh-Hans")
                    .AddBaseTypes(typeof(AbpValidationResource))
                    .AddVirtualJson("/Localization/MenuManagement");//当前项目资源按照文件路径写
            });

            Configure<AbpExceptionLocalizationOptions>(options =>
            {
                options.MapCodeNamespace("MenuManagement", typeof(MenuManagementResource));
            });
        }
    }
}
