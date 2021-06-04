﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Reflection;
using Jh.SourceGenerator.Common.GeneratorAttributes;
using Jh.SourceGenerator.Common.GeneratorDtos;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using Jh.SourceGenerator.Common.CodeBuilders;
using System.IO;
using Jh.Abp.Common;

namespace Jh.SourceGenerator.Common
{
    public class GeneratorService
    {
        public  GeneratorOptions generatorOptions { get; set; }
        public  Assembly LoadAssembly { get; }
        public  GeneratorService(Assembly assembly, GeneratorOptions options)
        {
            LoadAssembly = assembly;
            generatorOptions = options;
            GeneratorConsts.DbContext = options.DbContext;
            GeneratorConsts.Namespace = options.Namespace;
            GeneratorConsts.ControllerBase = options.ControllerBase;
        }

        public virtual IEnumerable<Type> GetLoadableTypes()
        { 
            return LoadAssembly.DefinedTypes.Select((TypeInfo t) => t.AsType());
        }

        public virtual IEnumerable<Type> GetTableClass()
        {
            var classCollection = GetLoadableTypes().Where(cla=>cla.IsClass);
            var tableClass = classCollection.Where(tab=>tab.CustomAttributes.Any(a=>a.AttributeType.Equals(typeof(GeneratorClassAttribute))));
            return tableClass;
        }

        public virtual IEnumerable<TableDto> GetTablesDto(IEnumerable<Type> classTypes)
        {
            foreach (var classType in classTypes)
            {
                yield return new TableDto(GeneratorConsts.DbContext, GeneratorConsts.Namespace, GeneratorConsts.ControllerBase)
                {
                    Name = GetTableName(classType),
                    KeyType = GetKeyType(classType),
                    InheritClass = GetTableInheritClass(classType),
                    Comment = GetTableDescription(classType),
                    FieldsCreateOrUpdateInput = GetFieldDto(GetMembers<CreateOrUpdateInputDtoAttribute>(classType)).ToList(),
                    FieldsRetrieve = GetFieldDto(GetMembers<RetrieveDtoAttribute>(classType)).ToList(),
                    FieldsIgnore = GetFieldDto(GetMembers<ProfileIgnoreAttribute>(classType)).ToList(),
                };
            }
        }

        public virtual TableDto GetTableDto(Type classType)
        {
            return new TableDto(GeneratorConsts.DbContext, GeneratorConsts.Namespace, GeneratorConsts.ControllerBase)
            {
                Name = GetTableName(classType),
                KeyType = GetKeyType(classType),
                InheritClass = GetTableInheritClass(classType),
                Comment = GetTableDescription(classType),
                FieldsCreateOrUpdateInput = GetFieldDto(GetMembers<CreateOrUpdateInputDtoAttribute>(classType)).ToList(),
                FieldsRetrieve = GetFieldDto(GetMembers<RetrieveDtoAttribute>(classType)).ToList(),
                FieldsIgnore = GetFieldDto(GetMembers<ProfileIgnoreAttribute>(classType)).ToList(),
            };
        }

        public virtual string GetTableName(Type classType)
        {
            return classType.Name;
        }

        public virtual string GetKeyType(Type classType)
        {
            var baseType = classType.BaseType;
            var genericTypeArgs = baseType.GenericTypeArguments;
            return genericTypeArgs.FirstOrDefault()?.ToString();
        }

        public virtual string GetTableInheritClass(Type classType)
        {
            return classType.BaseType.Name.Replace("`1","");
        }

        public virtual string GetTableDescription(Type classType)
        {
            var attr = classType.CustomAttributes.Where(a => a.AttributeType.Equals(typeof(DescriptionAttribute))).FirstOrDefault();
            if (attr != null)
            {
                var args = attr.ConstructorArguments.FirstOrDefault();
                if (args != null)
                {
                    return args.Value?.ToString();
                }
            }
            return default;
        }


        public virtual IEnumerable<PropertyInfo> GetMembers<TAttribute>(Type classType)
        {
            foreach (var property in classType.GetProperties())
            {
                var attrName = typeof(TAttribute).Name;
                if (property.CustomAttributes.Any(a => a.AttributeType.Equals(typeof(TAttribute))))
                {
                    yield return property;
                }
            }
        }

