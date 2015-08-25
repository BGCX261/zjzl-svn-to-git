using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace zjzl
{
    public static class NotifyHelper
    {
        /// <summary>
        /// ����Ϊ"(O-O)"��ͼ��ΪWarning��MessageBox
        /// </summary>
        /// <param name="msg"></param>
        public static void NotifyUser(string msg)
        {
            if (!string.IsNullOrEmpty(msg))
            {
                MessageBox.Show(msg, "(O-O)", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

    }
}
