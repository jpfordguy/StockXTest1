using System;
using System.IO;
using System.Xml;

namespace StockXTest1
{

    class DatabaseHelper
    {
        public static PostgresInterface DatabaseInterface = null;


        public static string Server = "localhost";
        public static int Port = 5432;
        public static string UserId = "postgres";
        public static string Password = "password";


        public static bool LoadSettings()
        {
            string file = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "settings.xml");
            XmlDocument doc = new XmlDocument();
            try
            {
                doc.Load(file);
            }
            catch
            {
                return false;
            }
            if (doc.ChildNodes == null)
            {
                return false;
            }
            if (doc.ChildNodes.Count != 1)
            {
                return false;
            }
            XmlNode root = doc.ChildNodes[0];
            if (root.Name != "settings")
            {
                return false;
            }
            if (root.ChildNodes == null)
            {
                return false;
            }
            if (root.ChildNodes.Count != 4)
            {
                return false;
            }
            XmlNode node = root.ChildNodes[0];
            int count = 0;
            while (node != null)
            {
                if (node.Name == "server")
                {
                    if (node.InnerText.Trim() == "")
                    {
                        return false;
                    }
                    Server = node.InnerText.Trim();
                    count++;
                }
                if (node.Name == "port")
                {
                    int tmpint = 0;
                    if (!int.TryParse(node.InnerText.Trim(), out tmpint))
                    {
                        return false;
                    }
                    if (tmpint < 1)
                    {
                        return false;
                    }
                    Port = tmpint;
                    count++;
                }
                if (node.Name == "userid")
                {
                    if (node.InnerText.Trim() == "")
                    {
                        return false;
                    }
                    UserId = node.InnerText.Trim();
                    count++;
                }
                if (node.Name == "password")
                {
                    if (node.InnerText.Trim() == "")
                    {
                        return false;
                    }
                    Password = node.InnerText.Trim();
                    count++;
                }
                node = node.NextSibling;
            }
            if (count != 4)
            {
                return false;
            }
            return true;
        }


        public static bool SaveSettings()
        {
            string file = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "settings.xml");
            XmlDocument doc = new XmlDocument();
            XmlNode root = doc.CreateElement("settings");
            doc.AppendChild(root);

            XmlNode node = doc.CreateElement("server");
            node.InnerText = Server;
            root.AppendChild(node);

            node = doc.CreateElement("port");
            node.InnerText = Port.ToString();
            root.AppendChild(node);

            node = doc.CreateElement("userid");
            node.InnerText = UserId;
            root.AppendChild(node);

            node = doc.CreateElement("password");
            node.InnerText = Password;
            root.AppendChild(node);
            try
            {
                doc.Save(file);
            }
            catch
            {
                return false;
            }
            return true;
        }
    }
}
