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
        /// <param name="conn">�������ܵ����ݿ�������</param>
        /// <param name="pro">��Ʒ����</param>
        /// <exception cref="System.NullReferenceException">connΪnull����Empty���׳����쳣</exception>
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
            this.Text = "��¼��" + this.product.ToString();
        }

        private void buttonLogin_Click(object sender, EventArgs e)
        {
            #region ��鲻Ϊ��
            textBoxUserId.Text = textBoxUserId.Text.Trim();
            textBoxPwd.Text = textBoxPwd.Text.Trim();
            if (string.IsNullOrEmpty(textBoxUserId.Text))
            {
                NotifyHelper.NotifyUser("����");
                return;
            }
            if (string.IsNullOrEmpty(textBoxPwd.Text))
            {
                NotifyHelper.NotifyUser("����");
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
                        NotifyHelper.NotifyUser("������˼����û��Ȩ��");
                        return;
                    }
#warning ����ʱҪ����������֤
                    // ��֤����
                    //if(PswdHelper.EncryptString(textBoxPwd.Text)!=dr["password"].ToString())
                    //{
                    //    NotifyHelper.NotifyUser("�û�������������");
                    //    return;
                    //}

                    this.DialogResult = DialogResult.OK;
                    empId = textBoxUserId.Text;
                    this.Close();
                }
                else
                {
                    NotifyHelper.NotifyUser("�û�������������");
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