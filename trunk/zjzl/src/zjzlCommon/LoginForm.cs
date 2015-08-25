using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using MySql.Data.MySqlClient;

namespace zjzl
{
    public partial class LoginForm : Form
    {
        private string empId = string.Empty;
        public string EmpId
        {
            get { return empId; }
        }

        private string connStr = string.Empty;
        Products product = Products.none;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="conn">经过加密的数据库连接字</param>
        /// <param name="pro">产品代号</param>
        /// <exception cref="System.NullReferenceException">conn为null或者Empty都抛出此异常</exception>
        public LoginForm(string conn, Products pro)
        {
            InitializeComponent();
            if (string.IsNullOrEmpty(conn))
            {
                throw new System.NullReferenceException();
            }
            connStr = conn;
            this.product = pro;
            this.DialogResult = DialogResult.Cancel;
            this.Text = "登录到" + this.product.ToString();
        }

        private void buttonLogin_Click(object sender, EventArgs e)
        {
            #region 检查不为空
            textBoxUserId.Text = textBoxUserId.Text.Trim();
            textBoxPwd.Text = textBoxPwd.Text.Trim();
            if (string.IsNullOrEmpty(textBoxUserId.Text))
            {
                NotifyHelper.NotifyUser("工号");
                return;
            }
            if (string.IsNullOrEmpty(textBoxPwd.Text))
            {
                NotifyHelper.NotifyUser("密码");
                return;
            }
            #endregion

            MySqlConnection conn = null;
            try
            {
                conn = MySqlConnHelper.GetMySqlConn(connStr);
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "select password, acl from employee where id=?id;";
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("?id", textBoxUserId.Text);
                MySqlDataReader dr = null;
                conn.Open();
                dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    string acl = dr["acl"].ToString();

                    string tmp = string.Format(",{0},", product.ToString("D"));
                    if (acl.Contains(tmp) == false)
                    {
                        NotifyHelper.NotifyUser("不好意思，您没有权限");
                        return;
                    }
#warning 发布时要加入密码验证
                    // 验证密码
                    //if(PswdHelper.EncryptString(textBoxPwd.Text)!=dr["password"].ToString())
                    //{
                    //    NotifyHelper.NotifyUser("用户名或密码有误");
                    //    return;
                    //}

                    this.DialogResult = DialogResult.OK;
                    empId = textBoxUserId.Text;
                    this.Close();
                }
                else
                {
                    NotifyHelper.NotifyUser("用户名或密码有误");
                    return;
                }
            }
            catch (Exception ex)
            {
                NotifyHelper.NotifyUser(ex.ToString());
            }
            finally
            {
                if (conn != null)
                {
                    conn.Close();
                }
            }
        }

        private void buttonCancle_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}