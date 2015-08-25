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
using RKLib.ExportData;

namespace zjzl
{
    public partial class ManageForm : Form
    {
        private readonly string[] seperator ={ ";" };  // 如果支持本地模式，才起作用
        private string employeeID = "0";
        private LogHelper logHelper = null;

        public ManageForm()
        {
            InitializeComponent();
        }

        private void ManageForm_Load(object sender, EventArgs e)
        {
            this.Hide();

            try
            {
                logHelper = new LogHelper(Products.manage);
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
                LoginForm l = new LoginForm(Properties.Settings.Default.DbConn, Products.manage);
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
                NotifyHelper.NotifyUser(ex.Message);
            }
        }

        private void Init()
        {
            MySqlConnection conn = null;
            try
            {
                InitEmpTab();
            }
            //catch (MySqlException sqlEx)
            //{
            //    WriteLog(sqlEx.ToString());
            //    NotifyHelper.NotifyUser("操作失败: " + sqlEx.Message);
            //}
            catch (Exception ex)
            {
                WriteLog(ex.ToString());
                NotifyHelper.NotifyUser("操作失败: " + ex.Message);
            }
            finally
            {
                if (conn != null)
                {
                    conn.Close();
                }
            }
        }

        private void NotifyUser(string msg)
        {
            if (!string.IsNullOrEmpty(msg))
            {
                MessageBox.Show(msg, "(O-O)", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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

        private void ManageForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            string s = string.Format("{0} exits, version={1}",
                Application.ProductName, Application.ProductVersion);
            WriteLog(s);
        }

        #region material infomation
        private void InitMatTab()
        {
            RefreshMatList();
            ResetMat();
        }

        private void TrimAllMat()
        {
            textBoxMatID.Text = textBoxMatID.Text.Trim();
            textBoxMatRName.Text = textBoxMatRName.Text.Trim();
            textBoxMatName.Text = textBoxMatName.Text.Trim();
            textBoxMatLevel.Text = textBoxMatLevel.Text.Trim();
            textBoxMatPrice.Text = textBoxMatPrice.Text.Trim();
        }

        private void HandleMatEdit()
        {
            MySqlConnection conn = null;
            try
            {
                conn = MySqlConnHelper.GetMySqlConn(Properties.Settings.Default.DbConn);
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandType = CommandType.Text;
                #region
                cmd.CommandText = "update material set name=?name, rname=?rname, level=?level, price=?price where id=?id;";
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("?name", textBoxMatName.Text);
                cmd.Parameters.AddWithValue("?rname", textBoxMatRName.Text);
                cmd.Parameters.AddWithValue("?level", textBoxMatLevel.Text);
                cmd.Parameters.AddWithValue("?price", textBoxMatPrice.Text);
                string id = listViewMatInfo.SelectedItems[0].SubItems[0].Text;
                cmd.Parameters.AddWithValue("?id", id);
                conn.Open();
                cmd.ExecuteNonQuery();

                #region 记录操作日志
                StringBuilder sb = new StringBuilder(512);
                sb.AppendFormat("U material ");
                sb.AppendFormat("{0}=[{1}],", "name", textBoxMatRName.Text);
                sb.AppendFormat("{0}=[{1}],", "rname", textBoxMatName.Text);
                sb.AppendFormat("{0}=[{1}],", "level", textBoxMatLevel.Text);
                sb.AppendFormat("{0}=[{1}],", "price", textBoxMatPrice.Text);
                sb.AppendFormat("{0}=[{1}],", "id", id);
                WriteLog(sb.ToString());
                #endregion

                // 更新列表
                ResetMat();
                RefreshMatList();
                #endregion
            }
            catch (Exception ex)
            {
                WriteLog(ex.ToString());
                NotifyHelper.NotifyUser("操作失败: " + ex.Message);
            }
            finally
            {
                if (conn != null)
                {
                    conn.Close();
                }
            }
        }

        private void HandleMatAdd()
        {
            MySqlConnection conn = null;
            try
            {
                conn = MySqlConnHelper.GetMySqlConn(Properties.Settings.Default.DbConn);
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandType = CommandType.Text;
                #region
                cmd.CommandText = "insert into material(name, rname, level, price) values(?name, ?rname, ?level, ?price);";
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("?name", textBoxMatName.Text);
                cmd.Parameters.AddWithValue("?rname", textBoxMatRName.Text);
                cmd.Parameters.AddWithValue("?level", textBoxMatLevel.Text);
                cmd.Parameters.AddWithValue("?price", textBoxMatPrice.Text);
                conn.Open();

                #region log
                StringBuilder sb = new StringBuilder(512);
                sb.AppendFormat("C material ");
                sb.AppendFormat("{0}=[{1}],", "name", textBoxMatRName.Text);
                sb.AppendFormat("{0}=[{1}],", "rname", textBoxMatName.Text);
                sb.AppendFormat("{0}=[{1}],", "level", textBoxMatLevel.Text);
                sb.AppendFormat("{0}=[{1}],", "price", textBoxMatPrice.Text);
                WriteLog(sb.ToString());
                #endregion
                cmd.ExecuteNonQuery();

                // 更新列表
                ResetMat();
                RefreshMatList();
                #endregion
            }
            catch (Exception ex)
            {
                WriteLog(ex.ToString());
                NotifyHelper.NotifyUser("操作失败: " + ex.Message);
            }
            finally
            {
                if (conn != null)
                {
                    conn.Close();
                }
            }
        }

        private void ResetMat()
        {
            textBoxMatID.Text = string.Empty;
            textBoxMatRName.Text = string.Empty;
            textBoxMatName.Text = string.Empty;
            textBoxMatLevel.Text = string.Empty;
            textBoxMatPrice.Text = string.Empty;
        }

        private bool CheckMatItems()
        {
            //if (string.IsNullOrEmpty(textBoxMatID.Text))  // 添加操作
            {
                if (string.IsNullOrEmpty(textBoxMatRName.Text))
                {
                    NotifyHelper.NotifyUser("内部名称");
                    return false;
                }
            }
            if (string.IsNullOrEmpty(textBoxMatName.Text))
            {
                NotifyHelper.NotifyUser(label25.Text);
                return false;
            }
            if (string.IsNullOrEmpty(textBoxMatLevel.Text))
            {
                NotifyHelper.NotifyUser("等级");
                return false;
            }
            decimal tmpDec = 0m;
            if (string.IsNullOrEmpty(textBoxMatPrice.Text)
                || decimal.TryParse(textBoxMatPrice.Text, out tmpDec) == false)
            {
                NotifyHelper.NotifyUser("单价");
                return false;
            }
            return true;
        }

        private void buttonMatCommit_Click(object sender, EventArgs e)
        {
            TrimAllMat();
            if (CheckMatItems() == false)
            {
                return;
            }
            if (string.IsNullOrEmpty(textBoxMatID.Text))
            {
                HandleMatAdd();
            }
            else
            {
                HandleMatEdit();
            }
        }

        private void buttonMatRefresh_Click(object sender, EventArgs e)
        {
            RefreshMatList();
        }

        private void buttonMatReset_Click(object sender, EventArgs e)
        {
            ResetMat();
        }

        private void RefreshMatList()
        {
            listViewMatInfo.Items.Clear();
            MySqlConnection conn = null;
            try
            {
                conn = MySqlConnHelper.GetMySqlConn(Properties.Settings.Default.DbConn);
                MySqlDataAdapter adapter = new MySqlDataAdapter();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "select id, name, rname, level, price from material order by name, id asc;";
                adapter.SelectCommand = cmd;
                DataTable dt = new DataTable();
                conn.Open();
                adapter.Fill(dt);
                conn.Close();
                if (dt.Rows.Count > 0)
                {
                    if (dt.Rows.Count > 0)
                    {

                        listViewMatInfo.BeginUpdate();
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            ListViewItem item = new ListViewItem(dt.Rows[i]["id"].ToString());
                            item.SubItems.Add(dt.Rows[i]["rname"].ToString());
                            item.SubItems.Add(dt.Rows[i]["name"].ToString());
                            item.SubItems.Add(dt.Rows[i]["level"].ToString());
                            //string acl = dt.Rows[i]["level"].ToString();
                            //item.SubItems.Add(acl);
                            item.SubItems.Add(dt.Rows[i]["price"].ToString());

                            listViewMatInfo.Items.Add(item);
                        }
                        listViewMatInfo.EndUpdate();
                    }
                }
            }
            catch (Exception ex)
            {
                WriteLog(ex.ToString());
                NotifyHelper.NotifyUser("操作失败: " + ex.Message);
            }
            finally
            {
                if (conn != null)
                {
                    conn.Close();
                }
            }
        }

        private void listViewMatInfo_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            ResetMat();

            if (listViewMatInfo.SelectedIndices.Count > 0)
            {
                textBoxMatID.Text = listViewMatInfo.SelectedItems[0].Text;
                textBoxMatRName.Text = listViewMatInfo.SelectedItems[0].SubItems[1].Text;
                textBoxMatName.Text = listViewMatInfo.SelectedItems[0].SubItems[2].Text;
                textBoxMatLevel.Text = listViewMatInfo.SelectedItems[0].SubItems[3].Text;
                textBoxMatPrice.Text = listViewMatInfo.SelectedItems[0].SubItems[4].Text;
            }
        }