        public virtual IEnumerable<FieldDto> GetFieldDto(IEnumerable<PropertyInfo> properties)
        {
            foreach (var property in properties)
            {
                var descriptionAttr = GetAttrArgs<DescriptionAttribute>(property)?.FirstOrDefault();
                var description = string.Empty;
                if (descriptionAttr != null)
                {
                    description = descriptionAttr?.Value?.ToString();
                }
                var required = GetAttr<RequiredAttribute>(property);
                var theType = property.PropertyType;
                var valueType = ObjectExtensions.GetObjectType(theType);
                var fieldDto = new FieldDto()
                {
                    Name = GetFiledName(property),
                    Description = description,
                    Type = theType.Name,
                    FieldType = valueType,
                    IsRequired = required != null
                };
                if (theType.IsGenericType && theType.
                      GetGenericTypeDefinition().Equals
                      (typeof(Nullable<>)))
                {
                    fieldDto.IsNullable = true;
                    fieldDto.Type = theType.GetGenericArguments().FirstOrDefault().Name;
                }
                fieldDto.IsNullable = fieldDto.GetIsNullable();
                yield return fieldDto;
            }
        }

        /// <summary>
        /// 获取指定特性
        /// </summary>
        /// <typeparam name="TAttribute"></typeparam>
        /// <param name="property"></param>
        /// <returns></returns>
        public virtual CustomAttributeData GetAttr<TAttribute>(PropertyInfo property)
        {
            return property.CustomAttributes
                .Where(a => a.AttributeType.Equals(typeof(TAttribute))).FirstOrDefault();
        }

        public virtual IEnumerable<CustomAttributeTypedArgument> GetAttrArgs<TAttribute>(PropertyInfo property)
        {
            var attr = property.CustomAttributes
                .Where(a => a.AttributeType.Equals(typeof(TAttribute))).FirstOrDefault();
            if (attr == null)
            {
                return default;
            }
            return attr.ConstructorArguments.AsEnumerable();
        }

        public virtual string GetFiledName(PropertyInfo property)
        {
            return property.Name;
        }

        public virtual bool GeneratorCode()
        { 
            var tables = GetTablesDto(GetTableClass());
            CreateFile(new PermissionsCodeBuilder(tables, generatorOptions.CreateContractsPermissionsPath));
            foreach (var tableDto in tables)
            {
                {//contracts
                    var CreateInputDto = CreateFile(new CreateInputDtoCodeBuilder(tableDto, generatorOptions.CreateContractsPath));
                    var CreateRetrieveInputDto = CreateFile(new RetrieveInputDtoCodeBuilder(tableDto, generatorOptions.CreateContractsPath));
                    var CreateIDapperRepository = CreateFile(new IDapperRepositoryCodeBuilder(tableDto, generatorOptions.CreateContractsPath));
                    var CreateDeleteInputDto = CreateFile(new DeleteInputDtoCodeBuilder(tableDto, generatorOptions.CreateContractsPath));
                    var CreateUpdateInputDto = CreateFile(new UpdateInputDtoCodeBuilder(tableDto, generatorOptions.CreateContractsPath));
                    var CreateDomainDto = CreateFile(new DomainDtoCodeBuilder(tableDto, generatorOptions.CreateContractsPath));
                    var CreateIAppService = CreateFile(new IAppServiceCodeBuilder(tableDto, generatorOptions.CreateContractsPath));
                }
                var CreateIRepository = CreateFile(new IRepositoryCodeBuilder(tableDto, generatorOptions.CreateDomainPath));
                var CreateDapperRepository = CreateFile(new DapperRepositoryCodeBuilder(tableDto, generatorOptions.CreateEfCorePath));
                var CreateRepository = CreateFile(new RepositoryCodeBuilder(tableDto, generatorOptions.CreateEfCorePath));
                var CreateAppService = CreateFile(new AppServiceCodeBuilder(tableDto, generatorOptions.CreateApplicationPath));
                var CreateProfile = CreateFile(new ProfileCodeBuilder(tableDto, generatorOptions.CreateApplicationPath));
                var CreateController = CreateFile(new ControllerCodeBuilder(tableDto, generatorOptions.CreateHttpApiPath));
                //获取去模板列表
                foreach (var file in new DirectoryInfo(generatorOptions.CreateHtmlTemplatePath).GetFiles())
                {
                    if (file.Extension.Equals(".cshtml"))
                    {
                        var _filePath = Path.Combine(generatorOptions.CreateHtmlTemplatePath, file.FullName);
                        CreateFile(new HtmlTemplateCodeBuilder(_filePath, tableDto, generatorOptions.CreateHtmlPath));
                    }
                }
            }
            return true;
        }

        public virtual bool CreateFile(CodeBuilderAbs codeBuilder)
        {
            new DirectoryInfo(codeBuilder.FilePath).CreateDirectoryInfo();
            var filePath = Path.Combine(codeBuilder.FilePath, codeBuilder.FileName + codeBuilder.Suffix);
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
            File.WriteAllText(filePath, codeBuilder.ToString());
            return true;
        }
    }
}
