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
    public partial class InfoForm : Form
    {
        string customerID;
        public int personID = -1;
        PurchaseForm UI;

        public InfoForm(string customerID, PurchaseForm purForm)
        {
            InitializeComponent();
            this.customerID = customerID;
            UI = purForm;
        }

        private void buttonClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void InfoForm_Shown(object sender, EventArgs e)
        {
            MySqlConnection conn = null;
            try
            {
                conn = MySqlConnHelper.GetMySqlConn(Properties.Settings.Default.DbConn);
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = @"select t.id as person_id, t.name as person_name, t.upper as person_upper,
t.upper_used as person_upper_used, s.name as org_name from pur_customer t, pur_organization s 
where t.tag=?tag and t.org_id=s.id;";
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("?tag", customerID);
                
                conn.Open();
                MySqlDataReader dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    StringBuilder sb = new StringBuilder();
                    sb.Append("����: ");
                    sb.AppendLine(dr["person_name"].ToString());
                    sb.Append("�����޶�: ");
                    sb.AppendLine(dr["person_upper"].ToString());
                    sb.Append("��ʹ���޶�: ");
                    sb.AppendLine(dr["person_upper_used"].ToString());
                    sb.Append("������֯: ");
                    sb.AppendLine(dr["org_name"].ToString());
                    richTextBox1.Text = sb.ToString();

                    personID = int.Parse(dr["person_id"].ToString());
                }
                else
                {
                    richTextBox1.Text = "δ��ϵͳ���ҵ�������";
                }
                dr.Close();

            }
            catch (MySqlException sqlEx)
            {
                UI.WriteLog(sqlEx.ToString());
                NotifyHelper.NotifyUser("�޷����ӷ�����: " + sqlEx.Message);

            }
            catch (Exception ex)
            {
                UI.WriteLog(ex.ToString());
                NotifyHelper.NotifyUser("��ȡ����ʧ��: " + ex.Message);
            }
            finally
            {
                if (conn != null)
                {
                    conn.Close();
                }
            }
        }
    }
}