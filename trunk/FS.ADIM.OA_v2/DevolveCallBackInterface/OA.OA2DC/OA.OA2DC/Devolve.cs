using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using FounderSoftware.Framework.Business;

namespace OA.DEVOVLE.OA2DC
{
    public class CDevolve
    {
        /// <summary>
        /// 操作DB_ADIMQS中的T_OA_Devolve表
        /// </summary>
        /// <param name="SerialNo"></param>
        /// <param name="Approve"></param>
        /// <param name="RejectReason"></param>
        /// <param name="Accepter"></param>
        /// <returns></returns>
        static public string CallBack(int SerialNo, bool Approve, string RejectReason)
        {
            return CallBack(SerialNo, Approve, RejectReason, "");
        }

        static public string CallBack(int SerialNo, bool Approve, string RejectReason, string Accepter)
        {
            // 根据传入的参数更新库DB_ADIMQS中的T_OA_Devolve表
            try
            {
                EntityOADevolveHistory entity = new EntityOADevolveHistory();

                string sSQL = "Update " + EntityOADevolveHistory.TABLE_NAME
                    + " Set EditDate = GETDATE(), DevolveStatus = " + Convert.ToInt32(Approve)
                    + ", RejectReason = '" + RejectReason
                    + "', ProcessDate = GETDATE() "
                    + ", Accepter = '" + Accepter + "'"
                    + " Where SerialNo = " + SerialNo.ToString() + ";";

                int iAffected = Entity.RunNoQuery( sSQL, "Sql",
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
    }
}
