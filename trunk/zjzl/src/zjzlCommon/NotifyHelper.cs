using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace zjzl
{
    public static class NotifyHelper
    {
        /// <summary>
        /// 标题为"(O-O)"、图标为Warning的MessageBox
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
