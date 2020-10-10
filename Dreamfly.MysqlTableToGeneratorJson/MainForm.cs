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

namespace Dreamfly.MysqlTableToGeneratorJson
{
    public partial class MainForm : Form
    {
        private IDbHelper _dbHelper;
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

            DataTable dt = GetFieldTable();

            List<EntityItem> items=new List<EntityItem>();

            if (dt != null && dt.Rows.Count > 0)
            {
                foreach (DataRow row in dt.Rows)
                {
                    if (row["Field"].ToString() == "Seq" || row["Field"].ToString() == "Id")
                    {
                        continue;
                    }

                    EntityItem item=new EntityItem
                    {
                        Name=row["Field"].ToString(),
                        Description=row["Comment"].ToString(),
                        IsRequired=row["Null"].ToString()=="No",
                        MapTypes=new List<EntityItemMapType> { 
                            EntityItemMapType.CreateInput, 
                            EntityItemMapType.Output }
                    };

                    if (row["Field"].ToString().Contains("_"))
                    {
                        item.ColumnName = row["Field"].ToString().Replace("_", "");
                    }

                    string type = row["Type"].ToString();
                    //類型
                    Regex regex=new Regex(@"varchar\((/d+)\)");
                    bool isMatch = regex.IsMatch(type);

                }
            }

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                
            }


        }

        private DataTable GetFieldTable()
        {
            string sql = $"SHOW FULL COLUMNS FROM {txtTableName.Text}";
            _dbHelper = new MariaDBHelper(ConfigurationManager.ConnectionStrings["LiteXConnection"].ConnectionString);
            return _dbHelper.GetDataTable(sql);
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