        #endregion

        #region product infomation
        private void InitProTab()
        {
            RefreshProList();
            ResetPro();
        }

        private void TrimAllPro()
        {
            textBoxProID.Text = textBoxProID.Text.Trim();
            textBoxProRName.Text = textBoxProRName.Text.Trim();
            textBoxProName.Text = textBoxProName.Text.Trim();
            textBoxProLevel.Text = textBoxProLevel.Text.Trim();
            textBoxProPrice.Text = textBoxProPrice.Text.Trim();
        }

        private void HandleProEdit()
        {
            MySqlConnection conn = null;
            try
            {
                conn = MySqlConnHelper.GetMySqlConn(Properties.Settings.Default.DbConn);
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandType = CommandType.Text;
                #region
                cmd.CommandText = "update product set name=?name, rname=?rname, level=?level, price=?price where id=?id;";
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("?name", textBoxProName.Text);
                cmd.Parameters.AddWithValue("?rname", textBoxProRName.Text);
                cmd.Parameters.AddWithValue("?level", textBoxProLevel.Text);
                cmd.Parameters.AddWithValue("?price", textBoxProPrice.Text);
                string id = listViewProInfo.SelectedItems[0].SubItems[0].Text;
                cmd.Parameters.AddWithValue("?id", id);
                conn.Open();
                cmd.ExecuteNonQuery();

                #region 记录操作日志
                StringBuilder sb = new StringBuilder(512);
                sb.AppendFormat("U product ");
                sb.AppendFormat("{0}=[{1}],", "name", textBoxProRName.Text);
                sb.AppendFormat("{0}=[{1}],", "rname", textBoxProName.Text);
                sb.AppendFormat("{0}=[{1}],", "level", textBoxProLevel.Text);
                sb.AppendFormat("{0}=[{1}],", "price", textBoxProPrice.Text);
                sb.AppendFormat("{0}=[{1}],", "id", id);
                WriteLog(sb.ToString());
                #endregion

                // 更新列表
                ResetPro();
                RefreshProList();
                #endregion
            }
            catch (Exception ex)
            {
                WriteLog(ex.ToString());
                NotifyHelper.NotifyUser("操作失败: " + ex.Message);
            }
            finally
            {
                if (conn != null)
                {
                    conn.Close();
                }
            }
        }

        private void HandleProAdd()
        {
            MySqlConnection conn = null;
            try
            {
                conn = MySqlConnHelper.GetMySqlConn(Properties.Settings.Default.DbConn);
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandType = CommandType.Text;
                #region
                cmd.CommandText = "insert into product(name, rname, level, price) values(?name, ?rname, ?level, ?price);";
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("?name", textBoxProName.Text);
                cmd.Parameters.AddWithValue("?rname", textBoxProRName.Text);
                cmd.Parameters.AddWithValue("?level", textBoxProLevel.Text);
                cmd.Parameters.AddWithValue("?price", textBoxProPrice.Text);
                conn.Open();

                #region log
                StringBuilder sb = new StringBuilder(512);
                sb.AppendFormat("C product ");
                sb.AppendFormat("{0}=[{1}],", "name", textBoxProRName.Text);
                sb.AppendFormat("{0}=[{1}],", "rname", textBoxProName.Text);
                sb.AppendFormat("{0}=[{1}],", "level", textBoxProLevel.Text);
                sb.AppendFormat("{0}=[{1}],", "price", textBoxProPrice.Text);
                WriteLog(sb.ToString());
                #endregion
                cmd.ExecuteNonQuery();

                // 更新列表
                ResetPro();
                RefreshProList();
                #endregion
            }
            catch (Exception ex)
            {
                WriteLog(ex.ToString());
                NotifyHelper.NotifyUser("操作失败: " + ex.Message);
            }
            finally
            {
                if (conn != null)
                {
                    conn.Close();
                }
            }
        }

        private void ResetPro()
        {
            textBoxProID.Text = string.Empty;
            textBoxProRName.Text = string.Empty;
            textBoxProName.Text = string.Empty;
            textBoxProLevel.Text = string.Empty;
            textBoxProPrice.Text = string.Empty;
        }

