using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Identity;
using Volo.Abp.ObjectExtending;

namespace Jh.Abp.Identity.Application.Identity
{
    //TODO:暂时没有用到重写
    [Dependency(ReplaceServices = true)]
    [ExposeServices(
        typeof(IIdentityUserAppService)
        )]
    public class JhIdentityUserAppService : IdentityUserAppService
    {
        public JhIdentityUserAppService(IdentityUserManager userManager, IIdentityUserRepository userRepository, IIdentityRoleRepository roleRepository, IOptions<IdentityOptions> identityOptions) : base(userManager, userRepository, roleRepository, identityOptions)
        {
        }

        [Authorize(IdentityPermissions.Users.Create)]
        public override async Task<IdentityUserDto> CreateAsync(IdentityUserCreateDto input)
        {
            await IdentityOptions.SetAsync();

            var user = new IdentityUser(
                GuidGenerator.Create(),
                input.UserName,
                input.Email,
                CurrentTenant.Id
            );

            input.MapExtraPropertiesTo(user);

            (await UserManager.CreateAsync(user, input.Password)).CheckErrors();
            await UpdateUserByInput(user, input);

            await CurrentUnitOfWork.SaveChangesAsync();

            return ObjectMapper.Map<IdentityUser, IdentityUserDto>(user);
        }
    }
}
