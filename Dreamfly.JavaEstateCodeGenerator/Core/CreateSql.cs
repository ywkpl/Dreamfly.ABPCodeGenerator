using System;
using System.Text;
using Dreamfly.JavaEstateCodeGenerator.Helper;
using Dreamfly.JavaEstateCodeGenerator.Models;

namespace Dreamfly.JavaEstateCodeGenerator.Core
{
    public class CreateSql
    {
        private readonly EntityDto _dto;
        private StringBuilder _sqlBuilder;

        public CreateSql(EntityDto dto)
        {
            _dto = dto;
            _sqlBuilder=new StringBuilder();
        }

        public String ToSql()
        {

            return "";
        }

        private void GeneratorItemsSql()
        {
        }

        private void ItemSql(EntityItemDto itemDto)
        {
            DataType dataType = itemDto.Type.ToEnum<DataType>(DataType.String);

            switch (dataType)
            {
                case DataType.String:
                    break;
                case DataType.Long:
                    break;
                case DataType.Integer:
                    break;
                case DataType.Boolean:
                    break;
                case DataType.Date:
                    break;
                case DataType.Float:
                    break;
                case DataType.Json:
                    break;
                case DataType.BigDecimal:
                    break;
                case DataType.Text:
                    break;
                case DataType.Set:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        

    }
}