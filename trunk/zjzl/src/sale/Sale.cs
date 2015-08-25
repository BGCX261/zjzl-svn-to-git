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
    public partial class SaleForm : Form
    {
        private readonly string[] seperator ={ ";" };  // 如果支持本地模式，才起作用
        /// <summary>
        /// 物品表
        /// </summary>
        private ArrayList tableMaterial = null;
        /// <summary>
        /// 客户表
        /// </summary>
        //private ArrayList customers = null;
        /// <summary>
        /// 定时器，用于隐藏“提交成功”的文字提示
        /// </summary>
        System.Timers.Timer eraseTimer = new System.Timers.Timer();
        LogHelper logHelper;

        private string employeeID = "0";

        public SaleForm()
        {
            InitializeComponent();
        }

        private void SaleForm_Load(object sender, EventArgs e)
        {
            this.Hide();

            try
            {
                logHelper = new LogHelper(Products.sale);
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
                LoginForm l = new LoginForm(Properties.Settings.Default.DbConn, Products.sale);
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
                eraseTimer.Interval = 2000;
                eraseTimer.Elapsed += new ElapsedEventHandler(eraseTimer_Elapsed);
                conn = MySqlConnHelper.GetMySqlConn(Properties.Settings.Default.DbConn);
                MySqlDataAdapter adapter = new MySqlDataAdapter();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "select id, name, level, price from product order by name asc, level asc;";
                adapter.SelectCommand = cmd;
                DataTable dt = new DataTable();
                conn.Open();
                adapter.Fill(dt);
                #region 物品
                if (dt.Rows.Count > 0)
                {
                    tableMaterial = new ArrayList();
                    string pastName = null;
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        TableMaterial tm = new TableMaterial();
                        tm.id = int.Parse(dt.Rows[i]["id"].ToString());
                        tm.name = dt.Rows[i]["name"].ToString();
                        tm.level = dt.Rows[i]["level"].ToString();
                        decimal tmpPrice = 0.0M;
                        if (decimal.TryParse(dt.Rows[i]["price"].ToString(), out tmpPrice))
                        {
                            tm.price = tmpPrice;
                        }

                        tableMaterial.Add(tm);

                        if (pastName != tm.name)
                        {
                            comboBoxMaterialName.Items.Add(tm.name);
                        }
                        pastName = tm.name;
                    }
                }
                #endregion

                cmd.CommandText = "select id, name from sale_customer order by id asc";
                adapter.SelectCommand = cmd;
                dt = new DataTable();
                adapter.Fill(dt);
                conn.Close();
                if(dt.Rows.Count>0)
                {
                    //customers = new ArrayList();
                    for (int j = 0; j < dt.Rows.Count;j++ )
                    {
                        SaleCustomer saleCustomer = new SaleCustomer();
                        saleCustomer.id = int.Parse(dt.Rows[j]["id"].ToString());
                        saleCustomer.name = dt.Rows[j]["name"].ToString();

                        comboBoxCorpName.Items.Add(saleCustomer);
                    }
                    comboBoxCorpName.DisplayMember = "name";
                    comboBoxCorpName.ValueMember = "id";
               }
            }
            catch (MySqlException sqlEx)
            {
                WriteLog(sqlEx.ToString());
                NotifyHelper.NotifyUser("无法连接服务器: " + sqlEx.Message);
            }
            catch (Exception ex)
            {
                WriteLog(ex.ToString());
                NotifyHelper.NotifyUser("获取数据失败: " + ex.Message);
            }
            finally
            {
                if (conn != null)
                {
                    conn.Close();
                }
            }
        }

        private void comboBoxMaterialName_SelectedIndexChanged(object sender, EventArgs e)
        {
            // 清空单价和金额
            textBoxPrice.Text = string.Empty;
            textBoxCash.Text = string.Empty;
            // 重新fill 下列列表comboBoxMaterialLevel的选项
            comboBoxMaterialLevel.Items.Clear();
            if(comboBoxMaterialName.SelectedIndex<0)
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
                        comboBoxMaterialLevel.DisplayMember = "Name";
                        comboBoxMaterialLevel.ValueMember = "ID";
                        //comboBoxMaterialLevel.Items.Add(((TableMaterial)tableMaterial[i]).level);
                    }
                }
            }
        }

        private void comboBoxMaterialLevel_SelectedIndexChanged(object sender, EventArgs e)
        {
            // 清空单价和金额
            textBoxPrice.Text = string.Empty;
            textBoxCash.Text = string.Empty;
            // 填写单价
            if(comboBoxMaterialLevel.SelectedIndex<0)
            {
                return;
            }
            if (tableMaterial != null)
            {
                for (int i = 0; i < tableMaterial.Count; i++)
                {
                    if (((TableMaterial)tableMaterial[i]).name == comboBoxMaterialName.SelectedItem.ToString()
                        && ((TableMaterial)tableMaterial[i]).level.ToString() == comboBoxMaterialLevel.SelectedItem.ToString())
                    {
                        textBoxPrice.Text = ((TableMaterial)tableMaterial[i]).price.ToString();
                        //textBoxMaterialID.Text = ((TableMaterial)tableMaterial[i]).id.ToString();
                        break;
                    }
                }
            }
        }

        private void buttonReset_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("确定要重置", "(#-#)", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
            {
                Reset();
            }
        }

        private void Reset()
        {
            textBoxQuantity.Text = string.Empty;
            textBoxPrice.Text = string.Empty;
            textBoxCash.Text = string.Empty;

            comboBoxCorpName.SelectedIndex = -1;
            comboBoxMaterialLevel.SelectedIndex = -1;
            // 焦点跳到物品
            comboBoxMaterialName.Focus();
        }

        private void buttonCommit_Click(object sender, EventArgs e)
        {
            if(CheckItems()==false)
            {
                return;
            }

            // 写入数据库
            MySqlConnection conn = null;
            MySqlTransaction tran = null;
            try
            {
                conn = MySqlConnHelper.GetMySqlConn(Properties.Settings.Default.DbConn);
                conn.Open();
                tran = conn.BeginTransaction();
                    MySqlCommand comm = conn.CreateCommand();
                    comm.CommandText = @"insert into sale_detail(counter, emp_id, deal_time, 
product_id, quantity, customer_id, price) 
values(?counter, ?emp_id, now(), ?product_id, ?quantity, ?customer_id, ?price)";
                    comm.Parameters.Clear();
                    comm.Parameters.AddWithValue("?counter", Properties.Settings.Default.CounterName); 
                    comm.Parameters.AddWithValue("?emp_id", employeeID);
                    string product_id;
                    product_id = (comboBoxMaterialLevel.SelectedItem as ListItem).ID.ToString(); 
                    comm.Parameters.AddWithValue("?product_id", product_id);
                    string quantity;
                    quantity = textBoxQuantity.Text;
                    comm.Parameters.AddWithValue("?quantity", quantity);
                    string customer_id;
                    customer_id = (comboBoxCorpName.SelectedItem as SaleCustomer).id.ToString();
                    comm.Parameters.AddWithValue("?customer_id", customer_id);
                    string price;
                    price = textBoxPrice.Text;
                    comm.Parameters.AddWithValue("?price", price);
                    comm.Transaction = tran;
                    #region log
                    StringBuilder sb = new StringBuilder(512);
                    sb.AppendFormat("C sale_detail {0}=[{1}], ", "counter", Properties.Settings.Default.CounterName);
                    sb.AppendFormat("{0}=[{1}], ", "emp_id", employeeID);
                    sb.AppendFormat("{0}=[{1}], ", "product_id", product_id);
                    sb.AppendFormat("{0}=[{1}], ", "quantity", quantity);
                    sb.AppendFormat("{0}=[{1}], ", "customer_id", customer_id);
                    sb.AppendFormat("{0}=[{1}]", "price", price);
                    WriteLog(sb.ToString());
                    #endregion                    
                    comm.ExecuteNonQuery();

                tran.Commit();
                WriteLog("transaction done");

                Reset();
                labelCommitOK.Text = "提交成功";
                labelCommitOK.ForeColor = Color.DodgerBlue;
                eraseTimer.Start();
            }
            catch (MySqlException sqlEx)
            {
                if(tran!=null)
                {
                    tran.Rollback();
                }
                WriteLog(sqlEx.ToString());
                NotifyHelper.NotifyUser("无法连接服务器");
                return;
            }
            catch (Exception ex)
            {
                if(tran!=null)
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

        /// <summary>
        /// 检查各个必填项
        /// </summary>
        /// <returns></returns>
        private bool CheckItems()
        {
           decimal tmp = 0M;
           if (comboBoxMaterialName.SelectedIndex < 0)
            {
                NotifyHelper.NotifyUser("物品名称");
                return false;
            }
            else if (comboBoxMaterialLevel.SelectedIndex < 0)
            {
                NotifyHelper.NotifyUser("物品等级");
                return false;
            }
            else if (textBoxPrice.Text == string.Empty || decimal.TryParse(textBoxPrice.Text, out tmp)==false)
            {
                NotifyHelper.NotifyUser("单价");
                return false;
            }
            else if (textBoxQuantity.Text == string.Empty || decimal.TryParse(textBoxQuantity.Text, out tmp) == false)
            {
                NotifyHelper.NotifyUser("数量");
                return false;
            }
            else if (comboBoxCorpName.SelectedIndex < 0)
            {
            // 检查客户名称
                NotifyHelper.NotifyUser("客户名称");
                return false;
            }

            return true;
        }

        private void textBoxPrice_Leave(object sender, EventArgs e)
        {
            textBoxPrice.Text = textBoxPrice.Text.Trim();
            if (!string.IsNullOrEmpty(textBoxPrice.Text))
            {
                FillCash();
            }
        }

        private void textBoxQuantity_Leave(object sender, EventArgs e)
        {
            textBoxQuantity.Text = textBoxQuantity.Text.Trim();
            if (!string.IsNullOrEmpty(textBoxQuantity.Text))
            {
                FillCash();
            }
        }

        private void FillCash()
        {
            decimal price = 0, quantity = 0;
            if (decimal.TryParse(textBoxPrice.Text, out price) == false)
            {
                return;
            }
            if (decimal.TryParse(textBoxQuantity.Text, out quantity) == false)
            {
                return;
            }

            textBoxCash.Text = (price * quantity).ToString();
        }

        private void eraseTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            eraseTimer.Stop();
            EraseLabelComitOk();
        }

        delegate void EraseLabelDelegate();

        private void EraseLabelComitOk()
        {
            if(labelCommitOK.InvokeRequired)
            {
                EraseLabelDelegate elc = new EraseLabelDelegate(EraseLabelComitOk);
                this.Invoke(elc);
            }
            else
            {
            labelCommitOK.Text = string.Empty;
            }
        }

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

        private void SaleForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            string s = string.Format("{0} exits, version={1}",
                Application.ProductName, Application.ProductVersion);
            WriteLog(s);
        }
    }

    public class SaleCustomer
    {
        public int id;
        public string name;

        public override string ToString()
        {
            return this.name;
        }
    }
}