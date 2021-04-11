﻿using System;
using Microsoft.EntityFrameworkCore;
using Volo.Abp;

namespace Jh.Abp.FormCustom.EntityFrameworkCore
{
    public static class FormCustomDbContextModelCreatingExtensions
    {
        public static void ConfigureFormCustom(
            this ModelBuilder builder,
            Action<FormCustomModelBuilderConfigurationOptions> optionsAction = null)
        {
            Check.NotNull(builder, nameof(builder));

            var options = new FormCustomModelBuilderConfigurationOptions(
                FormCustomDbProperties.DbTablePrefix,
                FormCustomDbProperties.DbSchema
            );

            optionsAction?.Invoke(options);

            /* Configure all entities here. Example:

            builder.Entity<Question>(b =>
            {
                //Configure table & schema name
                b.ToTable(options.TablePrefix + "Questions", options.Schema);
            
                b.ConfigureByConvention();
            
                //Properties
                b.Property(q => q.Title).IsRequired().HasMaxLength(QuestionConsts.MaxTitleLength);
                
                //Relations
                b.HasMany(question => question.Tags).WithOne().HasForeignKey(qt => qt.QuestionId);

                //Indexes
                b.HasIndex(q => q.CreationTime);
            });
            */
        }
    }
}