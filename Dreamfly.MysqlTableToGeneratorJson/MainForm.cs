using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using LiteX.DbHelper.Core;
using LiteX.DbHelper.MariaDB;
using Newtonsoft.Json;

namespace Dreamfly.MysqlTableToGeneratorJson
{
    public partial class MainForm : Form
    {
        private IDbHelper _dbHelper;
        private readonly List<string> _passFieldNames = new List<string>()
        {
            "seq",
            "id",
            "adduser_id",
            "adddate",
            "moduser_id",
            "moddate"
        };
        private readonly List<string> _searchFieldNames = new List<string>()
        {
            "code",
            "name"
        };

        private List<EntityItem> _entityItems=new List<EntityItem>();

        public MainForm()
        {
            InitializeComponent();
        }

        private void btnGenerator_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(txtTableName.Text))
            {
                MessageBox.Show("請輸入表名！");
                return;
            }

            Generator();
        }

        private void Generator()
        {
            try
            {
                TryGeneratorResult();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        private void TryGeneratorResult()
        {
            DataTable dt = GetFieldTable();
            GeneratorEntityItems(dt);
            txtResult.Text = JsonConvert.SerializeObject(_entityItems, Formatting.Indented);
        }

        private void GeneratorEntityItems(DataTable dt)
        {
            
            if (dt != null && dt.Rows.Count > 0)
            {
                foreach (DataRow row in dt.Rows)
                {
                    var item=MakeItem(row);

                    if (item != null)
                    {
                        _entityItems.Add(item);
                    }
                }
            }
        }

        private EntityItem MakeItem(DataRow row)
        {
            if (_passFieldNames.Contains(row["Field"].ToString().ToLower()))
            {
                return null;
            }

            EntityItem item = new EntityItem
            {
                Name = row["Field"].ToString().Replace("_", ""),
                Description = row["Comment"].ToString(),
                IsRequired = row["Null"].ToString() == "No",
                MapTypes = new List<EntityItemMapType>
                {
                    EntityItemMapType.CreateInput,
                    EntityItemMapType.Output
                }
            };

            //查詢條件
            AddQuery(item);

            //别名
            MakeColumnName(row, item);


            //類型
            SetTypeAndLength(row, item);

            return item;
        }

        private void SetTypeAndLength(DataRow row, EntityItem item)
        {
            bool isRequire = item.IsRequired;
            //类型
            string type = row["Type"].ToString();

            if (type == "double")
            {
                item.Type = isRequire ? "decimal" : "decimal?";
            }

            if (type == "text")
            {
                item.Type = "string";
            }

            if (type == "date" || type == "datetime")
            {
                item.Type = isRequire ? "DateTime" : "DateTime?";
            }

            if (type == "bit" || type == "char(1)")
            {
                item.Type = isRequire ? "bool" : "bool?";
            }

            if (type == "char(36)")
            {
                if (row["Field"].ToString().ToLower().Contains("file_id"))
                {
                    item.Type = isRequire ? "Guid" : "Guid?";
                }
                else
                {
                    item.Type = isRequire ? "int" : "int?";
                }
            }

            //字符串
            Regex regex = new Regex(@"varchar\((\d+)\)");
            bool isMatch = regex.IsMatch(type);
            if (isMatch)
            {
                item.Type = "string";
                Match match = regex.Match(type);
                item.Length = int.Parse(match.Groups[1].Value);
            }
        }

        private void MakeColumnName(DataRow row, EntityItem item)
        {
            var field = row["Field"].ToString();
            //别名
            if (field.Contains("_"))
            {
                item.ColumnName = MakeUpperFirst(field);
            }
        }

        private string MakeUpperFirst(string fieldName)
        {
            return $"{fieldName.Substring(0, 1).ToUpper()}{fieldName.Substring(1)}";
        }

        private void AddQuery(EntityItem item)
        {
            //查詢條件
            if (_searchFieldNames.Contains(item.Name.ToLower()))
            {
                item.MapTypes.Add(EntityItemMapType.QueryInput);
            }
        }

        private DataTable GetFieldTable()
        {
            string sql = $"SHOW FULL COLUMNS FROM {txtTableName.Text} FROM PMERP";
//            string sql =
//                $"select * from information_schema.COLUMNS where table_name = 'Phrase' and table_schema = 'PMERP';";
            _dbHelper = new MariaDBHelper(ConfigurationManager.ConnectionStrings["LiteXConnection"].ConnectionString);
            return _dbHelper.GetDataTable(sql, cmdType:CommandType.Text);
        }

        private void btnCopy_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(txtResult.Text))
            {
                MessageBox.Show("結果數據為空，無需複製！");
                return;
            }

            Clipboard.SetDataObject(txtResult.Text);
            MessageBox.Show("複製成功！");
        }
    }
}
