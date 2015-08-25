using System;
using System.Collections.Generic;
using System.Xml;
using System.IO;

namespace FounderSoftware.ADIM.OA.OA2DP
{
    class CDevolveCfg
    {
        private XmlDocument m_Cfgdoc = new XmlDocument();
        private XmlDocument m_DevDoc = new XmlDocument();

        public XmlDocument XmlDoc
        {
            get { return m_DevDoc; }
            set { m_DevDoc = value; }
        }

        public CDevolveCfg(string sPath)
        {
            if (File.Exists(sPath))
                m_Cfgdoc.Load(sPath);
            else
                throw new Exception("创建失败");
        }

        public string GetOtherNodeValues(string sXPath)
        {
            XmlNode node = m_Cfgdoc.SelectSingleNode(sXPath);
            if (node != null)
            {
                return node.InnerText;
            }
            return "";
        }

        public void SetPublicNodeValue()
        {
            XmlNodeList nodes = m_Cfgdoc.SelectNodes("/Devolve/Public");
            for (int i = 0; i < nodes.Count; i++)
            {
                for (int j = 0; j < nodes[i].ChildNodes.Count; j++)
                {
                    if (nodes[i].ChildNodes[j].Attributes == null || nodes[i].ChildNodes[j].Attributes.Count == 0) continue;
                    XmlNode node = m_DevDoc.SelectSingleNode(nodes[i].ChildNodes[j].Attributes["XPath"].Value);
                    if (node != null)
                    {
                        node.InnerText = nodes[i].ChildNodes[j].InnerText;
                    }
                }
            }
        }

        public void SetPrivateNodeValue(string sProcess)
        {
            XmlNodeList nodes = m_Cfgdoc.SelectNodes("/Devolve/Process[@Name='" + sProcess + "']/Private");
            for (int i = 0; i < nodes.Count; i++)
            {
                for (int j = 0; j < nodes[i].ChildNodes.Count; j++)
                {
                    if (nodes[i].ChildNodes[j].Attributes == null || nodes[i].ChildNodes[j].Attributes.Count == 0) continue;
                    XmlNode PrivateNode = m_DevDoc.SelectSingleNode(nodes[i].ChildNodes[j].Attributes["XPath"].Value);
                    if (PrivateNode != null)
                    {
                        PrivateNode.InnerText = nodes[i].ChildNodes[j].InnerText;
                    }
                }
            }
        }

        public List<DevKVItem> MapFunction(string sDevPlat, string sProcName)
        {
            List<DevKVItem> ls = new List<DevKVItem>();

            XmlNodeList nodes = m_Cfgdoc.SelectNodes("/Devolve/Process[@Name='" + sProcName + "']/Map/" + sDevPlat + "/Node");
            for (int i = 0; i < nodes.Count; i++)
            {
                try
                {
                    DevKVItem kvitem = new DevKVItem();
                    string sAttrName = nodes[i].Attributes[0].Value;
                    string sType = "";
                    if (nodes[i].Attributes.Count >= 2)
                    {
                        sType = nodes[i].Attributes[1].Value;
                    }
                    string sEnAttr = nodes[i].SelectSingleNode("/Devolve/Process[@Name='" + sProcName + "']/Map/" + sDevPlat + "/Node[@Name='" + sAttrName + "']").InnerText;
                    kvitem.sKey = sAttrName;
                    kvitem.sValue = sEnAttr;
                    kvitem.sType = sType;
                    ls.Add(kvitem);
                }
                catch
                {
                    continue;
                }
            }
            return ls;
        }
    }

    public class DevKVItem
    {
        private string _sKey;

        public string sKey
        {
            get { return _sKey; }
            set { _sKey = value; }
        }
        private string _sValue;

        public string sValue
        {
            get { return _sValue; }
            set { _sValue = value; }
        }

        private string _type;
        public string sType
        {
            get { return _type; }
            set { _type = value; }
        }
    }
}
