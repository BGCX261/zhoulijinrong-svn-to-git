using System;
using System.Xml;
using System.Configuration;
using FounderSoftware.Framework.Business;

namespace FounderSoftware.ADIM.OA.OA2DP
{
    //TODO: 创建数据库表
    //TODO: 创建对应表实体
    //TODO: 封装DP2OA接口
    //TODO: WEB.CONFIG写入DP数据库参数
    public class OA2DP
    {
        XmlDocument m_doc = new XmlDocument();

        private string _XmlStr = "";
        private int _ID = 0;

        private int SetEntity( int iDevolveStatus,
                               int FK_ArchiveId,
                               string sNo,
                               int iOAID,
                               string RejectReason,
                               int iSerialNo,
                               string sTransferXML,
                               string sFormsData,
                               string sProcName)
        {
            EntityOADevolveHistory entity = new EntityOADevolveHistory();
            try
            {
                entity.DevolveDate = DateTime.Now;
                entity.DevolveStatus = iDevolveStatus;
                entity.FK_ArchiveId = FK_ArchiveId;
                entity.No = sNo;
                entity.OAID = iOAID;
                entity.ProcessDate = DateTime.Now;
                entity.RejectReason = RejectReason;
                entity.SerialNo = iSerialNo;
                entity.TransferXML = sTransferXML;
                entity.FormsData = sFormsData;
                entity.ProcessName = sProcName;
                entity.Save();

                return _ID = entity.ID;
            }
            catch
            {
                return 0;
            }
        }

        public string SendDevolve(string sXml, string sFormsData, string sProcessName)
        {
            _XmlStr = sXml;
            string result = DP2OA.DP2OA.DevolveReceive(_XmlStr);
            int iSerielNo = 0;

            string sDevolveStatus = GetValueOfXML("/Devolve/Archive/DevolveStatus");
            string sFK_ArchiveID = GetValueOfXML("/Devolve/Archive/File/FK_ArchiveID");
            string sNo = GetValueOfXML("/Devolve/Archive/No");

            string[] Arr = GetValueOfXML("/Devolve/Params/System").Split('_');
            string sOAID = "";
            if (Arr.Length == 2)
                sOAID = Arr[1];//GetValueOfXML("/Devolve/Archive/OAID");

            if (string.IsNullOrEmpty(sDevolveStatus)) sDevolveStatus = "0";
            if (string.IsNullOrEmpty(sFK_ArchiveID)) sFK_ArchiveID = "0";
            if (string.IsNullOrEmpty(sOAID)) sOAID = "0";
            if ((iSerielNo = DP2OA.ExtendMethod.ToInt(result)) < 0)
            {
                // TODO:归档失败处理
                //SetEntity(Convert.ToInt32(sDevolveStatus), Convert.ToInt32(sFK_ArchiveID),
                //          sNo, Convert.ToInt32(sOAID), result, 0, _XmlStr);

                return result;
            }
            //TODO:归档成功处理
            SetEntity(Convert.ToInt32(sDevolveStatus), Convert.ToInt32(sFK_ArchiveID),
                      sNo, Convert.ToInt32(sOAID), "", iSerielNo, _XmlStr, sFormsData, sProcessName);
            return result;
        }
        
        /// <summary>
        /// 操作DB_ADIMQS中的T_OA_Devolve表
        /// </summary>
        /// <param name="SerialNo"></param>
        /// <param name="Approve"></param>
        /// <param name="RejectReason"></param>
        /// <returns></returns>
        public string CallBack(int SerialNo, bool Approve, string RejectReason)
        {
            // 根据传入的参数更新库DB_ADIMQS中的T_OA_Devolve表
            try
            {
                //EntityOADevolveHistory entity = new EntityOADevolveHistory();

                string sSQL = "Update " + EntityOADevolveHistory.TABLE_NAME
                    + " Set EditDate = GETDATE(), DevolveStatus = " + Convert.ToInt32(Approve) 
                    + ", RejectReason = '" + RejectReason 
                    + "', ProcessDate = GETDATE() "
                    + " Where SerialNo = " + SerialNo.ToString() + ";";

                int iAffected = Entity.RunNoQuery(sSQL, "Sql",
                                                    ConfigurationManager.AppSettings["OA_DataSource"],
                                                    ConfigurationManager.AppSettings["OA_DataBase"],
                                                    ConfigurationManager.AppSettings["OA_User"],
                                                    ConfigurationManager.AppSettings["OA_Pwd"]);

                return "true";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        /// <summary>
        /// 参数sNodeName使用XML的XPATH
        /// </summary>
        /// <param name="sNodeName">待获取节点值XPATH路径</param>
        /// <returns>若找到则返回对应节点值,否则返回空</returns>
        private string GetValueOfXML(string sNodeName)
        {
            string sRet = "";
            if (string.IsNullOrEmpty(_XmlStr)) return "";

            m_doc.LoadXml(_XmlStr);
            XmlNode node = m_doc.SelectSingleNode(sNodeName);
            if (node != null)
                sRet = node.InnerText;

            return sRet;
        }
    }
}