        private bool CheckProItems()
        {
            //if (string.IsNullOrEmpty(textBoxProID.Text))  // 添加操作
            {
                if (string.IsNullOrEmpty(textBoxProRName.Text))
                {
                    NotifyHelper.NotifyUser("名称");
                    return false;
                }
            }
            if (string.IsNullOrEmpty(textBoxProName.Text))
            {
                NotifyHelper.NotifyUser(label26.Text);
                return false;
            }
            if (string.IsNullOrEmpty(textBoxProLevel.Text))
            {
                NotifyHelper.NotifyUser("等级");
                return false;
            }
            decimal tmpDec = 0m;
            if (string.IsNullOrEmpty(textBoxProPrice.Text)
                || decimal.TryParse(textBoxProPrice.Text, out tmpDec) == false)
            {
                NotifyHelper.NotifyUser("单价");
                return false;
            }
            return true;
        }

        private void buttonProCommit_Click(object sender, EventArgs e)
        {
            TrimAllPro();
            if (CheckProItems() == false)
            {
                return;
            }
            if (string.IsNullOrEmpty(textBoxProID.Text))
            {
                HandleProAdd();
            }
            else
            {
                HandleProEdit();
            }
        }

        private void buttonProRefresh_Click(object sender, EventArgs e)
        {
            RefreshProList();
        }

        private void buttonProReset_Click(object sender, EventArgs e)
        {
            ResetPro();
        }

        private void RefreshProList()
        {
            listViewProInfo.Items.Clear();
            MySqlConnection conn = null;
            try
            {
                conn = MySqlConnHelper.GetMySqlConn(Properties.Settings.Default.DbConn);
                MySqlDataAdapter adapter = new MySqlDataAdapter();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "select id, name, rname, level, price from product order by name asc;";
                adapter.SelectCommand = cmd;
                DataTable dt = new DataTable();
                conn.Open();
                adapter.Fill(dt);
                conn.Close();
                if (dt.Rows.Count > 0)
                {
                    if (dt.Rows.Count > 0)
                    {

                        listViewProInfo.BeginUpdate();
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            ListViewItem item = new ListViewItem(dt.Rows[i]["id"].ToString());
                            item.SubItems.Add(dt.Rows[i]["rname"].ToString());
                            item.SubItems.Add(dt.Rows[i]["name"].ToString());
                            item.SubItems.Add(dt.Rows[i]["level"].ToString());
                            //string acl = dt.Rows[i]["level"].ToString();
                            //item.SubItems.Add(acl);
                            item.SubItems.Add(dt.Rows[i]["price"].ToString());

                            listViewProInfo.Items.Add(item);
                        }
                        listViewProInfo.EndUpdate();
                    }
                }
            }
            catch (Exception ex)
            {
                WriteLog(ex.ToString());
                NotifyHelper.NotifyUser("操作失败: " + ex.Message);
            }
            finally
            {
                if (conn != null)
                {
                    conn.Close();
                }
            }
        }

        private void listViewProInfo_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            ResetPro();

