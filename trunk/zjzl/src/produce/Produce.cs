using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using System.Timers;

using MySql.Data.MySqlClient;

namespace zjzl
{
    public partial class ProduceForm : Form
    {
        private readonly string[] seperator ={ ";" };  // 如果支持本地模式，才起作用
        private ArrayList tableMaterial = null;   // 物品表
        private ArrayList tableProduct = null;
        private string employeeID = "0";
        /// <summary>
        /// 消除“提交成功”提示的定时器
        /// </summary>
        System.Timers.Timer eraseTimer = new System.Timers.Timer();
        private LogHelper logHelper = null;

        public ProduceForm()
        {
            InitializeComponent();
        }

        #region 初始化与提交
        private void ProduceForm_Load(object sender, EventArgs e)
        {
            this.Hide();

            try
            {
                logHelper = new LogHelper(Products.produce);
            }
            catch (Exception ex)
            {
                Console.Write(ex);
                NotifyHelper.NotifyUser("初始化日志模块失败, \r\n程序仍可使用，但不记录日志");
            }

            string s = string.Format("{0} starts, version={1}",
                Application.ProductName, Application.ProductVersion);
            WriteLog(s);

            try
            {
                LoginForm l = new LoginForm(Properties.Settings.Default.DbConn, Products.produce);
                if (l.ShowDialog() == DialogResult.OK)
                {
                    employeeID = l.EmpId;
                    this.Show();
                    Init();
                }
                else
                {
                    NotifyHelper.NotifyUser("登录失败, 程序将退出");
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                WriteLog(ex.ToString());
            }
        }

        private void Init()
        {
            MySqlConnection conn = null;
            try
            {
                Reset();
                eraseTimer.Elapsed += new ElapsedEventHandler(eraseTimer_Elapsed);
                eraseTimer.Interval = 2500;

                string pastName = null;
                conn = MySqlConnHelper.GetMySqlConn(Properties.Settings.Default.DbConn);
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandType = CommandType.Text;
                MySqlDataAdapter adapter = null;
                adapter = new MySqlDataAdapter();
                DataTable dt = null;
                conn.Open();

                #region 操作员
                //adapter = new MySqlDataAdapter();
                cmd.CommandText = "select id, name from employee";
                adapter.SelectCommand = cmd;
                dt = new DataTable();
                adapter.Fill(dt);
                conn.Close();
                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        ListItem mt = new ListItem();
                        mt.ID = dt.Rows[i]["id"].ToString();
                        mt.Name = string.Format("{0} -- {1}", dt.Rows[i]["name"].ToString(), mt.ID);
                        comboBoxOperator.Items.Add(mt);
                    }
                    comboBoxOperator.DisplayMember = "Name";
                    comboBoxOperator.ValueMember = "ID";
                }
                #endregion

                #region 原料
                cmd.CommandText = "select id, name, level, price from material order by name asc, level asc;";
                adapter.SelectCommand = cmd;
                dt = new DataTable();
                adapter.Fill(dt);
                conn.Close();
                if (dt.Rows.Count > 0)
                {
                    tableMaterial = new ArrayList();
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        TableMaterial tm = new TableMaterial();
                        tm.id = int.Parse(dt.Rows[i]["id"].ToString());
                        tm.name = dt.Rows[i]["name"].ToString();
                        tm.level = dt.Rows[i]["level"].ToString();
                        tableMaterial.Add(tm);

                        if (pastName != tm.name)
                        {
                            comboBoxMaterialName.Items.Add(tm.name);
                        }
                        pastName = tm.name;
                    }
                }
                #endregion

