using System;
using System.Data;
using System.Text;
using FS.ADIM.OA.BLL;
using System.Web;
using FS.ADIM.OA.BLL.Busi.Menu;
using FS.ADIM.OA.BLL.Entity;
using FS.ADIM.OA.BLL.Common;
using FS.ADIM.OA.BLL.Busi;
using FS.ADIM.OA.BLL.Busi.Process;
using FounderSoftware.ADIM.SSO.Utility;

namespace FS.ADIM.OA.WebUI
{
    public partial class Left : System.Web.UI.Page
    {
        protected String l_strDraftCount = "0"; //草稿箱
        protected String m_strWaitHandleCount = "0"; //待办文件
        protected String m_strCommonWaitHandleCount = "0"; //公办文件
        protected String m_strWaitReadCount = "0"; //待阅文件

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                try
                {
                    GetAllCount();

                    LoadMenuList();
                }
                catch (Exception ex)
                {

                }
            }
        }

        private void GetAllCount()
        {
            //草稿箱文件数目
            B_DraftBox l_busDraftBox = new B_DraftBox();
            l_strDraftCount = l_busDraftBox.GetDraftCount(CurrentUserInfo.UserName);

            //待办文件数目
            B_TaskFile l_busTaskFile = new B_TaskFile();
            M_EntityMenu m_TaskFileSearchCondition = new M_EntityMenu();
            m_TaskFileSearchCondition.LoginUserID = CurrentUserInfo.UserName;
            m_strWaitHandleCount = l_busTaskFile.GetWaitingHandleCount(m_TaskFileSearchCondition);

            //公办文件数目
            B_CommonTaskFile l_busCommonTaskFile = new B_CommonTaskFile();
            M_EntityMenu m_GongBanFile = new M_EntityMenu();
            m_GongBanFile.LoginUserID = CurrentUserInfo.UserName;
            m_strCommonWaitHandleCount = l_busCommonTaskFile.GetCommonWaitingHandleCount(m_GongBanFile);

            //待阅文件
            M_EntityMenu mSearchCond = new M_EntityMenu();
            mSearchCond.LoginUserID = CurrentUserInfo.UserName;
            mSearchCond.Is_Read = 0;
            B_Circulate l_busCirculate = new B_Circulate(String.Empty);
            m_strWaitReadCount = l_busCirculate.GetWaitingReadCount(mSearchCond);
        }

        private void LoadMenuList()
        {
            String url = "Container.aspx?ClassName=FS.ADIM.OA.WebUI.";
            String path = "";
            bool isAdmin = true;
            //获得菜单
            String sPath = HttpContext.Current.Server.MapPath((@"~\Config\WFDBConfig.xml"));

            string role = SysString.GetStringFormatForList(CurrentUserInfo.RoleName, ",");

            if (role.IndexOf("系统管理员") > 0)
            {
                isAdmin = true;
            }
            else
            {
                isAdmin = false;
            }

            DataTable l_dtrDataTable = LoadMenu.GetMenu(sPath,isAdmin);
            DataRow[] l_dtrTopFloor = l_dtrDataTable.Select("FloorCode=" + 1); //一级菜单

            StringBuilder l_stbMergeString = new StringBuilder(); //最终菜单HTML

            String l_strDisplayCount = ""; //显示的待办公办的数字
            int topFloorLength = l_dtrTopFloor.Length;

            if (CurrentUserInfo.LoginName.Equals("pengsj") || CurrentUserInfo.LoginName.Equals("zhanglz") || CurrentUserInfo.LoginName.Equals("yangxj") || CurrentUserInfo.LoginName.Equals("zhulei"))
            {
                topFloorLength = 3;
            }

            for (int i = 0; i < topFloorLength; i++)
            {
                l_stbMergeString.AppendFormat(@"<div class='topFolder' id='Div{0}'><img src='Img/menu_bt.gif' border='0' alt='{1}'>{1}</img></div>", (i + 1).ToString(), l_dtrTopFloor[i]["Name"].ToString());

                DataRow[] l_dtrChildFloor = l_dtrDataTable.Select("ParentID=" + l_dtrTopFloor[i]["ID"].ToString()); //子菜单
                l_stbMergeString.AppendFormat(@"<div class='sub' id='Div{0}Sub'>", (i + 1).ToString());
                for (int j = 0; j < l_dtrChildFloor.Length; j++)
                {
                    path = l_dtrChildFloor[j]["Path"].ToString();
                    if (!path.Contains(url))
                    {
                        path = url + path;
                    }
                    switch (l_dtrChildFloor[j]["Name"].ToString())
                    {
                        case "待办文件": l_strDisplayCount = "(" + m_strWaitHandleCount + ")"; break;
                        case "公办文件": l_strDisplayCount = "(" + m_strCommonWaitHandleCount + ")"; break;
                        case "待阅文件": l_strDisplayCount = "(" + m_strWaitReadCount + ")"; break;
                        case "草稿箱": l_strDisplayCount = "(" + l_strDraftCount + ")"; break;
                        default: l_strDisplayCount = ""; break;
                    }
                    l_stbMergeString.AppendFormat(@"<div class='subItem' onmouseover='ItemMouseOver(this);' onmouseout='ItemMouseOut(this);'><a href='{0}'  target='main'><img src='Img/menu_list.gif' border='0' alt='{1}' />{1}{2}</a></div>", path, l_dtrChildFloor[j]["Name"].ToString(), l_strDisplayCount);
                }
                l_stbMergeString.AppendFormat("</div>");
            }
            lblMenu.Text = l_stbMergeString.ToString();
        }
    }
}
