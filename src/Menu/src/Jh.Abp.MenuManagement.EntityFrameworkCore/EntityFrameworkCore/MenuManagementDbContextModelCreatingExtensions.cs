using System;
using Jh.Abp.MenuManagement.Menus;
using Microsoft.EntityFrameworkCore;
using Volo.Abp;
using Volo.Abp.EntityFrameworkCore.Modeling;
using Volo.Abp.Identity;
using Volo.Abp.ObjectExtending;

namespace Jh.Abp.MenuManagement.EntityFrameworkCore
{
    public static class MenuManagementDbContextModelCreatingExtensions
    {
        public static void ConfigureMenuManagement(
            this ModelBuilder builder,
            Action<MenuManagementModelBuilderConfigurationOptions> optionsAction = null)
        {
            Check.NotNull(builder, nameof(builder));

            var options = new MenuManagementModelBuilderConfigurationOptions(
                MenuManagementDbProperties.DbTablePrefix,
                MenuManagementDbProperties.DbSchema
            );

            optionsAction?.Invoke(options);

            builder.Entity<Menu>(b =>
            {
                b.ConfigureByConvention();
            });

            builder.Entity<MenuAndRoleMap>(b =>
            {
                b.ConfigureByConvention();
                b.Property(p => p.Id).ValueGeneratedOnAdd();

                b.HasOne(mrm => mrm.Menu).WithMany(menu => menu.MenuRoleMaps).HasForeignKey(menu => menu.MenuId);
                b.HasIndex(c => c.RoleId).IncludeProperties(p => p.MenuId);//mysql不能使用包含列
            });

            //builder.Entity<MyUsers>(b =>
            //{
            //    b.ToTable(AbpIdentityDbProperties.DbTablePrefix + "Users"); //Sharing the same table "AbpUsers" with the IdentityUser

            //    b.ConfigureByConvention();

            //    b.Property(x => x.Avatar).IsRequired(false).HasMaxLength(500).HasColumnName(nameof(MyUsers.Avatar));
            //    b.Property(x => x.Introduction).IsRequired(false).HasMaxLength(500).HasColumnName(nameof(MyUsers.Introduction));

            //});
        }
    }
}