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
    public partial class ManageForm : Form
    {
        #region 员工信息
        private void InitEmpTab()
        {
            checkedListBoxEmpAcl.Items.Clear();
            string[] ss = Enum.GetNames(typeof(Products));
            for (int i = 0; i < ss.Length; i++)
            {
                checkedListBoxEmpAcl.Items.Add(ss[i], false);
            }
            //checkedListBoxEmpAcl.Enabled = false;

            RefreshEmpList();
            ResetEmp();
        }

        private void tabPageEmp_Click(object sender, EventArgs e)
        {

        }

        private void TrimAllEmp()
        {
            textBoxEmpID.Text = textBoxEmpID.Text.Trim();
            textBoxEmpName.Text = textBoxEmpName.Text.Trim();
            textBoxEmpPwd.Text = textBoxEmpPwd.Text.Trim();
            textBoxEmpPwd2.Text = textBoxEmpPwd2.Text.Trim();
        }

        private string GetEmpAcl()
        {
            StringBuilder sb = new StringBuilder(16);
            sb.Append(",");
            for (int i = 0; i < checkedListBoxEmpAcl.Items.Count; i++)
            {
                if (checkedListBoxEmpAcl.GetItemChecked(i) == true)
                {
                    Products pro = (Products)Enum.Parse(typeof(Products), checkedListBoxEmpAcl.Items[i].ToString(), true);
                    sb.AppendFormat("{0},", pro.ToString("D"));
                }
            }
            return sb.ToString();
        }

        private void HandleEmpEdit()
        {
            MySqlConnection conn = null;
            try
            {
                conn = MySqlConnHelper.GetMySqlConn(Properties.Settings.Default.DbConn);
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandType = CommandType.Text;
                #region
                string pwd = string.Empty;
                if(checkBoxEmpPwd.Checked==true)
                {
                    pwd = textBoxEmpPwd.Text;
                }
                else
                {
                    pwd = listViewEmpInfo.SelectedItems[0].SubItems[3].Text;
                }
                pwd = PswdHelper.EncryptString(pwd);   // 加密
                cmd.CommandText = "update employee set password=?pwd, acl= ?acl where id=?id;";
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("?pwd", pwd);
                string acl = GetEmpAcl();
                cmd.Parameters.AddWithValue("?acl", acl);
                string id=listViewEmpInfo.SelectedItems[0].SubItems[0].Text;
                cmd.Parameters.AddWithValue("?id", id);
                conn.Open();
                cmd.ExecuteNonQuery();

                // 更新列表
                ResetEmp();
                RefreshEmpList();
                #region 记录操作日志
                StringBuilder sb = new StringBuilder(512);
                sb.AppendFormat("U employee {0}=[{1}], ", "name", PswdHelper.EncryptString(textBoxEmpName.Text));
                sb.AppendFormat("{0}=[{1}], ", "pwd", PswdHelper.EncryptString(textBoxEmpPwd.Text));
                sb.AppendFormat("{0}=[{1}], ", "acl", acl);
                sb.AppendFormat("{0}=[{1}], ", "id", id);
                WriteLog(sb.ToString());
                #endregion


                #endregion
            }
            catch (Exception ex)
            {
                WriteLog(ex.ToString());
                NotifyHelper.NotifyUser(ex.Message);
            }
            finally
            {
                if (conn != null)
                {
                    conn.Close();
                }
            }
        }

        private void HandleEmpAdd()
        {
            MySqlConnection conn = null;
            try
            {
                conn = MySqlConnHelper.GetMySqlConn(Properties.Settings.Default.DbConn);
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandType = CommandType.Text;
                #region
                // 不检查是否已经有相同的姓名
                //cmd.CommandText = "select id, name, acl from employee where name=?name;";
                //cmd.Parameters.Clear();
                //cmd.Parameters.AddWithValue("?name", textBoxEmpName.text);
                //conn.Open();
                //MySqlDataReader dr = cmd.ExecuteReader();
                //if(dr.Read())
                //{
                //    dr.Close();
                //    conn.Close();
                //    NotifyHelper.NotifyUser("该姓名已存在");
                //    return;
                //}
                cmd.CommandText = "insert into employee(name, password, acl) values(?name, ?pwd, ?acl);";
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("?name", textBoxEmpName.Text);
                cmd.Parameters.AddWithValue("?pwd", PswdHelper.EncryptString(textBoxEmpPwd.Text));
                string acl = GetEmpAcl();
                cmd.Parameters.AddWithValue("?acl", acl);
                conn.Open();
                #region log
                StringBuilder sb = new StringBuilder(512);
                sb.AppendFormat("C employee {0}=[{1}], ", "name", PswdHelper.EncryptString(textBoxEmpName.Text));
                sb.AppendFormat("{0}=[{1}], ", "pwd", PswdHelper.EncryptString(textBoxEmpPwd.Text));
                sb.AppendFormat("{0}=[{1}], ", "acl", acl);
                WriteLog(sb.ToString());
#endregion
                cmd.ExecuteNonQuery();

                // 更新列表
                ResetEmp();
                RefreshEmpList();
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

        private void ResetEmp()
        {
            textBoxEmpID.Text = string.Empty;
            textBoxEmpName.Text = string.Empty;
            textBoxEmpName.ReadOnly = false;
            textBoxEmpPwd.Text = string.Empty;
            textBoxEmpPwd2.Text = string.Empty;

            labelResetPwd.Visible = false;
            checkBoxEmpPwd.Visible = false;
            checkBoxEmpPwd.Checked = true;

            checkedListBoxEmpAcl.SelectedIndex = -1;
            for (int i = 0; i < checkedListBoxEmpAcl.Items.Count; i++)
            {
                checkedListBoxEmpAcl.SetItemChecked(i, false);
            }

        }

        private bool CheckEmpItems()
        {
            //if (string.IsNullOrEmpty(textBoxEmpID.Text))  // 添加操作
            {
                if (string.IsNullOrEmpty(textBoxEmpName.Text))
                {
                    NotifyHelper.NotifyUser("姓名");
                    return false;
                }
            }
            if (checkBoxEmpPwd.Checked)
            {
                if (textBoxEmpPwd.Text.Length < 6)
                {
                    NotifyHelper.NotifyUser("密码不能少于6位");
                    return false;
                }
                if (textBoxEmpPwd.Text != textBoxEmpPwd2.Text)
                {
                    NotifyHelper.NotifyUser("2次输入的秘密不一致");
                    return false;
                }
            }
            return true;
        }

        private void buttonEmpCommit_Click(object sender, EventArgs e)
        {
            TrimAllEmp();
            if (CheckEmpItems() == false)
            {
                return;
            }
            if (string.IsNullOrEmpty(textBoxEmpID.Text))
            {
                HandleEmpAdd();
            }
            else
            {
                HandleEmpEdit();
            }
            //ResetEmp();
        }

        private void buttonEmpRefresh_Click(object sender, EventArgs e)
        {
            RefreshEmpList();
        }

        private void buttonEmpReset_Click(object sender, EventArgs e)
        {
            ResetEmp();
        }

        private void RefreshEmpList()
        {
            listViewEmpInfo.Items.Clear();
            MySqlConnection conn = null;
            try
            {
                conn = MySqlConnHelper.GetMySqlConn(Properties.Settings.Default.DbConn);
                MySqlDataAdapter adapter = new MySqlDataAdapter();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "select id, name, acl, password from employee order by id asc;";
                adapter.SelectCommand = cmd;
                DataTable dt = new DataTable();
                conn.Open();
                adapter.Fill(dt);
                conn.Close();
                if (dt.Rows.Count > 0)
                {
                    if (dt.Rows.Count > 0)
                    {

                        listViewEmpInfo.BeginUpdate();
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            ListViewItem item = new ListViewItem(dt.Rows[i]["id"].ToString());
                            item.SubItems.Add(dt.Rows[i]["name"].ToString());
                            //item.SubItems.Add(dt.Rows[i]["level"].ToString());
                            string acl = dt.Rows[i]["acl"].ToString();
                            item.SubItems.Add(acl);
                            item.SubItems.Add(dt.Rows[i]["password"].ToString());

                            listViewEmpInfo.Items.Add(item);
                        }
                        listViewEmpInfo.EndUpdate();
                    }
                }
            }
//            catch (MySqlException sqlEx)
//            {
//#warning 记录日志
//                NotifyHelper.NotifyUser("无法连接服务器");
//            }
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

        private void listViewEmpInfo_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            ResetEmp();

            if (listViewEmpInfo.SelectedIndices.Count > 0)
            {
                labelResetPwd.Visible = true;
                checkBoxEmpPwd.Visible = true;
                checkBoxEmpPwd.Checked = false;
                textBoxEmpName.ReadOnly = true;

                textBoxEmpID.Text = listViewEmpInfo.SelectedItems[0].Text;
                textBoxEmpName.Text = listViewEmpInfo.SelectedItems[0].SubItems[1].Text;
                string acl = listViewEmpInfo.SelectedItems[0].SubItems[2].Text;
                // 转换acl
                string[] ss = acl.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                for (int i = 0; i < ss.Length; i++)
                {
                    string name = Enum.GetName(typeof(Products), int.Parse(ss[i]));
                    for (int j = 0; j < checkedListBoxEmpAcl.Items.Count; j++)
                    {
                        if (checkedListBoxEmpAcl.Items[j].ToString() == name)
                        {
                            checkedListBoxEmpAcl.SetItemChecked(j, true);
                        }
                    }
                }
            }
        }

        private void checkBoxEmpPwd_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxEmpPwd.Checked)
            {
                textBoxEmpPwd.Enabled = true;
                textBoxEmpPwd2.Enabled = true;
            }
            else
            {
                textBoxEmpPwd.Enabled = false;
                textBoxEmpPwd2.Enabled = false;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="acl">输入的格式为",1,2,4,"</param>
        /// <returns>输出的格式为"pur,sale,manage"</returns>
        //private string AclToString(string acl)
        //{

        //}
        #endregion
    }
}
