using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Identity;
using Volo.Abp.ObjectExtending;

namespace Jh.Abp.Identity.Application.Identity
{
    [Dependency(ReplaceServices = true)]
    [ExposeServices(
        typeof(IIdentityUserAppService)
        )]
    public class JhIdentityUserAppService : IdentityUserAppService
    {
        public JhIdentityUserAppService(IdentityUserManager userManager, IIdentityUserRepository userRepository, IIdentityRoleRepository roleRepository, IOptions<IdentityOptions> identityOptions) : base(userManager, userRepository, roleRepository, identityOptions)
        {
        }

        [Authorize(IdentityPermissions.Users.Default)]
        public override async Task<PagedResultDto<IdentityUserDto>> GetListAsync(GetIdentityUsersInput input)
        {
            var count = await UserRepository.GetCountAsync(input.Filter);
            var list = await UserRepository.GetListAsync(input.Sorting, input.MaxResultCount, input.SkipCount, input.Filter);

            return new PagedResultDto<IdentityUserDto>(
                count,
                ObjectMapper.Map<List<IdentityUser>, List<IdentityUserDto>>(list)
            );
        }
    }
}
