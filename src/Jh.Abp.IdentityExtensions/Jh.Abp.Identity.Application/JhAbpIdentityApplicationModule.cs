using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Identity;
using Volo.Abp.Modularity;

namespace Jh.Abp.Identity.Application
{
    [DependsOn(
        typeof(AbpIdentityApplicationModule)
        )]
    public class JhAbpIdentityApplicationModule:AbpModule
    {
    }
}
