using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using System.Text.RegularExpressions;

using MySql.Data.MySqlClient;

namespace zjzl
{
    public partial class PurchaseForm : Form
    {
        private readonly string[] seperator ={ ";" };  // ���֧�ֱ���ģʽ����������
        private ArrayList tableMaterial = null;   // ��Ʒ��
        private string employeeID = "0";
        /// <summary>
        /// �ͻ����
        /// </summary>
        private int personID = -1;
        private LogHelper logHelper = null;

        public PurchaseForm()
        {
            InitializeComponent();
        }

        private void PurchaseForm_Load(object sender, EventArgs e)
        {
            this.Hide();

            try
            {
                logHelper = new LogHelper(Products.purchase);
            }
            catch (Exception ex)
            {
                Console.Write(ex);
                NotifyHelper.NotifyUser("��ʼ����־ģ��ʧ��, \r\n�����Կ�ʹ�ã�������¼��־");
            }

            string s = string.Format("{0} starts, version={1}",
                Application.ProductName, Application.ProductVersion);
            WriteLog(s);

            try
            {
                LoginForm l = new LoginForm(Properties.Settings.Default.DbConn, Products.purchase);
                if (l.ShowDialog() == DialogResult.OK)
                {
                    employeeID = l.EmpId;
                    this.Show();
                    Init();
                }
                else
                {
                    NotifyHelper.NotifyUser("��¼ʧ��, �����˳�");
                    this.Close();
                }
            }
            catch(Exception ex)
            {
                WriteLog(ex.ToString());
            }
       }

        private void PurchaseForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            string s = string.Format("{0} exits, version={1}",
                Application.ProductName, Application.ProductVersion);
            WriteLog(s);
        }

        #region �����Դ���

        //private void button1_Click(object sender, EventArgs e)
        //{
        //    MySqlConnectionStringBuilder connBuilder =
        //        new MySqlConnectionStringBuilder();

        //    connBuilder.Add("Database", "zjzl");
        //    connBuilder.Add("Data Source", "localhost");
        //    connBuilder.Add("User Id", "root");
        //    connBuilder.Add("Password", "123");
        //    //connBuilder.Add("Charset", "utf8");

        //    MySqlConnection connection =
        //        new MySqlConnection(connBuilder.ConnectionString);

        //    try
        //    {
        //        MySqlCommand cmd = connection.CreateCommand();
        //        cmd.CommandType = CommandType.Text;
        //        cmd.CommandText = "select id, name, level, price from material;";

        //        connection.Open();
        //        MySqlDataReader dr = cmd.ExecuteReader();
        //        while (dr.Read())
        //        {
        //            comboBoxMaterialName.Items.Add(dr["name"].ToString());
        //            comboBoxMaterialLevel.Items.Add(dr["level"].ToString());
        //            //com
        //        }
        //        dr.Close();
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show(ex.ToString());
        //    }
        //    finally
        //    {
        //        if (connection != null)
        //        {
        //            connection.Close();
        //        }
        //    }
        //}

        //private void button2_Click(object sender, EventArgs e)
        //{
        //    MySqlConnection connection = MySqlConnHelper.GetMySqlConn(Properties.Settings.Default.DbConn);

        //    try
        //    {
        //        MySqlCommand cmd = connection.CreateCommand();
        //        cmd.CommandType = CommandType.Text;
        //        cmd.CommandText = "insert into material(name, level, price) values(@name, @level, 0.05);";
        //        cmd.Parameters.Clear();
        //        cmd.Parameters.AddWithValue("@name", "�Ͼ�����");
        //        cmd.Parameters.AddWithValue("@level", "5");

        //        connection.Open();
        //        cmd.ExecuteNonQuery();
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show(ex.ToString());
        //    }
        //    finally
        //    {
        //        if (connection != null)
        //        {
        //            connection.Close();
        //        }
        //    }
        //}
        #endregion

