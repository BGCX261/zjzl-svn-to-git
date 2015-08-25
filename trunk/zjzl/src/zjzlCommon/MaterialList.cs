using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using System.IO;


namespace zjzl
{
    [Serializable]
    public struct GradeAndPrice
    {
        public string grade;
        /// <summary>
        /// ิช/ถึ
        /// </summary>
        public string price;
    }

    [Serializable]
    public class Material
    {
        public string name;
        public List<GradeAndPrice> grades = null;

        public override string ToString()
        {
            return Material.Serialize(this);
        }

        public static string Serialize(Material mat)
        {
            StringWriter sw = new StringWriter();
            XmlSerializer ser = new XmlSerializer(typeof(Material));
            ser.Serialize(sw, mat);
            return sw.ToString();
        }

        public static Material Deserialize(string s)
        {
            StringReader sr = new StringReader(s);
            XmlSerializer ser = new XmlSerializer(typeof(Material));
            Material mat = (Material)ser.Deserialize(sr);
            sr.Close();
            return mat;
        }
    }

    [Serializable]
    public class MaterialList
    {
        public List<Material> materialList = null;

        public static string Serialize(MaterialList mats)
        {
            StringWriter sw = new StringWriter();
            XmlSerializer ser = new XmlSerializer(typeof(MaterialList));
            ser.Serialize(sw, mats);
            return sw.ToString();
        }

        public static MaterialList Deserialize(string s)
        {
            StringReader sr = new StringReader(s);
            XmlSerializer ser = new XmlSerializer(typeof(MaterialList));
            MaterialList mats = (MaterialList)ser.Deserialize(sr);
            sr.Close();
            return mats;
        }

    }
}