            if (listViewProInfo.SelectedIndices.Count > 0)
            {
                textBoxProID.Text = listViewProInfo.SelectedItems[0].Text;
                textBoxProRName.Text = listViewProInfo.SelectedItems[0].SubItems[1].Text;
                textBoxProName.Text = listViewProInfo.SelectedItems[0].SubItems[2].Text;
                textBoxProLevel.Text = listViewProInfo.SelectedItems[0].SubItems[3].Text;
                textBoxProPrice.Text = listViewProInfo.SelectedItems[0].SubItems[4].Text;
            }
        }

        #endregion

        #region sale customer infomation
        //private void InitSaleCusTab()
        //{
        //    RefreshSaleCusList();
        //    ResetSaleCus();
        //}

        private void TrimAllSaleCus()
        {
            textBoxSaleCusID.Text = textBoxSaleCusID.Text.Trim();
            textBoxSaleCusName.Text = textBoxSaleCusName.Text.Trim();
        }

        private void HandleSaleCusEdit()
        {
            MySqlConnection conn = null;
            try
            {
                conn = MySqlConnHelper.GetMySqlConn(Properties.Settings.Default.DbConn);
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandType = CommandType.Text;
                #region
                cmd.CommandText = "update sale_customer set name=?name where id=?id;";
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("?name", textBoxSaleCusName.Text);
                string id = listViewSaleCusInfo.SelectedItems[0].SubItems[0].Text;
                cmd.Parameters.AddWithValue("?id", id);
                conn.Open();
                cmd.ExecuteNonQuery();
                #region 记录操作日志
                StringBuilder sb = new StringBuilder(512);
                sb.AppendFormat("U sale_customer {0}=[{1}], ", "name", textBoxSaleCusName.Text);
                sb.AppendFormat("{0}=[{1}], ", "id", id);
                WriteLog(sb.ToString());
                #endregion

                // 更新列表
                ResetSaleCus();
                RefreshSaleCusList();
                #endregion
            }
            catch (Exception ex)
            {
                WriteLog(ex.ToString());
                NotifyHelper.NotifyUser("操作失败: " + ex.Message);
            }
            finally
            {
                if (conn != null)
                {
                    conn.Close();
                }
            }
        }

        private void HandleSaleCusAdd()
        {
            MySqlConnection conn = null;
            try
            {
                conn = MySqlConnHelper.GetMySqlConn(Properties.Settings.Default.DbConn);
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandType = CommandType.Text;
                #region
                cmd.CommandText = "insert into sale_customer(name) values(?name);";
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("?name", textBoxSaleCusName.Text);
                conn.Open();
                #region log
                StringBuilder sb = new StringBuilder(512);
                sb.AppendFormat("C sale_customer ");
                sb.AppendFormat("{0}=[{1}],", "name", textBoxSaleCusName.Text);
                WriteLog(sb.ToString());
                #endregion
                cmd.ExecuteNonQuery();

                // 更新列表
                ResetSaleCus();
                RefreshSaleCusList();
                #endregion
            }
            catch (Exception ex)
            {
                WriteLog(ex.ToString());
                NotifyHelper.NotifyUser("操作失败: " + ex.Message);
            }
            finally
            {
                if (conn != null)
                {
                    conn.Close();
                }
            }
        }

        private void ResetSaleCus()
        {
            textBoxSaleCusID.Text = string.Empty;
            textBoxSaleCusName.Text = string.Empty;
        }

        private bool CheckSaleCusItems()
        {
            //if (string.IsNullOrEmpty(textBoxSaleCusID.Text))  // 添加操作
            {
                if (string.IsNullOrEmpty(textBoxSaleCusName.Text))
                {
                    NotifyHelper.NotifyUser("公司名");
                    return false;
                }
            }
            return true;
        }

        private void buttonSaleCusCommit_Click(object sender, EventArgs e)
        {
            TrimAllSaleCus();
            if (CheckSaleCusItems() == false)
            {
                return;
            }
            if (string.IsNullOrEmpty(textBoxSaleCusID.Text))
            {
                HandleSaleCusAdd();
            }
            else
            {
                HandleSaleCusEdit();
            }
        }

        private void buttonSaleCusRefresh_Click(object sender, EventArgs e)
        {
            RefreshSaleCusList();
        }

        private void buttonSaleCusReset_Click(object sender, EventArgs e)
        {
            ResetSaleCus();
        }

        private void RefreshSaleCusList()
        {
            listViewSaleCusInfo.Items.Clear();
            MySqlConnection conn = null;
            try
            {
                conn = MySqlConnHelper.GetMySqlConn(Properties.Settings.Default.DbConn);
                MySqlDataAdapter adapter = new MySqlDataAdapter();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "select id, name from sale_customer order by name asc;";
                adapter.SelectCommand = cmd;
                DataTable dt = new DataTable();
                conn.Open();
                adapter.Fill(dt);
                conn.Close();
                if (dt.Rows.Count > 0)
                {
                    if (dt.Rows.Count > 0)
                    {

                        listViewSaleCusInfo.BeginUpdate();
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            ListViewItem item = new ListViewItem(dt.Rows[i]["id"].ToString());
                            item.SubItems.Add(dt.Rows[i]["name"].ToString());

                            listViewSaleCusInfo.Items.Add(item);
                        }
                        listViewSaleCusInfo.EndUpdate();
                    }
                }
            }
            catch (Exception ex)
            {
                WriteLog(ex.ToString());
                NotifyHelper.NotifyUser("操作失败: " + ex.Message);
            }
            finally
            {
                if (conn != null)
                {
                    conn.Close();
                }
            }
        }

        private void listViewSaleCusInfo_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            ResetSaleCus();

            if (listViewSaleCusInfo.SelectedIndices.Count > 0)
            {
                textBoxSaleCusID.Text = listViewSaleCusInfo.SelectedItems[0].Text;
                textBoxSaleCusName.Text = listViewSaleCusInfo.SelectedItems[0].SubItems[1].Text;
            }
        }

        #endregion

        #region Pur Org info
        //private void InitSaleCusTab()
        //{
        //    RefreshSaleCusList();
        //    ResetSaleCus();
        //}

        private void TrimAllPurOrg()
        {
            textBoxPurOrgID.Text = textBoxPurOrgID.Text.Trim();
            textBoxPurOrgName.Text = textBoxPurOrgName.Text.Trim();
        }

        private void HandlePurOrgEdit()
        {
            MySqlConnection conn = null;
            try
            {
                conn = MySqlConnHelper.GetMySqlConn(Properties.Settings.Default.DbConn);
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandType = CommandType.Text;
                #region
                cmd.CommandText = "update  pur_organization set name=?name where id=?id;";
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("?name", textBoxPurOrgName.Text);
                string id = listViewPurOrgInfo.SelectedItems[0].SubItems[0].Text;
                cmd.Parameters.AddWithValue("?id", id);
                conn.Open();
                cmd.ExecuteNonQuery();
                #region 记录操作日志
                StringBuilder sb = new StringBuilder(512);
                sb.AppendFormat("U pur_organization ");
                sb.AppendFormat("{0}=[{1}],", "name", textBoxPurOrgName.Text);
                sb.AppendFormat("{0}=[{1}],", "id", id);
                WriteLog(sb.ToString());
                #endregion

                // 更新列表
                ResetPurOrg();
                RefreshPurOrgList();
                #endregion
            }
            catch (Exception ex)
            {
                WriteLog(ex.ToString());
                NotifyHelper.NotifyUser("操作失败: " + ex.Message);
            }
            finally
            {
                if (conn != null)
                {
                    conn.Close();
                }
            }
        }

        private void HandlePurOrgAdd()
        {
            MySqlConnection conn = null;
            try
            {
                conn = MySqlConnHelper.GetMySqlConn(Properties.Settings.Default.DbConn);
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandType = CommandType.Text;
                #region
                cmd.CommandText = "insert into  pur_organization(name) values(?name);";
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("?name", textBoxPurOrgName.Text);
                conn.Open();
                #region log
                StringBuilder sb = new StringBuilder(512);
                sb.AppendFormat("C pur_organization {0}=[{1}],", "name", textBoxPurOrgName.Text);
                WriteLog(sb.ToString());
                #endregion
                cmd.ExecuteNonQuery();

                // 更新列表
                ResetPurOrg();
                RefreshPurOrgList();
                #endregion
            }
            catch (Exception ex)
            {
                WriteLog(ex.ToString());
                NotifyHelper.NotifyUser("操作失败: " + ex.Message);
            }
            finally
            {
                if (conn != null)
                {
                    conn.Close();
                }
            }
        }

        private void ResetPurOrg()
        {
            textBoxPurOrgID.Text = string.Empty;
            textBoxPurOrgName.Text = string.Empty;
        }

        private bool CheckPurOrgItems()
        {
            //if (string.IsNullOrEmpty(textBoxPurOrgID.Text))  // 添加操作
            {
                if (string.IsNullOrEmpty(textBoxPurOrgName.Text))
                {
                    NotifyHelper.NotifyUser("组织名");
                    return false;
                }
            }
            return true;
        }

        private void buttonPurOrgCommit_Click(object sender, EventArgs e)
        {
            TrimAllPurOrg();
            if (CheckPurOrgItems() == false)
            {
                return;
            }
            if (string.IsNullOrEmpty(textBoxPurOrgID.Text))
            {
                HandlePurOrgAdd();
            }
            else
            {
                HandlePurOrgEdit();
            }
        }

        private void buttonPurOrgRefresh_Click(object sender, EventArgs e)
        {
            RefreshPurOrgList();
        }

        private void buttonPurOrgReset_Click(object sender, EventArgs e)
        {
            ResetPurOrg();
        }

        private void RefreshPurOrgList()
        {
            listViewPurOrgInfo.Items.Clear();
            MySqlConnection conn = null;
            try
            {
                conn = MySqlConnHelper.GetMySqlConn(Properties.Settings.Default.DbConn);
                MySqlDataAdapter adapter = new MySqlDataAdapter();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "select id, name from  pur_organization order by name asc;";
                adapter.SelectCommand = cmd;
                DataTable dt = new DataTable();
                conn.Open();
                adapter.Fill(dt);
                conn.Close();
                if (dt.Rows.Count > 0)
                {
                    if (dt.Rows.Count > 0)
                    {

                        listViewPurOrgInfo.BeginUpdate();
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            ListViewItem item = new ListViewItem(dt.Rows[i]["id"].ToString());
                            item.SubItems.Add(dt.Rows[i]["name"].ToString());

                            listViewPurOrgInfo.Items.Add(item);
                        }
                        listViewPurOrgInfo.EndUpdate();
                    }
                }
            }
            catch (Exception ex)
            {
                WriteLog(ex.ToString());
                NotifyHelper.NotifyUser("操作失败: " + ex.Message);
            }
            finally
            {
                if (conn != null)
                {
                    conn.Close();
                }
            }
        }

        private void listViewPurOrgInfo_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            ResetPurOrg();

            if (listViewPurOrgInfo.SelectedIndices.Count > 0)
            {
                textBoxPurOrgID.Text = listViewPurOrgInfo.SelectedItems[0].Text;
                textBoxPurOrgName.Text = listViewPurOrgInfo.SelectedItems[0].SubItems[1].Text;
            }
        }

        #endregion

        #region Pur Per info
        private ArrayList tableOrg = null;   // 组织表

        private void TrimAllPurPer()
        {
            //textBoxPurPerID.Text = textBoxPurPerID.Text.Trim();
            textBoxPurPerName.Text = textBoxPurPerName.Text.Trim();
            textBoxPurPerNI.Text = textBoxPurPerNI.Text.Trim();
            textBoxPurPerUpper.Text = textBoxPurPerUpper.Text.Trim();
        }

        private void HandlePurPerEdit()
        {
            MySqlConnection conn = null;
            try
            {
                conn = MySqlConnHelper.GetMySqlConn(Properties.Settings.Default.DbConn);
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandType = CommandType.Text;
                #region
                cmd.CommandText = "update  pur_customer set upper=?upper where id=?id;";
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("?upper", textBoxPurPerUpper.Text);
                string id = listViewPurPerInfo.SelectedItems[0].SubItems[0].Text;
                cmd.Parameters.AddWithValue("?id", id);
                conn.Open();
                cmd.ExecuteNonQuery();

                #region 记录操作日志
                StringBuilder sb = new StringBuilder(512);
                sb.AppendFormat("U pur_customer ");
                sb.AppendFormat("{0}=[{1}],", "upper", textBoxPurPerUpper.Text);
                sb.AppendFormat("{0}=[{1}],", "id", id);
                WriteLog(sb.ToString());
                #endregion

                // 更新列表
                ResetPurPer();
                RefreshPurPerList();
                #endregion
            }
            catch (Exception ex)
            {
                WriteLog(ex.ToString());
                NotifyHelper.NotifyUser("操作失败: " + ex.Message);
            }
            finally
            {
                if (conn != null)
                {
                    conn.Close();
                }
            }
        }

        static readonly string tagPrefix = "690" + Properties.Settings.Default.TagStub;
        private void HandlePurPerAdd()
        {
            MySqlConnection conn = null;
            try
            {
                string orgID = "0";
                ListItem listItem = comboBoxPurOrg.SelectedItem as ListItem;
                orgID = listItem.ID;
                conn = MySqlConnHelper.GetMySqlConn(Properties.Settings.Default.DbConn);
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandType = CommandType.Text;
                #region
                cmd.CommandText = "select max(id) from pur_customer;";
                cmd.Parameters.Clear();
                conn.Open();
                string maxID = cmd.ExecuteScalar().ToString();
                maxID = maxID.PadLeft(5, '0');
                string tag = tagPrefix + maxID;  //690 00001 00001，690固定，中间5位从配置文件得来，末5位为id-1

                cmd.CommandText = @"insert into  pur_customer(name, org_id, ni, tag, upper) 
 values(?name, ?org_id, ?ni, ?tag, ?upper);";
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("?name", textBoxPurPerName.Text);
                cmd.Parameters.AddWithValue("?org_id", orgID);
                cmd.Parameters.AddWithValue("?ni", textBoxPurPerNI.Text);
                cmd.Parameters.AddWithValue("?tag", tag);
                cmd.Parameters.AddWithValue("?upper", textBoxPurPerUpper.Text);
                cmd.ExecuteNonQuery();

                #region log
                StringBuilder sb = new StringBuilder(512);
                sb.AppendFormat("C pur_customer ");
                sb.AppendFormat("{0}=[{1}],", "name", PswdHelper.EncryptString(textBoxPurPerName.Text));
                sb.AppendFormat("{0}=[{1}],", "org_id", orgID);
                sb.AppendFormat("{0}=[{1}],", "ni", PswdHelper.EncryptString(textBoxPurPerNI.Text));
                sb.AppendFormat("{0}=[{1}],", "tag", tag);
                sb.AppendFormat("{0}=[{1}],", "upper", textBoxPurPerUpper.Text);

                WriteLog(sb.ToString());
                #endregion

                // 更新列表
                ResetPurPer();
                RefreshPurPerList();
                #endregion
            }
            catch (Exception ex)
            {
                WriteLog(ex.ToString());
                NotifyHelper.NotifyUser("操作失败: " + ex.Message);
            }
            finally
            {
                if (conn != null)
                {
                    conn.Close();
                }
            }
        }

        private void ResetPurPer()
        {
            textBoxPurPerID.Text = string.Empty;
            textBoxPurPerName.Text = string.Empty;
            comboBoxPurOrg.SelectedIndex = -1;
            textBoxPurPerNI.Text = string.Empty;
            textBoxPurPerTag.Text = string.Empty;
            textBoxPurPerUpper.Text = "0";
            textBoxPurPerUppUsed.Text = string.Empty;

            textBoxPurPerName.ReadOnly = false;
            textBoxPurPerNI.ReadOnly = false;
            comboBoxPurOrg.Enabled = true;
        }

        private bool CheckPurPerItems()
        {
            //if (string.IsNullOrEmpty(textBoxPurPerID.Text))  // 添加操作
            {
                if (string.IsNullOrEmpty(textBoxPurPerName.Text))
                {
                    NotifyHelper.NotifyUser("姓名");
                    return false;
                }
            }
            if (comboBoxPurOrg.SelectedIndex == -1)
            {
                NotifyHelper.NotifyUser("所属组织");
                return false;
            }
            if (string.IsNullOrEmpty(textBoxPurPerNI.Text)
                || textBoxPurPerNI.Text.Length < 15
                || textBoxPurPerNI.Text.Length > 18)
            {
                NotifyHelper.NotifyUser("身份证");
                return false;

            }
            int tmpUpper = 0;
            if (string.IsNullOrEmpty(textBoxPurPerUpper.Text)
                 || int.TryParse(textBoxPurPerUpper.Text, out tmpUpper) == false)
            {
                NotifyHelper.NotifyUser("限额");
                return false;
            }
            return true;
        }

        private void buttonPurPerCommit_Click(object sender, EventArgs e)
        {
            TrimAllPurPer();
            if (CheckPurPerItems() == false)
            {
                return;
            }
            if (string.IsNullOrEmpty(textBoxPurPerID.Text))
            {
                HandlePurPerAdd();
            }
            else
            {
                HandlePurPerEdit();
            }
        }

        private void buttonPurPerRefresh_Click(object sender, EventArgs e)
        {
            RefreshPurPerList();
        }

        private void buttonPurPerReset_Click(object sender, EventArgs e)
        {
            ResetPurPer();
        }

        private void RefreshPurPerList()
        {
            listViewPurPerInfo.Items.Clear();
            MySqlConnection conn = null;
            try
            {
                conn = MySqlConnHelper.GetMySqlConn(Properties.Settings.Default.DbConn);
                MySqlDataAdapter adapter = new MySqlDataAdapter();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = @"select t.id, t.name, t.ni, t.upper, t.tag, t.upper_used, 
t.org_id, s.name as org_name 
from  pur_customer t, pur_organization s
where t.org_id=s.id order by t.name asc;";
                adapter.SelectCommand = cmd;
                DataTable dt = new DataTable();
                conn.Open();
                adapter.Fill(dt);
                conn.Close();
                if (dt.Rows.Count > 0)
                {

                    listViewPurPerInfo.BeginUpdate();
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        ListViewItem item = new ListViewItem(dt.Rows[i]["id"].ToString());
                        item.SubItems.Add(dt.Rows[i]["name"].ToString());
                        item.SubItems.Add(dt.Rows[i]["org_name"].ToString());
                        item.SubItems.Add(dt.Rows[i]["ni"].ToString());
                        item.SubItems.Add(dt.Rows[i]["upper"].ToString());
                        item.SubItems.Add(dt.Rows[i]["org_id"].ToString());    //5
                        item.SubItems.Add(dt.Rows[i]["tag"].ToString());    //6
                        item.SubItems.Add(dt.Rows[i]["upper_used"].ToString());    //7

                        listViewPurPerInfo.Items.Add(item);
                    }
                    listViewPurPerInfo.EndUpdate();
                }
                #region pur_organazition
                // 简化代码逻辑，每次刷新时都刷新tableOrg；
                // 虽然效率不是最好，但代码维护非常简单--与其他模块或函数之间无关联
                // if (tableOrg == null)    // 只运行一次
                {
                    cmd.CommandText = "select id, name from pur_organization order by name asc";
                    adapter.SelectCommand = cmd;
                    dt.Clear();
                    conn.Open();
                    adapter.Fill(dt);
                    conn.Close();
                    if (dt.Rows.Count > 0)
                    {
                        tableOrg = new ArrayList();
                        comboBoxPurOrg.Items.Clear();
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            TableMaterial tm = new TableMaterial();
                            tm.id = int.Parse(dt.Rows[i]["id"].ToString());
                            tm.name = dt.Rows[i]["name"].ToString();
                            tableOrg.Add(tm);

                            ListItem org = new ListItem(tm.id.ToString(), tm.name);
                            comboBoxPurOrg.Items.Add(org);
                        }
                        comboBoxPurOrg.DisplayMember = "Name";
                        comboBoxPurOrg.ValueMember = "ID";
                    }
                }
                #endregion

            }
            catch (Exception ex)
            {
                WriteLog(ex.ToString());
                NotifyHelper.NotifyUser("操作失败: " + ex.Message);
            }
            finally
            {
                if (conn != null)
                {
                    conn.Close();
                }
            }
        }

        private void listViewPurPerInfo_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            ResetPurPer();

            if (listViewPurPerInfo.SelectedIndices.Count > 0)
            {
                textBoxPurPerID.Text = listViewPurPerInfo.SelectedItems[0].Text;
                textBoxPurPerName.Text = listViewPurPerInfo.SelectedItems[0].SubItems[1].Text;

                string tmp = listViewPurPerInfo.SelectedItems[0].SubItems[5].Text;
                for (int i = 0; i < comboBoxPurOrg.Items.Count; i++)
                {
                    ListItem listItem = comboBoxPurOrg.Items[i] as ListItem;
                    if (tmp == listItem.ID)
                    {
                        comboBoxPurOrg.SelectedIndex = i;
                        break;
                    }
                }

                textBoxPurPerNI.Text = listViewPurPerInfo.SelectedItems[0].SubItems[3].Text;
                textBoxPurPerUpper.Text = listViewPurPerInfo.SelectedItems[0].SubItems[4].Text;
                textBoxPurPerTag.Text = listViewPurPerInfo.SelectedItems[0].SubItems[6].Text;
                textBoxPurPerUppUsed.Text = listViewPurPerInfo.SelectedItems[0].SubItems[7].Text;

                textBoxPurPerName.ReadOnly = true;
                textBoxPurPerNI.ReadOnly = true;
                comboBoxPurOrg.Enabled = false;
            }
        }

        #endregion

        #region pur记录
        DataTable purDt = new DataTable();

        private void buttonPurQuery_Click(object sender, EventArgs e)
        {
            listViewPurDetail.Items.Clear();
            string dateFrom = dateTimePickerPurFrom.Value.Date.ToString("yyyy-MM-dd");
            string dateTo = dateTimePickerPurTo.Value.Date.ToString("yyyy-MM-dd");
            string sql = string.Format(@"select s.id, m.name as mat_name, m.level, p.name as org_name,
 t.name as cus_name, t.ni, s.price, s.quantity,  s.counter, e.name as emp_name,  s.deal_time
 from pur_customer t, pur_detail s, pur_organization p, employee e, material m 
 where t.id=s.customer_id and t.org_id=p.id
 and s.emp_id=e.id and s.material_id=m.id
 and to_days(deal_time)>=to_days('{0}')
 and to_days(deal_time)<=to_days('{1}')
 order by s.id;", dateFrom, dateTo);

            MySqlConnection conn = null;
            try
            {
                conn = MySqlConnHelper.GetMySqlConn(Properties.Settings.Default.DbConn);
                MySqlDataAdapter adapter = new MySqlDataAdapter();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandType = CommandType.Text;
                purDt = new DataTable();
                conn.Open();
                #region 获取数据
                cmd.CommandText = sql;
                adapter.SelectCommand = cmd;
                adapter.Fill(purDt);
                conn.Close();
                if (purDt.Rows.Count > 0)
                {
                    listViewPurDetail.BeginUpdate();
                    for (int i = 0; i < purDt.Rows.Count; i++)
                    {
                        ListViewItem item = new ListViewItem(purDt.Rows[i]["id"].ToString());
                        item.SubItems.Add(purDt.Rows[i]["org_name"].ToString());
                        item.SubItems.Add(purDt.Rows[i]["cus_name"].ToString());
                        item.SubItems.Add(purDt.Rows[i]["ni"].ToString());
                        item.SubItems.Add(purDt.Rows[i]["mat_name"].ToString());
                        item.SubItems.Add(purDt.Rows[i]["level"].ToString());
                        item.SubItems.Add(purDt.Rows[i]["price"].ToString());
                        item.SubItems.Add(purDt.Rows[i]["quantity"].ToString());
                        item.SubItems.Add(purDt.Rows[i]["counter"].ToString());
                        item.SubItems.Add(purDt.Rows[i]["emp_name"].ToString());
                        item.SubItems.Add(purDt.Rows[i]["deal_time"].ToString());
                        listViewPurDetail.Items.Add(item);
                    }
                    listViewPurDetail.EndUpdate();
                }
                else
                {
                    // 向用户给出提示
                    NotifyHelper.NotifyUser("没有符合条件的数据");
                }
                #endregion
            }
            catch (Exception ex)
            {
                WriteLog(ex.ToString());
                NotifyHelper.NotifyUser("操作失败: " + ex.Message);
            }
            finally
            {
                if (conn != null)
                {
                    conn.Close();
                }
                listViewPurDetail.EndUpdate();
            }
        }

        private void buttonPurExport_Click(object sender, EventArgs e)
        {
            ExportData(purDt, "sg收购记录");
        }
        #endregion

        #region sale记录
        DataTable saleDt = new DataTable();

        private void buttonSaleQuery_Click(object sender, EventArgs e)
        {
            listViewSaleDetail.Items.Clear();
            string dateFrom = dateTimePickerSaleFrom.Value.Date.ToString("yyyy-MM-dd");
            string dateTo = dateTimePickerSaleTo.Value.Date.ToString("yyyy-MM-dd");
            string sql = string.Format(@"select s.id, sc.name as customer_name, p.name as product_name, 
 p.level as product_level, s.price, s.quantity, counter, e.name as emp_name, deal_time
 from sale_detail s, employee e, product p, sale_customer sc 
 where s.emp_id=e.id and s.product_id=p.id and s.customer_id=sc.id
 and to_days(deal_time)>=to_days('{0}')
 and to_days(deal_time)<=to_days('{1}')
 order by s.id;", dateFrom, dateTo);

            MySqlConnection conn = null;
            try
            {
                saleDt = new DataTable();
                conn = MySqlConnHelper.GetMySqlConn(Properties.Settings.Default.DbConn);
                MySqlDataAdapter adapter = new MySqlDataAdapter();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandType = CommandType.Text;
                conn.Open();
                #region
                cmd.CommandText = sql;
                adapter.SelectCommand = cmd;
                adapter.Fill(saleDt);
                conn.Close();
                if (saleDt.Rows.Count > 0)
                {
                    listViewSaleDetail.BeginUpdate();
                    for (int i = 0; i < saleDt.Rows.Count; i++)
                    {
                        ListViewItem item = new ListViewItem(saleDt.Rows[i]["id"].ToString());
                        item.SubItems.Add(saleDt.Rows[i]["customer_name"].ToString());
                        item.SubItems.Add(saleDt.Rows[i]["product_name"].ToString());
                        item.SubItems.Add(saleDt.Rows[i]["product_level"].ToString());
                        item.SubItems.Add(saleDt.Rows[i]["price"].ToString());
                        item.SubItems.Add(saleDt.Rows[i]["quantity"].ToString());
                        item.SubItems.Add(saleDt.Rows[i]["counter"].ToString());
                        item.SubItems.Add(saleDt.Rows[i]["emp_name"].ToString());
                        item.SubItems.Add(saleDt.Rows[i]["deal_time"].ToString());
                        listViewSaleDetail.Items.Add(item);
                    }
                    listViewSaleDetail.EndUpdate();
                }
                else
                {
                    // 向用户给出提示
                    NotifyHelper.NotifyUser("没有符合条件的数据");
                }

                #endregion
            }
            catch (Exception ex)
            {
                WriteLog(ex.ToString());
                NotifyHelper.NotifyUser("操作失败: " + ex.Message);
            }
            finally
            {
                if (conn != null)
                {
                    conn.Close();
                }
                listViewSaleDetail.EndUpdate();
            }
        }

        private void buttonSaleExport_Click(object sender, EventArgs e)
        {
            ExportData(saleDt, "xs销售记录");
        }
        #endregion

        #region operation 记录
        private void buttonProQuery_Click(object sender, EventArgs e)
        {
            listViewProDetail.Items.Clear();
            string dateFrom = dateTimePickerProFrom.Value.Date.ToString("yyyy-MM-dd");
            string dateTo = dateTimePickerProTo.Value.Date.ToString("yyyy-MM-dd");
            string sql = string.Format(@"select o.id, e.name as emp_name, counter, begin_time, end_time
 from operation_detail o, employee e
 where o.emp_id=e.id
 and to_days(begin_time)>=to_days('{0}')
 and to_days(end_time)<=to_days('{1}')
 order by o.id;", dateFrom, dateTo);

            MySqlConnection conn = null;
            try
            {
                conn = MySqlConnHelper.GetMySqlConn(Properties.Settings.Default.DbConn);
                MySqlDataAdapter adapter = new MySqlDataAdapter();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandType = CommandType.Text;
                conn.Open();
                #region
                cmd.CommandText = sql;
                adapter.SelectCommand = cmd;
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                conn.Close();
                if (dt.Rows.Count > 0)
                {
                    listViewProDetail.BeginUpdate();
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        ListViewItem item = new ListViewItem(dt.Rows[i]["id"].ToString());
                        item.SubItems.Add(dt.Rows[i]["emp_name"].ToString());
                        item.SubItems.Add(dt.Rows[i]["counter"].ToString());
                        item.SubItems.Add(dt.Rows[i]["begin_time"].ToString());
                        item.SubItems.Add(dt.Rows[i]["end_time"].ToString());
                        listViewProDetail.Items.Add(item);
                    }
                    listViewProDetail.EndUpdate();
                }
                else
                {
                    // 向用户给出提示
                    NotifyHelper.NotifyUser("没有符合条件的数据");
                }
                #endregion
            }
            catch (Exception ex)
            {
                WriteLog(ex.ToString());
                NotifyHelper.NotifyUser("操作失败: " + ex.Message);
            }
            finally
            {
                if (conn != null)
                {
                    conn.Close();
                }
                listViewProDetail.EndUpdate();
            }
        }

        private void listViewProDetail_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            listViewMaterialCost.Items.Clear();
            listViewProductOut.Items.Clear();

            if (listViewProDetail.SelectedIndices.Count <= 0)
            {
                return;
            }
            string op_id = listViewProDetail.SelectedItems[0].Text;

            string sql;
            MySqlConnection conn = null;
            try
            {
                conn = MySqlConnHelper.GetMySqlConn(Properties.Settings.Default.DbConn);
                MySqlDataAdapter adapter = new MySqlDataAdapter();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandType = CommandType.Text;
                conn.Open();

                #region 原料消耗
#warning 改为变量绑定的方式
                sql = string.Format(@"select p.name, p.level, m.quantity
 from material_cost m, material p 
 where m.material_id=p.id
 and m.operation_id= '{0}';", op_id);
                cmd.CommandText = sql;
                adapter.SelectCommand = cmd;
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                conn.Close();
                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        ListViewItem item = new ListViewItem(dt.Rows[i]["name"].ToString());
                        item.SubItems.Add(dt.Rows[i]["level"].ToString());
                        item.SubItems.Add(dt.Rows[i]["quantity"].ToString());
                        listViewMaterialCost.Items.Add(item);
                    }
                }
                else
                {
                    // 从理论上来讲，不会出现找不到数据的情况
                }
                #endregion

                #region 产品产出
                sql = string.Format(@"select p.name, p.level, m.quantity
 from product_out m, product p 
 where m.product_id=p.id
 and m.operation_id= '{0}';", op_id);

                cmd.CommandText = sql;
                adapter.SelectCommand = cmd;
                dt = new DataTable();
                adapter.Fill(dt);
                conn.Close();
                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        ListViewItem item = new ListViewItem(dt.Rows[i]["name"].ToString());
                        item.SubItems.Add(dt.Rows[i]["level"].ToString());
                        item.SubItems.Add(dt.Rows[i]["quantity"].ToString());
                        listViewProductOut.Items.Add(item);
                    }
                }
                else
                {
                    // 从理论上来讲，不会出现找不到数据的情况
                }
                #endregion
            }
            catch (Exception ex)
            {
                WriteLog(ex.ToString());
                NotifyHelper.NotifyUser("操作失败: " + ex.Message);
            }
            finally
            {
                if (conn != null)
                {
                    conn.Close();
                }
            }
        }

        private void ExportOperationDetail(string sql, string prefix)
        {
            MySqlConnection conn = null;
            try
            {
                conn = MySqlConnHelper.GetMySqlConn(Properties.Settings.Default.DbConn);
                MySqlDataAdapter adapter = new MySqlDataAdapter();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandType = CommandType.Text;
                conn.Open();
                #region
                cmd.CommandText = sql;
                adapter.SelectCommand = cmd;
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                conn.Close();
                if (dt.Rows.Count > 0)
                {
                    ExportData(dt, prefix);
                }
                else
                {
                    // 向用户给出提示
                    NotifyHelper.NotifyUser("没有符合条件的数据");
                }
                #endregion
            }
            catch (Exception ex)
            {
                WriteLog(ex.ToString());
                NotifyHelper.NotifyUser("操作失败: " + ex.Message);
            }
            finally
            {
                if (conn != null)
                {
                    conn.Close();
                }
            }
        }

        private void buttonCostExp_Click(object sender, EventArgs e)
        {
            string dateFrom = dateTimePickerProFrom.Value.Date.ToString("yyyy-MM-dd");
            string dateTo = dateTimePickerProTo.Value.Date.ToString("yyyy-MM-dd");
            string sql = string.Format(@"select o.id, e.name as emp_name, counter, 
 begin_time, end_time, p.name as mat_name, 
 p.level, m.quantity
 from operation_detail o, employee e, material_cost m, material p 
 where  o.emp_id=e.id and o.id=m.operation_id 
 and m.material_id=p.id
 and to_days(begin_time)>=to_days('{0}')
 and to_days(end_time)<=to_days('{1}')
 order by o.id; ", dateFrom, dateTo);

            ExportOperationDetail(sql, "原料消耗");
        }

        private void buttonProduceExp_Click(object sender, EventArgs e)
        {
            string dateFrom = dateTimePickerProFrom.Value.Date.ToString("yyyy-MM-dd");
            string dateTo = dateTimePickerProTo.Value.Date.ToString("yyyy-MM-dd");
            string sql = string.Format(@"select o.id, e.name as emp_name, counter, 
 begin_time, end_time, p.name as mat_name, 
 p.level, m.quantity
 from operation_detail o, employee e, product_out m, product p
 where  o.emp_id=e.id and o.id=m.operation_id 
 and m.product_id=p.id
 and to_days(begin_time)>=to_days('{0}')
 and to_days(end_time)<=to_days('{1}')
 order by o.id; ", dateFrom, dateTo);

            ExportOperationDetail(sql, "物品产出");

        }
        #endregion

        #region 数据导出
        RKLib.ExportData.Export objExport = new RKLib.ExportData.Export("Win");

        /// <summary>
        /// 到处数据到文件
        /// </summary>
        /// <param name="dt">包含数据的DataTable</param>
        /// <param name="prefix">保存文件对话框中显示的文件名，不包含后缀</param>
        private void ExportData(DataTable dt, string prefix)
        {
            if (dt != null && dt.Rows.Count > 0)
            {
                try
                {
                    SaveFileDialog sfd = new SaveFileDialog();
                    if (prefix == null)
                    {
                        prefix = string.Empty;
                    }
                    sfd.FileName = prefix;
                    sfd.Filter = "所有文件(*.*)|*.*|csv文件(*.csv)|*.csv|文本文件(*.txt)|*.txt|excel文件(*.xls)|*.xls";
                    if (sfd.ShowDialog() == DialogResult.OK)
                    {
                        string filename = sfd.FileName;
                        objExport.ExportDetails(dt, Export.ExportFormat.CSV, filename);
                        NotifyUser("导出完毕");
                    }
                }
                catch (Exception ex)
                {
                    logHelper.WriteLog(ex.ToString());
                    NotifyUser(ex.Message);
                }
            }
            else
            {
                NotifyUser("没有数据，所以未导出数据");
            }
        }

        #endregion



        //private void checkBoxEmpPwd_CheckedChanged(object sender, EventArgs e)
        //{

        //}

        //private void buttonEmpRefresh_Click(object sender, EventArgs e)
        //{

        //}

        //private void buttonEmpReset_Click(object sender, EventArgs e)
        //{

        //}

        //private void listViewEmpInfo_MouseDoubleClick(object sender, MouseEventArgs e)
        //{

        //}

        //private void buttonEmpCommit_Click(object sender, EventArgs e)
        //{

        //}

    }

}