﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using Volo.Abp.Domain.Entities.Auditing;

namespace Jh.Abp.MenuManagement.Menus
{
    [Description("菜单和角色映射表")]
    [Table(MenuManagementDbProperties.BaseDbTablePrefix + "MenuAndRoleMap")]
    public class MenuAndRoleMap : CreationAuditedEntity<Guid>
    {
        [Description("菜单外键")]
        [Required]
        [ForeignKey("Menu")]
        public Guid MenuId { get; set; }

        [Description("角色外键")]
        [Required]
        public Guid RoleId { get; set; }

        public Menu Menu { get; set; }

        public MenuAndRoleMap() { }
        public MenuAndRoleMap(Guid menuid,Guid roleid) {
            this.MenuId = menuid;
            this.RoleId = roleid;
        }
    }
}
