using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace zjzl
{
    public class LogHelper
    {
        private static string dir="log";
        private DateTime presentDate = DateTime.Now;
        private Products product = Products.none;
        private bool inited = false;
        string path;
        string dateFormat = "yyyyMMdd";
        string timeFormat = "HH:mm:ss";
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="product">请勿传递Products.none</param>
        /// <exception cref="System.Exception"></exception>
        public LogHelper(Products prod)
        {
            product = prod;
            if(Directory.Exists(dir)==false)
            {
                Directory.CreateDirectory(dir);
            }
            path = string.Format("{0}{1}{2}_{3}.txt", dir, Path.DirectorySeparatorChar,
                presentDate.ToString(dateFormat), product.ToString());

            inited = true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="s">日志内容</param>
        /// <exception cref="System.Exception"></exception>
        public void WriteLog(string s)
        {
            if(inited==false)
            {
                return;
            }

            //if(presentDate.Date==DateTime.Now.Date)
            //{

            //}
            //else
            //{
            //    presentDate = DateTime.Now;
            //}

            //string path = string.Format("{0}{1}{2}_{3}.txt", dir, Path.DirectorySeparatorChar,
            //    presentDate.ToString("yyyyMMdd"), product.ToString());
            DateTime present = DateTime.Now;
            string tmp = string.Format("{0} {1}{2}", present.ToString(timeFormat), s, Environment.NewLine);
            File.AppendAllText(path, tmp);
        }
    }
}