        private void Init()
        {
            #region ������λ/Units
            //string s = Properties.Settings.Default["Units"].ToString();
            //if (!string.IsNullOrEmpty(s))
            //{
            //    string[] ss = s.Split(seperator, StringSplitOptions.RemoveEmptyEntries);
            //    if (ss != null)
            //    {
            //        comboBoxUnits.Items.AddRange(ss);
            //        comboBoxUnits.SelectedIndex = 0;
            //    }
            //}
            #endregion

            MySqlConnection conn = null;
            try
            {
                conn = MySqlConnHelper.GetMySqlConn(Properties.Settings.Default.DbConn);
                MySqlDataAdapter adapter = new MySqlDataAdapter();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "select id, name, level, price from material order by name asc, level asc;";
                adapter.SelectCommand = cmd;
                DataTable dt = new DataTable();
                conn.Open();
                adapter.Fill(dt);
                conn.Close();
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
                        tm.price = decimal.Parse(dt.Rows[i]["price"].ToString());
                        tableMaterial.Add(tm);

                        if (pastName != tm.name)
                        {
                            comboBoxMaterialName.Items.Add(tm.name);
                        }
                        pastName = tm.name;
                    }
                }
            }
            catch (MySqlException sqlEx)
            {
                WriteLog(sqlEx.ToString());
                NotifyHelper.NotifyUser("�޷����ӷ�����");
            }
            catch (Exception ex)
            {
                WriteLog(ex.ToString());
                NotifyHelper.NotifyUser("��ȡ����ʧ��");
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
            // ��յ��ۺͽ��
            textBoxPrice.Text = string.Empty;
            textBoxCash.Text = string.Empty;
            // ����fill �����б�comboBoxMaterialLevel��ѡ��
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
                    }
                }
                comboBoxMaterialLevel.DisplayMember = "Name";
                comboBoxMaterialLevel.ValueMember = "ID";
            }
        }

        private void comboBoxMaterialLevel_SelectedIndexChanged(object sender, EventArgs e)
        {
            // ��յ��ۺͽ��
            textBoxPrice.Text = string.Empty;
            textBoxCash.Text = string.Empty;
            // ��д����
            if (comboBoxMaterialLevel.SelectedIndex < 0)
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
                        break;
                    }
                }
            }
        }

        private void buttonReset_Click(object sender, EventArgs e)
        {
            // ��ѯ��
            if (MessageBox.Show("ȷ��Ҫ����", "(#-#)", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
            {
                Reset();
            }
        }

        /// <summary>
        /// ���������
        /// </summary>
        private void Reset()
        {
            textBoxCustomerTag.Text = string.Empty;
            textBoxQuantity.Text = string.Empty;
            textBoxPrice.Text = string.Empty;
            textBoxCash.Text = string.Empty;
            personID = -1;
        }

        private void buttonOk_Click(object sender, EventArgs e)
        {
            // �ȼ�����е����ֵ
            if (CheckItems() == false)
            {
                return;
            }
            FillCash();
            MySqlConnection conn = null;
            MySqlTransaction tran = null;
            try
            {
            // д�����ݿ�
                conn = MySqlConnHelper.GetMySqlConn(Properties.Settings.Default.DbConn);
                conn.Open();
                tran = conn.BeginTransaction();

                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = @"insert into pur_detail(counter, emp_id, deal_time, material_id, quantity, customer_id, price)  
 values(?counter, ?emp_id, now(), ?material_id, ?quantity, ?customer_id, ?price);";
                cmd.Parameters.Clear();
                //cmd.Parameters.AddWithValue("?factory_id", facotry_id);
                cmd.Parameters.AddWithValue("?counter", Properties.Settings.Default.CounterName);
                cmd.Parameters.AddWithValue("?emp_id", employeeID); 
                cmd.Parameters.AddWithValue("?material_id", (comboBoxMaterialLevel.SelectedItem as ListItem).ID);
                cmd.Parameters.AddWithValue("?quantity", textBoxQuantity.Text);
                cmd.Parameters.AddWithValue("?customer_id", personID);
                cmd.Parameters.AddWithValue("?price", textBoxPrice.Text);
#region log
                StringBuilder sb = new StringBuilder(512);
                sb.AppendFormat("C pur_detail {0}=[{1}], ", "counter", Properties.Settings.Default.CounterName);
                sb.AppendFormat("{0}=[{1}], ", "emp_id", employeeID);
                sb.AppendFormat("{0}=[{1}], ", "material_id", (comboBoxMaterialLevel.SelectedItem as ListItem).ID);
                sb.AppendFormat("{0}=[{1}], ", "quantity", textBoxQuantity.Text);
                sb.AppendFormat("{0}=[{1}], ", "customer_id", personID);
                sb.AppendFormat("{0}=[{1}]", "price", textBoxPrice.Text);
                WriteLog(sb.ToString());
#endregion
                cmd.ExecuteNonQuery();

                cmd = conn.CreateCommand();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "update pur_customer set upper_used = upper_used+" + textBoxQuantity.Text+" where id="+personID;
                cmd.Parameters.Clear();

                string s = string.Format("U pur_customer upper_used=[{0}]", textBoxQuantity.Text);
                WriteLog(s);

                cmd.ExecuteNonQuery();
                tran.Commit();

                WriteLog("transaction done");

                // ���뵽�б���
                string[] tmp = new string[6];
                tmp[0] = textBoxCustomerTag.Text;
                tmp[1] = comboBoxMaterialName.SelectedItem.ToString();
                tmp[2] = comboBoxMaterialLevel.SelectedItem.ToString();
                tmp[3] = textBoxPrice.Text;
                tmp[4] = textBoxQuantity.Text;
                tmp[5] = textBoxCash.Text;
                ListViewItem item = new ListViewItem(tmp);
                if (listView1.Items.Count > 4)
                {
                    listView1.Items.RemoveAt(listView1.Items.Count - 1);
                }
                listView1.Items.Add(item);

                // ��������
                Reset();
                textBoxCustomerTag.Focus();
            }
            catch (MySqlException sqlEx)
            {
                if (tran != null)
                {
                    tran.Rollback();
                }

                WriteLog(sqlEx.ToString());
                NotifyHelper.NotifyUser("�޷����ӷ�����");
                return;
            }
            catch (Exception ex)
            {
                if (tran != null)
                {
                    tran.Rollback();
                }
                WriteLog(ex.ToString());
                NotifyHelper.NotifyUser("���ʷ���������");
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
        /// ������������
        /// </summary>
        /// <returns></returns>
        private bool CheckItems()
        {
            //if (textBoxCustomerTag.Text.Trim() == string.Empty)
            //{
            //    NotifyUser("��Ա���");
            //    return false;
            //}

            decimal tmp = 0M;
            if (personID < 0)
            {
                NotifyHelper.NotifyUser("��Ա���");
                return false;
            }
            else if (comboBoxMaterialName.SelectedIndex < 0)
            {
                NotifyHelper.NotifyUser("��Ʒ����");
                return false;
            }
            else if (comboBoxMaterialLevel.SelectedIndex < 0)
            {
                NotifyHelper.NotifyUser("��Ʒ�ȼ�");
                return false;
            }
            else if (textBoxPrice.Text == string.Empty || decimal.TryParse(textBoxPrice.Text, out tmp) == false)
            {
                NotifyHelper.NotifyUser("����");
                return false;
            }
            else if (textBoxQuantity.Text == string.Empty || decimal.TryParse(textBoxQuantity.Text, out tmp) == false)
            {
                NotifyHelper.NotifyUser("����");
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

        private void buttonMore_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBoxCustomerTag.Text))
            {
                return;
            }
            InfoForm info = new InfoForm(textBoxCustomerTag.Text, this);
            info.ShowDialog();
            personID = info.personID;
        }

    }

}