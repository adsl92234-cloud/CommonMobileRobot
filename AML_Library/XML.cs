using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.IO;
using System.Windows.Forms;
using System.Collections;

namespace AML
{
    class XML
    {  
        public static void CreateFile(string file)
        {
            XmlDocument DOC = new XmlDocument();
            XmlElement ROOT = DOC.CreateElement(file);
            DOC.AppendChild(ROOT);

            DOC.Save(file+".xml");

        }
        public static void CreateNode(string file, string node, string node1)
        {
            XmlDocument DOC = new XmlDocument();
            DOC.Load(file+".xml");

            XmlNode NODE = DOC.SelectSingleNode(node);
            if (NODE == null)
                return;

            XmlElement NODE1 = DOC.CreateElement(node1);
            NODE.AppendChild(NODE1);

            DOC.Save(file+".xml");
        }
        public static void CreateStatement(string file, string node, string title, string statement1, string statement2)
        {
            XmlDocument DOC = new XmlDocument();
            DOC.Load(file+".xml");

            XmlNode NODE = DOC.SelectSingleNode(node);
            if (NODE == null)
                return;

            XmlElement STATEMENT = DOC.CreateElement(title);
            STATEMENT.SetAttribute(statement1, statement2);
            NODE.AppendChild(STATEMENT);

            DOC.Save(file+".xml");
        }
        public static string Read(string file , string node ,string defaultValue)
        {
            XmlDocument DOC = new XmlDocument();
            try
            {
                DOC.Load(file + ".xml");
                XmlNode NODE = DOC.SelectSingleNode(node);
                if (NODE == null)
                    throw new Exception();

                XmlElement ELEMENT = (XmlElement)NODE;
                string data = ELEMENT.GetAttribute("value");

                return data;
            }
            catch (Exception ex)
            {
                return defaultValue;
            }
        }
        public static string Read(string node, string defaultValue)
        {
            XmlDocument DOC = new XmlDocument();
            try
            {
                DOC.Load("Setting.xml");
                XmlNode NODE = DOC.SelectSingleNode(node);
                if (NODE == null)
                    throw new Exception();

                XmlElement ELEMENT = (XmlElement)NODE;
                string data = ELEMENT.GetAttribute("value");

                return data;
            }
            catch
            {
                return defaultValue;
            }
        }
        public static void Delete(string file, string node)
        {
            XmlDocument DOC = new XmlDocument();
            DOC.Load(file+".xml");

            XmlNode NODE = DOC.SelectSingleNode(node);
            if (NODE == null)
                return;

            XmlElement DELETE = (XmlElement) NODE;
            DELETE.RemoveAll();

            DOC.Save(file+".xml");
        }

        // test
        public static void ReWrite(string file, string node, string target, string statement)
        {
            XmlDocument DOC = new XmlDocument();
            DOC.Load(file+".xml");

            XmlNode NODE = DOC.SelectSingleNode(node);
            if (NODE == null)
                return;

            XmlElement ELEMENT = (XmlElement)NODE;
            string data = ELEMENT.GetAttribute(target);
            XmlAttribute attribute = ELEMENT.GetAttributeNode(target);
            XmlAttributeCollection attributes = ELEMENT.Attributes;

            string space = "";

            foreach(XmlAttribute item in attributes)
            {
                space += item.Name + "," + item.Value + Environment.NewLine;
                if (item.Name == target)
                    item.Value = statement;

            }

            DOC.Save(file+".xml");
}
        public static string[] GetAllNodes(string file)
        {
            try
            {
                //Create the XmlDocument.
                List<string> lstlevel = new List<string>();
                XmlDocument document = new XmlDocument();
                document.Load(file + ".xml");
                foreach (XmlNode node in document.GetElementsByTagName("Setting"))
                {
                    foreach (XmlNode node1 in node.ChildNodes)
                    {
                        foreach (XmlNode node2 in node1.ChildNodes)
                        {
                            lstlevel.Add(node1.Name + "," + node2.Name);
                        }
                    }
                }
                return lstlevel.ToArray();
            }
            catch
            {
                return null;
            }
        }
 public static void UpdataNodeValue(string file,string node,string value)
        {
            XmlDocument DOC = new XmlDocument();
            DOC.Load(file + ".xml");

            XmlNode NODE = DOC.SelectSingleNode(node);
            if (NODE == null)
                return;

            XmlElement ELEMENT = (XmlElement)NODE;
            ELEMENT.SetAttribute("value", value);
            DOC.Save(file + ".xml");
        }
    }
}
