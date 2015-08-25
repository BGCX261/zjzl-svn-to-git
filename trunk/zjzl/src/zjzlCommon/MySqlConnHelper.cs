using System;
using System.Collections.Generic;
using System.Text;

using MySql.Data.MySqlClient;

namespace zjzl
{
    public class MySqlConnHelper
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="connStr">���ܹ��������ַ���</param>
        /// <returns></returns>
        /// <exception cref="System.NullReferenceException"></exception>
        /// <exception cref="System.Execption"></exception>
        public static MySqlConnection GetMySqlConn(string connStr)
        {
            if(string.IsNullOrEmpty(connStr))
            {
                throw new System.NullReferenceException("�����ֲ���Ϊ��");
            }
            string tmp = PswdHelper.DecryptString(connStr);
            string[] ss = tmp.Split(new char[] { ':' }, StringSplitOptions.RemoveEmptyEntries);
            if(ss.Length!=5)
            {
                throw new Exception("�����ָ�ʽ����");
            }

            MySqlConnectionStringBuilder connBuilder = new MySqlConnectionStringBuilder();

            connBuilder.Add("User Id", ss[0]);
            connBuilder.Add("Password", ss[1]);
            connBuilder.Add("Data Source", ss[2]);
            connBuilder.Add("Database", ss[3]);
            connBuilder.Add("Charset", ss[4]);

            MySqlConnection connection =
                new MySqlConnection(connBuilder.ConnectionString);

            return connection;
        }

    }
}
