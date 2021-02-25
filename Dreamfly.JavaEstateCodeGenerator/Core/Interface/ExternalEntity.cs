using System.Collections.Generic;
using System.Linq;
using Dreamfly.JavaEstateCodeGenerator.Helper;
using Dreamfly.JavaEstateCodeGenerator.Models;

namespace Dreamfly.JavaEstateCodeGenerator.Core.Interface
{
    public abstract class ExternalEntity
    {
        protected readonly List<Entity> JsonEntities = JsonDataHelper.ReadEntities();

        protected Entity ResultEntity;

        protected List<string> FilterFields = new List<string>
        {
            "seq", "id", "adduser_id", "adddate", "moduser_id", "moddate"
        };


        public abstract Entity ReadEntity();

        protected abstract void AddFields();

        protected void MakeResultEntity()
        {
            ResultEntity = new Entity
            {
                HasIHasCompany = true,
                HasIHasTenant = true,
                IsSync = false,
                EntityItems = new List<EntityItem>()
            };
        }

        protected void AddTenantField()
        {
            if (ResultEntity.HasIHasTenant)
            {
                var entityItem = ResultEntity.EntityItems.FirstOrDefault(t => t.Name == "tenantId");
                if (entityItem == null)
                {
                    ResultEntity.EntityItems.Add(new EntityItem
                    {
                        Name = "tenantId",
                        ColumnName = "TenantId",
                        Description = "集團編號",
                        IsRequired = false,
                        Type = "Long",
                        InQuery = false,
                        InCreate = false,
                        InResponse = false
                    });
                }
                else
                {
                    entityItem.Type = "Long";
                    entityItem.Description = "集團編號";
                    entityItem.IsRequired = false;
                    entityItem.InQuery = false;
                    entityItem.InCreate = false;
                    entityItem.InResponse = false;
                }
            }
        }

        protected void AddCompanyField()
        {
            if (ResultEntity.HasIHasCompany)
            {
                var entityItem = ResultEntity.EntityItems.FirstOrDefault(t => t.Name == "companyId");
                if (entityItem == null)
                {
                    ResultEntity.EntityItems.Add(new EntityItem
                    {
                        Name = "companyId",
                        ColumnName = "CompanyId",
                        Description = "公司編號",
                        IsRequired = false,
                        Type = "Long",
                        InQuery = true,
                        InCreate = false,
                        InResponse = false
                    });
                }
                else
                {
                    entityItem.Type = "Long";
                    entityItem.Description = "公司編號";
                    entityItem.IsRequired = false;
                    entityItem.InQuery = true;
                    entityItem.InCreate = false;
                    entityItem.InResponse = false;
                }
            }
        }

        protected void SetEntityItemDefaultValues(EntityItem item)
        {
            if (item.Name == "code" || item.Name == "name")
            {
                item.Length = 20;
                item.InQuery = true;
            }

            if (item.Name == "memo")
            {
                item.Length = 1000;
            }

            if (item.ColumnName.EndsWith("_Id"))
            {
                item.Type = "Long";
                if (!item.ColumnName.Contains("File"))
                {
                    item.InQuery = true;
                }
            }

            if (item.ColumnName.StartsWith("Code_"))
            {
                item.Type = "Long";
                item.InQuery = true;
            }
        }

        protected bool IsFilterField(string field)
        {
            return FilterFields.Contains(field.ToLower());
        }

        protected string ConvertFieldName(string field)
        {
            return ToCamelCase(RemoveUnderLine(field));
        }

        protected string RemoveUnderLine(string field)
        {
            return field.Replace("_", "");
        }

        private string ToCamelCase(string field)
        {
            return char.ToLowerInvariant(field[0]) + field.Substring(1);
        }
    }
}