                #region 产出
                //adapter = new MySqlDataAdapter();
                cmd.CommandText = "select id, name, level, price from product order by name asc, level asc;";
                adapter.SelectCommand = cmd;
                dt = new DataTable();
                adapter.Fill(dt);
                conn.Close();
                if (dt.Rows.Count > 0)
                {
                    tableProduct = new ArrayList();
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        TableMaterial tm = new TableMaterial();
                        tm.id = int.Parse(dt.Rows[i]["id"].ToString());
                        tm.name = dt.Rows[i]["name"].ToString();
                        tm.level = dt.Rows[i]["level"].ToString();
                        tableProduct.Add(tm);

                        if (pastName != tm.name)
                        {
                            comboBoxProductName.Items.Add(tm.name);
                        }
                        pastName = tm.name;
                    }
                }
                #endregion
            }
            catch (MySqlException sqlEx)
            {
                WriteLog(sqlEx.ToString());
                NotifyHelper.NotifyUser("无法连接服务器");
            }
            catch (Exception ex)
            {
                WriteLog(ex.ToString());
                NotifyHelper.NotifyUser("获取数据失败");
            }
            finally
            {
                if (conn != null)
                {
                    conn.Close();
                }
            }
        }

        private void buttonReset_Click(object sender, EventArgs e)
        {
            // 先询问
            if (MessageBox.Show("确定要重置", "(#-#)", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
            {
                Reset();
            }
        }

        private void Reset()
        {
            textBoxCounter.Text = string.Empty;
            DateTime present = DateTime.Now;
            textBoxBeginTime.Text = present.ToString("yyyyMMdd ");
            textBoxEndTime.Text = present.ToString("yyyyMMdd ");
            comboBoxOperator.SelectedIndex = -1;

            listViewMaterialCost.Items.Clear();
            listViewProductOut.Items.Clear();
            textBoxMaterialQuantity.Text = string.Empty;
            textBoxProductQuantity.Text = string.Empty;

            comboBoxProductLevel.SelectedIndex = -1;
            comboBoxMaterialLevel.SelectedIndex = -1;
        }

        private void eraseTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            eraseTimer.Stop();
            EraseLabelComitOk();
        }

        delegate void EraseLabelDelegate();

        private void EraseLabelComitOk()
        {
            if (labelCommitOK.InvokeRequired)
            {
                EraseLabelDelegate elc = new EraseLabelDelegate(EraseLabelComitOk);
                this.Invoke(elc);
            }
            else
            {
                labelCommitOK.Text = string.Empty;
            }
        }

        private bool CheckAllItems()
        {
            textBoxCounter.Text = textBoxCounter.Text.Trim();
            textBoxBeginTime.Text = textBoxBeginTime.Text.Trim();
            textBoxEndTime.Text = textBoxEndTime.Text.Trim();

            if (string.IsNullOrEmpty(textBoxCounter.Text))
            {
                NotifyHelper.NotifyUser("工作台");
                return false;
            }
            if (comboBoxOperator.SelectedIndex < 0)
            {
                NotifyHelper.NotifyUser("操作员");
                return false;
            }
            DateTime tmpTime;
            if (string.IsNullOrEmpty(textBoxBeginTime.Text))
            {
                NotifyHelper.NotifyUser("开始时间");
                return false;
            }
            else
            {
                bool b=DateTime.TryParseExact(textBoxBeginTime.Text, "yyyyMMdd HHmm", null, 
                    System.Globalization.DateTimeStyles.None, out tmpTime);
                if(b==false)
                {
                    NotifyHelper.NotifyUser("开始时间");
                    return false;
                }
            }
            if (string.IsNullOrEmpty(textBoxEndTime.Text))
            {
                NotifyHelper.NotifyUser("结束时间");
                return false;
            }
            else
            {
                bool b = DateTime.TryParseExact(textBoxEndTime.Text, "yyyyMMdd HHmm", null,
                    System.Globalization.DateTimeStyles.None, out tmpTime);
                if (b == false)
                {
                    NotifyHelper.NotifyUser("结束时间");
                    return false;
                }
            }
            if (listViewMaterialCost.Items.Count == 0)
            {
                NotifyHelper.NotifyUser("原料消耗");
                return false;
            }
            if (listViewProductOut.Items.Count == 0)
            {
                NotifyHelper.NotifyUser("物品产出");
                return false;
            }
            return true;
        }

        private void buttonCommit_Click(object sender, EventArgs e)
        {
            // 先检查所有的项都有值
            if (CheckAllItems() == false)
            {
                return;
            }
            MySqlConnection conn = null;
            MySqlTransaction tran = null;
            try
            {
                conn = MySqlConnHelper.GetMySqlConn(Properties.Settings.Default.DbConn);
                conn.Open();
                tran = conn.BeginTransaction();
                MySqlCommand comm = null;
                #region 基本信息
                comm = conn.CreateCommand();
                // mysql 日期格式参见 http://dev.mysql.com/doc/refman/5.1/en/date-and-time-functions.html#function_str-to-date
                comm.CommandText = @"insert into operation_detail(counter, emp_id, begin_time, 
end_time) 
values(?counter, ?emp_id, str_to_date(?begin_time, '%Y%m%d %H%i'), 
str_to_date(?end_time, '%Y%m%d %H%i') )";
                comm.Parameters.Clear();
                comm.Parameters.AddWithValue("?counter", textBoxCounter.Text);
                ListItem li = (comboBoxOperator.SelectedItem as ListItem);
                comm.Parameters.AddWithValue("?emp_id", li.ID);
                comm.Parameters.AddWithValue("?begin_time", textBoxBeginTime.Text);
                comm.Parameters.AddWithValue("?end_time", textBoxEndTime.Text);
                comm.Transaction = tran;
                #region log
                StringBuilder sb = new StringBuilder(512);
                sb.AppendFormat("C operation_detail ");
                sb.AppendFormat("{0}=[{1}], ", "counter", textBoxCounter.Text);
                sb.AppendFormat("{0}=[{1}], ", "emp_id", li.ID);
                sb.AppendFormat("{0}=[{1}], ", "begin_time", textBoxBeginTime.Text);
                sb.AppendFormat("{0}=[{1}]", "end_time", textBoxEndTime.Text);
                WriteLog(sb.ToString());
                #endregion
                comm.ExecuteNonQuery();
                #endregion
                comm = conn.CreateCommand();
#warning 设置一个断点，进行测试
                comm.CommandText = "select max(id) from operation_detail";
                string operationID = comm.ExecuteScalar().ToString();
                #region 原料消耗
                for (int i = 0; i < listViewMaterialCost.Items.Count; i++)
                {
                    comm = conn.CreateCommand();
                    comm.CommandText = @"insert into material_cost
(material_id, quantity, operation_id)
values(?material_id, ?quantity, ?operation_id)";
                    comm.Parameters.Clear();
                    string material_id;
                    material_id = listViewMaterialCost.Items[i].SubItems[3].Text;
                    comm.Parameters.AddWithValue("?material_id", material_id);
                    string quantity;
                    quantity = listViewMaterialCost.Items[i].SubItems[2].Text;
                    comm.Parameters.AddWithValue("?quantity", quantity);
                    comm.Parameters.AddWithValue("?operation_id", operationID);
                    #region log
                    sb = new StringBuilder(512);
                    sb.AppendFormat("C material_cost ");
                    sb.AppendFormat("{0}=[{1}], ", "material_id", material_id);
                    sb.AppendFormat("{0}=[{1}], ", "quantity", quantity);
                    sb.AppendFormat("{0}=[{1}], ", "operation_id", operationID);
                    WriteLog(sb.ToString());
                    #endregion
                    comm.ExecuteNonQuery();
                }
                #endregion
                #region 产品产出
                for (int i = 0; i < listViewProductOut.Items.Count; i++)
                {
                    comm = conn.CreateCommand();
                    comm.CommandText = @"insert into product_out
(product_id, quantity, operation_id)
values(?product_id, ?quantity, ?operation_id)";
                    comm.Parameters.Clear();
                    string product_id;
                    product_id = listViewProductOut.Items[i].SubItems[3].Text;
                    comm.Parameters.AddWithValue("?product_id", product_id);
                    string quantity;
                    quantity = listViewProductOut.Items[i].SubItems[2].Text;
                    comm.Parameters.AddWithValue("?quantity", quantity);
                    comm.Parameters.AddWithValue("?operation_id", operationID);
                    #region log
                    sb = new StringBuilder(512);
                    sb.AppendFormat("C product_out ");
                    sb.AppendFormat("{0}=[{1}], ", "product_id", product_id);
                    sb.AppendFormat("{0}=[{1}], ", "quantity", quantity);
                    sb.AppendFormat("{0}=[{1}], ", "operation_id", operationID);
                    WriteLog(sb.ToString());
                    #endregion
                    comm.ExecuteNonQuery();
                }
                #endregion
                tran.Commit();
                WriteLog("transaction done");

                Reset();
                labelCommitOK.Text = "提交成功";
                labelCommitOK.ForeColor = Color.DodgerBlue;
                eraseTimer.Start();
            }
            catch (MySqlException sqlEx)
            {
                if (tran != null)
                {
                    tran.Rollback();
                }

                WriteLog(sqlEx.ToString());
                NotifyHelper.NotifyUser("无法连接服务器");
                return;
            }
            catch (Exception ex)
            {
                if (tran != null)
                {
                    tran.Rollback();
                }

                WriteLog(ex.ToString());
                NotifyHelper.NotifyUser("访问服务器出错");
                return;
            }
            finally
            {
                if (conn != null)
                {
                    conn.Close();
                }
            }

        }
        #endregion

        #region 原料消耗
        private void comboBoxMaterialName_SelectedIndexChanged(object sender, EventArgs e)
        {
            comboBoxMaterialLevel.Items.Clear();
            if (comboBoxMaterialName.SelectedIndex < 0)
            {
                return;
            }
            if (tableMaterial != null)
            {
                for (int i = 0; i < tableMaterial.Count; i++)
                {
                    if (((TableMaterial)tableMaterial[i]).name == comboBoxMaterialName.SelectedItem.ToString())
                    {
                        ListItem mt = new ListItem();
                        mt.ID = ((TableMaterial)tableMaterial[i]).id.ToString();
                        mt.Name = ((TableMaterial)tableMaterial[i]).level;
                        comboBoxMaterialLevel.Items.Add(mt);
                    }
                }
                comboBoxMaterialLevel.DisplayMember = "Name";
                comboBoxMaterialLevel.ValueMember = "ID";
            }
        }

        /// <summary>
        /// 检查原料消耗的各个必填项
        /// </summary>
        /// <returns></returns>

        private bool CheckMaterialItems()
        {
            string prefix = "原料消耗->";
            decimal tmp = 0M;
            textBoxMaterialQuantity.Text = textBoxMaterialQuantity.Text.Trim();
            if (comboBoxMaterialName.SelectedIndex < 0)
            {
                NotifyHelper.NotifyUser(string.Format("{0}物品名称", prefix));
                return false;
            }
            else if (comboBoxMaterialLevel.SelectedIndex < 0)
            {
                NotifyHelper.NotifyUser(string.Format("{0}物品等级", prefix));
                return false;
            }
            else if (textBoxMaterialQuantity.Text == string.Empty || decimal.TryParse(textBoxMaterialQuantity.Text, out tmp) == false)
            {
                NotifyHelper.NotifyUser(string.Format("{0}数量", prefix));
                return false;
            }
            return true;
        }

        private void buttonAddMaterial_Click(object sender, EventArgs e)
        {
            if(CheckMaterialItems()==false)
            {
                return;
            }

            // 加入到列表中
            string[] tmp = new string[4];
            tmp[0] = comboBoxMaterialName.SelectedItem.ToString();
            tmp[1] = comboBoxMaterialLevel.SelectedItem.ToString();
            tmp[2] = textBoxMaterialQuantity.Text;
            tmp[3] = (comboBoxMaterialLevel.SelectedItem as ListItem).ID;
            ListViewItem item = new ListViewItem(tmp);
            listViewMaterialCost.Items.Add(item);

            // 清空输入框
            textBoxMaterialQuantity.Text = string.Empty;
            comboBoxMaterialLevel.SelectedIndex = -1;
        }

        private void buttonDelMaterial_Click(object sender, EventArgs e)
        {
            if (listViewMaterialCost.SelectedItems.Count == 0)
            {
                MessageBox.Show("尚未选中任何项", "(#-#)",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                if (MessageBox.Show("确认要删除选中项", "(#-#)", 
                    MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
                {
                    listViewMaterialCost.SelectedItems[0].Remove();
                }
            }
        }

        private void buttonClearMaterialList_Click(object sender, EventArgs e)
        {
            if (listViewMaterialCost.Items.Count == 0)
            {
                
            }
            else
            {
                if (MessageBox.Show("确认要清空<原料消耗>列表", "!*!", MessageBoxButtons.OKCancel) == DialogResult.OK)
                {
                    listViewMaterialCost.Items.Clear();
                }
            }
        }
        #endregion

        #region 物品产出
        private void comboBoxProductName_SelectedIndexChanged(object sender, EventArgs e)
        {
            comboBoxProductLevel.Items.Clear();
            if (comboBoxProductName.SelectedIndex < 0)
            {
                return;
            }
            if (tableProduct != null)
            {
                for (int i = 0; i < tableProduct.Count; i++)
                {
                    if (((TableMaterial)tableProduct[i]).name == comboBoxProductName.SelectedItem.ToString())
                    {
                        ListItem mt = new ListItem();
                        mt.ID = ((TableMaterial)tableProduct[i]).id.ToString();
                        mt.Name = ((TableMaterial)tableProduct[i]).level;
                        comboBoxProductLevel.Items.Add(mt);
                    }
                }
                comboBoxProductLevel.DisplayMember = "Name";
                comboBoxProductLevel.ValueMember = "ID";
            }
        }

        private bool CheckProductItems()
        {
            string prefix = "物品产出->";
            decimal tmp = 0M;
            textBoxProductQuantity.Text = textBoxProductQuantity.Text.Trim();
            if (comboBoxProductName.SelectedIndex < 0)
            {
                NotifyHelper.NotifyUser(string.Format("{0}物品名称", prefix));
                return false;
            }
            else if (comboBoxProductLevel.SelectedIndex < 0)
            {
                NotifyHelper.NotifyUser(string.Format("{0}物品等级", prefix));
                return false;
            }
            else if (textBoxProductQuantity.Text == string.Empty || decimal.TryParse(textBoxProductQuantity.Text, out tmp) == false)
            {
                NotifyHelper.NotifyUser(string.Format("{0}数量", prefix));
                return false;
            }
            return true;
        }

        private void buttonAddProduct_Click(object sender, EventArgs e)
        {
            if(CheckProductItems()==false)
            {
                return;
            }
            // 加入到列表中
            string[] tmp = new string[4];
            tmp[0] = comboBoxProductName.SelectedItem.ToString();
            tmp[1] = comboBoxProductLevel.SelectedItem.ToString();
            tmp[2] = textBoxProductQuantity.Text;
            tmp[3] = (comboBoxProductLevel.SelectedItem as ListItem).ID;
            ListViewItem item = new ListViewItem(tmp);
            listViewProductOut.Items.Add(item);

            // 清空输入框
            textBoxProductQuantity.Text = string.Empty;
            comboBoxProductLevel.SelectedIndex = -1;
        }

        private void buttonDelProduct_Click(object sender, EventArgs e)
        {
            if(listViewProductOut.SelectedItems.Count==0)
            {
                MessageBox.Show("尚未选中任何项", "(#-#)", 
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                if (MessageBox.Show("确认要删除选中项", "(#-#)", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
                {
                    listViewProductOut.SelectedItems[0].Remove();
                }
            }
        }

        private void buttonClearProductList_Click(object sender, EventArgs e)
        {
            if (listViewProductOut.Items.Count == 0)
            {

            }
            else
            {
                if (MessageBox.Show("确认要清空<物品产出>列表", "!*!", MessageBoxButtons.OKCancel) == DialogResult.OK)
                {
                    listViewProductOut.Items.Clear();
                }
            }
        }
        #endregion

        public void WriteLog(string s)
        {
            if (logHelper != null)
            {
                try
                {
                    logHelper.WriteLog(s);
                }
                catch (Exception ex)
                {
                    Console.Write(ex.Message);
                }
            }
        }

        private void ProduceForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            string s = string.Format("{0} exits, version={1}",
                Application.ProductName, Application.ProductVersion);
            WriteLog(s);
        }
    }

}