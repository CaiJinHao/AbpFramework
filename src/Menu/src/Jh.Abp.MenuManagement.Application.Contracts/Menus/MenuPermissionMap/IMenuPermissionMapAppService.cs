using Jh.Abp.Extensions;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.PermissionManagement;

namespace Jh.Abp.MenuManagement
{
	public interface IMenuPermissionMapAppService
		: ICrudApplicationService<MenuPermissionMap, MenuPermissionMapDto, MenuPermissionMapDto, System.Guid, MenuPermissionMapRetrieveInputDto, MenuPermissionMapCreateInputDto, MenuPermissionMapUpdateInputDto, MenuPermissionMapDeleteInputDto>
	{
		Task<IEnumerable<MenusTreeDto>> GetPermissionTreesAsync(Guid menuid, string providerName, string providerKey);

		Task UpdateAsync(string providerName, string providerKey, UpdatePermissionsDto input);

		Task<List<PermissionDefinition>> GetPermissionGrantsAsync();

		Task<IEnumerable<LocalizedString>> GetLocalizePermissionGrantsAsync();
	}
}