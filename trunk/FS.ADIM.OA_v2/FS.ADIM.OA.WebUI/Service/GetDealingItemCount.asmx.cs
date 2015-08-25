using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using FS.ADIM.OA.BLL;
using FS.ADIM.OA.BLL.Entity;
using FS.ADIM.OA.BLL.Busi.Menu;
using FS.ADIM.OA.BLL.Busi;
using FS.ADIM.OA.BLL.Busi.Process;

namespace FS.ADIM.OA.WebUI.Service
{
    /// <summary>
    /// GetDealingItemCount 的摘要说明
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // 若要允许使用 ASP.NET AJAX 从脚本中调用此 Web 服务，请取消对下行的注释。
    // [System.Web.Script.Services.ScriptService]
    public class GetDealingItemCount : System.Web.Services.WebService
    {

        [WebMethod]
        public string HelloWorld()
        {
            return "Hello World";
        }

        /// <summary>
        /// 获取所有事项的条数
        /// </summary>
        /// <returns></returns>
        [WebMethod]
        public string[] GetDealItemCount(string userName)
        {
            try
            {
                string waitTaskCount = "0";
                string commonTaskCount = "0";
                string waitReadCount = "0";

                string completedTaskCount = "0";
                string completedReadCount = "0";

                //待办文件数目
                B_TaskFile l_busTaskFile = new B_TaskFile();
                M_EntityMenu searchCondition = new M_EntityMenu();
                searchCondition.LoginUserID = userName;
                waitTaskCount = l_busTaskFile.GetWaitingHandleCount(searchCondition);

                //公办文件数目
                B_CommonTaskFile l_busCommonTaskFile = new B_CommonTaskFile();
                M_EntityMenu m_GongBanFile = new M_EntityMenu();
                m_GongBanFile.LoginUserID = userName;
                commonTaskCount = l_busCommonTaskFile.GetCommonWaitingHandleCount(m_GongBanFile);

                //待阅文件
                M_EntityMenu mSearchCond = new M_EntityMenu();
                B_Circulate l_busCirculate = new B_Circulate(String.Empty);
                mSearchCond.LoginUserID = userName;
                mSearchCond.Is_Inbox = false;
                mSearchCond.Is_Read = 0;
                waitReadCount = l_busCirculate.GetWaitingReadCount(mSearchCond);

                //已办
                B_CompletedTaskFile l_busCompletedTaskFile = new B_CompletedTaskFile();
                M_CompleteFile m_CompleteFile = new M_CompleteFile();
                m_CompleteFile.LoginUserID = userName;
                completedTaskCount = l_busCompletedTaskFile.GetCompletedFileCount(m_CompleteFile);

                //已阅
                mSearchCond.Is_Read = 1;
                completedReadCount = l_busCirculate.GetWaitingReadCount(mSearchCond);

                string[] strArr = new string[5];
                strArr[0] = waitTaskCount;
                strArr[1] = commonTaskCount; //公办
                strArr[2] = waitReadCount;
                strArr[3] = completedTaskCount;
                strArr[4] = completedReadCount;
                return strArr;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
