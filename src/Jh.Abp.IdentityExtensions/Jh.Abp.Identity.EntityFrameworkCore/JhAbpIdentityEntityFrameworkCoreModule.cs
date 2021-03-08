using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Identity.EntityFrameworkCore;
using Volo.Abp.Modularity;

namespace Jh.Abp.Identity.EntityFrameworkCore
{
    [DependsOn(
       typeof(AbpIdentityEntityFrameworkCoreModule))]
    public class JhAbpIdentityEntityFrameworkCoreModule : AbpModule
    {
    }
}
