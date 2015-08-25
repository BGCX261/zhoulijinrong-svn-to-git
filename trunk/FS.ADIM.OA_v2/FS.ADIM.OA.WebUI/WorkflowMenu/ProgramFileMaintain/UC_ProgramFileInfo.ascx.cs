/*****************************************************************/
// Copyright (C) 2010 方正国际软件有限公司
//
// 文件功能描述：程序文件维护
//
// 创 建 者：黄 琦
// 创建标识: C_2010.01.12
//
// 修改标识：renjinquan 2010-5-10
// 修改描述：修改SelectRowBindDetail函数，去除this.txtCode.Text使用GetText函数转换字符串

// 修改标识：renjinquan 2010-5-11
// 修改描述：1.修改GetSearchList函数，主持带'等的查询
//           2.修改SelectRowBindDetail函数，选择列表绑定数据时,对'&lt;','&rt;'等转换
/*****************************************************************/
using System;
using System.Data;
using System.Web.UI.WebControls;
using FounderSoftware.Framework.UI.WebPageFrame;
using FounderSoftware.Framework.UI.WebCtrls;
using FS.ADIM.OA.BLL.Busi.InfoMaintain;
using FS.ADIM.OA.BLL.Common;
using FS.OA.Framework.WorkFlow.AgilePoint;
using Ascentn.Workflow.Base;
using FS.ADIM.OA.BLL.Busi.Process;
using FS.ADIM.OU.OutBLL;
using FS.ADIM.OA.BLL;

namespace FS.ADIM.OA.WebUI.WorkflowMenu.ProgramFileMaintain
{
    public partial class UC_ProgramFileInfo : UCBase
    {
        #region GridView单元格定义
        //编码
        private const int CODE = 1;
        //版次
        private const int EDITION = 2;
        //文件名称
        private const int FILENAME = 3;
        //程序二级子类
        private const int SUBTYPE = 4;
        //程序一级子类
        private const int TYPE = 5;
        //程序分类
        private const int SORT = 6;
        //ArchiveState
        private const int ARCHIVESTATE = 7;
        //ApplyStyle
        private const int APPLYSTYLE = 8;
        //ID
        private const int KEY_ID = 9;
        //FILES附件实体
        private const int FILES = 10;
        //ProcessID
        private const int PROCESSID = 11;
        //PROTYPEID
        private const int PROTYPEID = 12;
        //PROTYPESUBID
        private const int PROTYPESUBID = 13;
        //重新发起按钮
        private const int RESTART = 14;
        #endregion

        /// <summary>
        /// 页面加载事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (FormsMethod.CheckRole(ConstString.RoleName.QUALITY_MEMBER) == false)
                {
                    lblPrompt.Text = "只有质保人员" + OAUser.GetUserByRoleName(ConstString.RoleName.QUALITY_MEMBER)[1].ToString() + "可以添加程序文件。";
                }
                else
                {
                    lblPrompt.Text = string.Empty;
                }
                txtName.ToolTip = "内容不能超过50个字符";
                txtCode.ToolTip = "内容不能超过20个字符";
                this.gvProgramFileList.ShowPagerRow = true;
                this.gvProgramFileList.PageType = PageType.ExteriorPage;
                BindProgramFileList(false, 0, 0, "", "", "", "", "", "");
                BindProgramSort(ddlSort);
                BindProgramSort(ddlSorts);
            }
        }

        /// <summary>
        /// 绑定程序分类
        /// </summary>
        /// <param name="ddl"></param>
        private void BindProgramSort(DropDownList ddl)
        {
            DataTable dt = B_ProgramSort.GetProgramSort();
            if (dt != null)
            {
                ddl.DataSource = dt;
                ddl.DataValueField = "ID";
                ddl.DataTextField = "Name";
                ddl.DataBind();
                ListItem li = new ListItem();
                li.Text = "--请选择--";
                li.Value = "0";
                ddl.Items.Insert(0, li);
            }
        }

        /// <summary>
        /// 绑定程序类型
        /// </summary>
        /// <param name="ddl"></param>
        /// <param name="sort"></param>
        private void BindProgramType(DropDownList ddl, string sort)
        {
            ddl.Items.Clear();
            B_ProgramType bllProType = new B_ProgramType();
            DataTable dt = bllProType.GetProgramTypeForSelect(sort);
            if (dt != null)
            {
                ddl.DataSource = dt;
                ddl.DataValueField = "ID";
                ddl.DataTextField = "Name";
                ddl.DataBind();
                ListItem li = new ListItem();
                li.Text = "--请选择--";
                li.Value = "0";
                ddl.Items.Insert(0, li);
            }
        }

        /// <summary>
        /// 绑定程序子类
        /// </summary>
        /// <param name="ddl"></param>
        /// <param name="proTypeID"></param>
        private void BindProgramSubType(DropDownList ddl, int typeID)
        {
            ddl.Items.Clear();
            B_ProgramTypeSub bllProTypeSub = new B_ProgramTypeSub();
            DataTable dt = bllProTypeSub.GetProgamSubTypeByTypeID(typeID);
            if (dt != null)
            {
                ddl.DataSource = dt;
                ddl.DataValueField = "ID";
                ddl.DataTextField = "SubTypeName";
                ddl.DataBind();
                ListItem li = new ListItem();
                li.Text = "--请选择--";
                li.Value = "0";
                ddl.Items.Insert(0, li);
            }

        }

        /// <summary>
        /// 绑定列表信息
        /// </summary>
        /// <param name="proTypeID"></param>
        /// <param name="proSubTypeID"></param>
        /// <param name="sort"></param>
        /// <param name="archiveState"></param>
        /// <param name="name"></param>
        /// <param name="code"></param>
        /// <param name="edition"></param>
        /// <param name="applyStyle"></param>
        private void BindProgramFileList(bool isSearch, int proTypeID, int proSubTypeID, string sort, string archiveState, string name, string code, string edition, string applyStyle)
        {
            B_ProgramFileInfo bllProFile = new B_ProgramFileInfo();

            int iStart = 0;
            int iEnd = 0;
            int iCount = 0;
            if (isSearch)
            {
                this.gvProgramFileList.PageIndex = 0;
                iCount = bllProFile.GetCountUseExteriorPage(proTypeID, proSubTypeID, sort, archiveState, name, code, edition, applyStyle, 1, gvProgramFileList.PageSize, ref iStart, ref iEnd);
            }
            else
            {
                iCount = bllProFile.GetCountUseExteriorPage(proTypeID, proSubTypeID, sort, archiveState, name, code, edition, applyStyle, gvProgramFileList.PageIndex + 1, gvProgramFileList.PageSize, ref iStart, ref iEnd);
            }
            DataTable dtList = bllProFile.GetProgamFileUseExteriorPage(proTypeID, proSubTypeID, sort, archiveState, name, code, edition, applyStyle, iStart, iEnd);

            this.gvProgramFileList.RecordCount = iCount;
            //绑定数据
            this.gvProgramFileList.DataSource = dtList;

            this.gvProgramFileList.DataBind();
        }

        protected void gvProgramFileList_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow && e.Row.RowType != DataControlRowType.Footer && e.Row.RowType != DataControlRowType.Header)
            {
                FSLinkButton lnkbtnRestart = (FSLinkButton)e.Row.Cells[RESTART].FindControl("lnkbtnRestart");
                if (!B_PF.IsExistProgramFile(e.Row.Cells[KEY_ID].Text.ToString()) && !string.IsNullOrEmpty(e.Row.Cells[EDITION].Text.Replace("&nbsp;", "").Trim()))
                {
                    lnkbtnRestart.Visible = e.Row.Cells[ARCHIVESTATE].Text.Equals(ConstString.ProgramFile.PROGRAM_ARCHIVED) ||
                        e.Row.Cells[ARCHIVESTATE].Text.Equals(ConstString.ProgramFile.PROGRAM_LOGOUT) ? false : true;
                    
                    if (!string.IsNullOrEmpty(e.Row.Cells[FILES].Text.Replace("&nbsp;", "").ToString()))
                    {
                        lnkbtnRestart.Visible = true;
                        lnkbtnRestart.Text = "附件下载";
                        lnkbtnRestart.OnClientClick = "OpenDetailDialog(" + e.Row.Cells[KEY_ID].Text + ");";
                    }
                }//流程表中不存在程序文件并且版次不为空（未发起流程）
                //else if (!string.IsNullOrEmpty(e.Row.Cells[Files].Text.Replace("&nbsp;", "").ToString()))
                //{
                //    lnkbtnRestart.Visible = true;
                //    lnkbtnRestart.Text = "附件下载";
                //    lnkbtnRestart.OnClientClick = "OpenDetailDialog(" + e.Row.Cells[ID].Text + ");";
                //}
                else
                {
                    if (!string.IsNullOrEmpty(e.Row.Cells[FILES].Text.Replace("&nbsp;", "").ToString()))
                    {
                        lnkbtnRestart.Visible = true;
                        lnkbtnRestart.Text = "附件下载";
                        lnkbtnRestart.OnClientClick = "OpenDetailDialog(" + e.Row.Cells[KEY_ID].Text + ");";
                    }
                    else
                    {
                        lnkbtnRestart.Visible = false;
                    }

                    LinkButton lnkbtnCancel = e.Row.Cells[RESTART].FindControl("lnkbtnCancel") as LinkButton;

                    //已发起流程并且程序状态为“未完成”则显示撤销流程，否则不可撤销
                    if (e.Row.Cells[ARCHIVESTATE].Text != ConstString.ProgramFile.PROGRAM_ARCHIVED &&
                        string.IsNullOrEmpty(Server.HtmlDecode(e.Row.Cells[ARCHIVESTATE].Text).Trim()) == false)
                    {
                        lnkbtnCancel.Visible = true;
                    }
                    else
                    {
                        lnkbtnCancel.Visible = false;
                    }
                }
            }
        }

        /// <summary>
        /// 列表选择行事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvProgramFileList_SelectedIndexChanged(object sender, EventArgs e)
        {
            SelectRowBindDetail();
        }

        /// <summary>
        /// 选择行绑定详细信息
        /// </summary>
        private void SelectRowBindDetail()
        {
            //this.lnkbtnDetail.Enabled = true;
            btnDel.Enabled = true;

            GridViewRow row = this.gvProgramFileList.SelectedRow;

            int index = gvProgramFileList.SelectedIndex;
            hfSelectedIndex.Value = gvProgramFileList.DataKeys[index]["ID"].ToString();

            //程序文件名称
            this.txtName.Text =SysString.GetText(row.Cells[FILENAME].Text.ToString());//renjinquan+ 对'<','>'等转换

            //编码
            this.txtCode.Text = SysString.GetText(row.Cells[CODE].Text);//renjinquan+ 对'&lt;','&rt;'等转换

            //版次
            //this.txtEdition.Text = row.Cells[EDITION].Text.ToString();

            //程序分类dropdownlist
            ListItem liSort = this.ddlSort.Items.FindByText(row.Cells[SORT].Text.ToString());
            ddlSort.ClearSelection();
            if (liSort != null)
            {
                liSort.Selected = true;
                BindProgramType(ddlProgramType, liSort.Text.ToString().Trim());
            }
            else
            {
                ddlSort.SelectedIndex = 0;
            }

            //程序一级子类dropdownlist
            ListItem liProgramType = this.ddlProgramType.Items.FindByValue(row.Cells[PROTYPEID].Text.ToString());
            ddlProgramType.ClearSelection();
            if (liProgramType != null)
            {
                liProgramType.Selected = true;
                BindProgramSubType(ddlProgramSubType, int.Parse(liProgramType.Value.ToString()));
            }
            else
            {
                ddlProgramType.SelectedIndex = 0;
            }

            //程序二级子类dropdownlist
            ListItem liProgramSubType = this.ddlProgramSubType.Items.FindByValue(row.Cells[PROTYPESUBID].Text.ToString());
            ddlProgramSubType.ClearSelection();
            if (liProgramSubType != null)
            {
                liProgramSubType.Selected = true;
            }
            else
            {
                ddlProgramSubType.SelectedIndex = 0;
            }
        }

        /// <summary>
        /// 分类选择事件（用于新增）
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlSort_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlSort.SelectedIndex == 0)
            {
                //清空一级子类
                ClearType(ddlProgramType);
                //清空二级子类
                //ClearSubType(ddlProgramSubType);
            }
            else
            {
                BindProgramType(ddlProgramType, ddlSort.SelectedItem.Text.ToString().Trim());
            }

            //清空二级子类
            ClearSubType(ddlProgramSubType);
        }

        /// <summary>
        /// 程序类型选择事件(用于新增)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlProgramType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlProgramType.SelectedIndex == 0)
            {
                ClearSubType(ddlProgramSubType);
            }
            else
            {
                int proTypeId = int.Parse(ddlProgramType.SelectedItem.Value.ToString().Trim());
                BindProgramSubType(ddlProgramSubType, proTypeId);
            }
        }

        /// <summary>
        /// 清空一级子类
        /// </summary>
        private void ClearType(DropDownList ddl)
        {
            ddl.Items.Clear();
            ListItem li = new ListItem();
            li.Text = "请选择类型";
            li.Value = "0";
            ddl.Items.Insert(0, li);
        }

        /// <summary>
        /// 清空二级子类
        /// </summary>
        private void ClearSubType(DropDownList ddl)
        {
            ddl.Items.Clear();
            ListItem li = new ListItem();
            li.Text = "请选择一级子类";
            li.Value = "0";
            ddl.Items.Insert(0, li);
        }

        /// <summary>
        /// 新增按钮事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnAdd_Click(object sender, EventArgs e)
        {
            if (FormsMethod.CheckRole(ConstString.RoleName.QUALITY_MEMBER) == false)
            {
                IMessage ms = new WebFormMessage(Page, "只有质保人员" + OAUser.GetUserByRoleName(ConstString.RoleName.QUALITY_MEMBER)[1].ToString() + "可以添加程序文件。");
                ms.Show();
                return;
            }
            if (txtName.Text.ToString().Contains("#") || txtName.Text.ToString().Contains("'"))
            {
                IMessage ms = new WebFormMessage(Page, "含有特殊字符，请替换后再上传");
                ms.Show();
                return;
            }       
            B_ProgramFileInfo enProFile = new B_ProgramFileInfo();
            enProFile.Name = txtName.Text.ToString().Trim();
            //bProFile.Edition = "1";//txtEdition.Text.ToString().Trim();
            enProFile.Code = txtCode.Text.ToString().Trim();
            enProFile.Sort = ddlSort.SelectedItem.Text.ToString().Trim();
            enProFile.ProTypId = int.Parse(ddlProgramType.SelectedItem.Value.ToString().Trim());
            enProFile.ProTypSubId = int.Parse(ddlProgramSubType.SelectedItem.Value.ToString().Trim());

            enProFile.ActivationDate = DateTime.Now;

            if (enProFile.Save())
            {
                BindProgramFileList(false, 0, 0, "", "", "", "", "", "");
                ClearData();
                IMessage ms = new WebFormMessage(Page, ConstString.PromptInfo.ACTION_ADD_SUC);
                ms.Show();
            }
            else
            {
                IMessage ms = new WebFormMessage(Page, enProFile.ErrMsgs[0].ToString());
                ms.Show();
            }
        }

        /// <summary>
        /// 清空数据
        /// </summary>
        private void ClearData()
        {
            btnDel.Enabled = false;
            txtCode.Text = string.Empty;
            txtName.Text = string.Empty;
            txtCodes.Text = string.Empty;
            txtEditions.Text = string.Empty;
            txtNames.Text = string.Empty;
            this.gvProgramFileList.SelectedIndex = -1;
            ddlSort.SelectedIndex = 0;
            ClearType(ddlProgramType);
            ClearSubType(ddlProgramSubType);
        }

        /// <summary>
        /// 删除操作
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnDel_Click(object sender, EventArgs e)
        {
            int index = gvProgramFileList.SelectedIndex;
            int strID = int.Parse(gvProgramFileList.DataKeys[index]["ID"].ToString());
            if (B_ProgramFileInfo.AllowDelProFile(strID))
            {
                IMessage ms = new WebFormMessage(Page, "该程序文件已在流程中，禁止删除。");
                ms.Show();
                return;
            }
            B_ProgramFileInfo enProFile = new B_ProgramFileInfo();

            enProFile.ID = strID;

            if (enProFile.Delete())
            {
                BindProgramFileList(false, 0, 0, "", "", "", "", "", "");
                ClearData();

                IMessage ms = new WebFormMessage(Page, ConstString.PromptInfo.ACTION_DEL_SUC);
                ms.Show();
            }
            else
            {
                IMessage ms = new WebFormMessage(Page, enProFile.ErrMsgs[0].ToString());
                ms.Show();

            }
        }

        /// <summary>
        /// 分类选择事件（用于查询）
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlSorts_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlSorts.SelectedIndex == 0)
            {
                //清空一级子类
                ClearType(ddlProgramTypes);
            }
            else
            {
                BindProgramType(ddlProgramTypes, ddlSorts.SelectedItem.Text.ToString().Trim());
            }

            //清空二级子类
            ClearSubType(ddlProgramSubTypes);
        }

        /// <summary>
        /// 程序类型选择事件（用于查询）
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlProgramTypes_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlProgramTypes.SelectedIndex == 0)
            {
                ClearSubType(ddlProgramSubTypes);
            }
            else
            {
                int proTypeId = int.Parse(ddlProgramTypes.SelectedItem.Value.ToString().Trim());
                BindProgramSubType(ddlProgramSubTypes, proTypeId);
            }
        }

        /// <summary>
        /// 查询按钮事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            GetSearchList(true);
        }

        protected void gvProgramFileList_ExteriorPaging(GridViewPageEventArgs e)
        {
            ClearData();
            GetSearchList(false);
        }

        private void GetSearchList(bool isSearch)
        {
            int proTypeID = int.Parse(ddlProgramTypes.SelectedItem.Value.ToString().Trim());
            int proSubTypeID = int.Parse(ddlProgramSubTypes.SelectedItem.Value.ToString().Trim());
            string sort = string.Empty;
            string archiveState = string.Empty;
            string applyStyle = string.Empty;
            if (ddlSorts.SelectedIndex != 0 && ddlSorts.SelectedIndex != -1)
            {
                sort = ddlSorts.SelectedItem.Text.ToString().Trim();
            }

            if (ddlArchiveStatus.SelectedIndex != 0 && ddlArchiveStatus.SelectedIndex != -1)
            {
                archiveState = ddlArchiveStatus.SelectedItem.Text.ToString().Trim();
            }

            if (ddlApplyStyle.SelectedIndex != 0 && ddlApplyStyle.SelectedIndex != -1)
            {
                applyStyle = ddlApplyStyle.SelectedItem.Text.ToString().Trim();
            }

            string fileName =FormsMethod.Filter(this.txtNames.Text.ToString().Trim());
            string code = FormsMethod.Filter(txtCodes.Text.ToString().Trim());
            string edition = FormsMethod.Filter(txtEditions.Text.ToString().Trim());

            BindProgramFileList(isSearch, proTypeID, proSubTypeID, sort, archiveState, fileName, code, edition, applyStyle);
        }

        protected void ddlProgramSubType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlProgramSubType.SelectedIndex != 0)
            {
                B_ProgramTypeSub subType = new B_ProgramTypeSub();
                subType.ID = int.Parse(ddlProgramSubType.SelectedValue.ToString());
                txtCode.Text = subType.CodeFrame;
            }
            else
            {
                txtCode.Text = string.Empty;
            }
        }

        /// <summary>
        /// 重新发起
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvProgramFileList_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            string id = gvProgramFileList.DataKeys[e.RowIndex].Value.ToString();
            B_ProgramFileInfo pfEntity = new B_ProgramFileInfo();
            pfEntity.ID = int.Parse(id);
            if (pfEntity.ApplyStyle == ConstString.ProgramFile.PROGRAM_CREATE)
            {
                if (pfEntity.Files.Length > 0)
                { return; }//存在附件
                pfEntity.ApplyStyle = null;
                pfEntity.ArchiveState = null;
                pfEntity.Year = null;
                pfEntity.Edition = null;

                if (pfEntity.Save())
                {
                    GetSearchList(true);
                    IMessage ms = new WebFormMessage(Page, ConstString.PromptInfo.ACTION_RESTART_SUC);
                    ms.Show();
                }
                else
                {
                    string strErr = string.Empty;
                    if (pfEntity.ErrMsgs.Count != 0)
                    {
                        foreach (string str in pfEntity.ErrMsgs)
                        {
                            strErr += str;
                        }
                    }
                    IMessage ms = new WebFormMessage(Page, strErr.Length > 300 ? strErr.Substring(0, 300) : strErr);
                    ms.Show();
                }
            }//创建程序
            else
            {
                if (pfEntity.Delete())
                {
                    GetSearchList(true);
                    IMessage ms = new WebFormMessage(Page, ConstString.PromptInfo.ACTION_RESTART_SUC);
                    ms.Show();
                }
                else
                {
                    string strErr = string.Empty;
                    if (pfEntity.ErrMsgs.Count != 0)
                    {
                        foreach (string str in pfEntity.ErrMsgs)
                        {
                            strErr += str;
                        }
                    }
                    IMessage ms = new WebFormMessage(Page, strErr.Length > 300 ? strErr.Substring(0, 300) : strErr);
                    ms.Show();
                }
            }//升版、注销程序
        }

        /// <summary>
        /// 撤销流程
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvProgramFileList_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            string strID = gvProgramFileList.DataKeys[e.RowIndex].Value.ToString();
            bool bFlag = false;
            foreach (string strRole in CurrentUserInfo.RoleName)
            {
                if (strRole == "OA系统管理员" || strRole == "OU系统管理员" || strRole.Contains("管理员") || strRole == ConstString.RoleName.QUALITY_MEMBER)
                {
                    bFlag = true;
                }
            }
            if (!bFlag)
            {
                IMessage ms = new WebFormMessage(Page, "只有\'OA系统管理员\'、\'OU系统管理员\'、\'管理员\'、\'质保人员\'可以撤销流程。");
                ms.Show();
                return;
            }
            try
            {
                B_ProgramFileInfo pfEntity = new B_ProgramFileInfo();
                pfEntity.ID = int.Parse(strID);
                string strProcessID = pfEntity.ProcessID;
                //if (pfEntity.ApplyStyle == ConstString.ProgramFile.PROGRAM_CREATE)
                //{
                //    pfEntity.ProcessID = null;
                //    pfEntity.Year = null;
                //    //pfEntity.ApplyStyle = null;
                //    //pfEntity.ArchiveState = null;
                //    //pfEntity.Edition = null;

                //    if (pfEntity.Save() == false)
                //    {
                //        IMessage ms = new WebFormMessage(Page, pfEntity.ErrMsgs[0].ToString());
                //        ms.Show();
                //        return;
                //    }

                //}//创建
                //else
                //{
                //    if (pfEntity.Delete() == false)
                //    {
                //        IMessage ms = new WebFormMessage(Page, pfEntity.ErrMsgs[0].ToString());
                //        ms.Show();
                //        return;
                //    }

                //}//升版、注销

                if (pfEntity.Delete() == false)
                {
                    IMessage ms = new WebFormMessage(Page, pfEntity.ErrMsgs[0].ToString());
                    ms.Show();
                    return;
                }
                GetSearchList(true);
                //取消流程
                AgilePointWF ag = new AgilePointWF();
                WorkflowService api = ag.GetAPI();
                api.CancelProcInst(strProcessID);
                IMessage msg = new WebFormMessage(Page, "撤销成功。");
                msg.Show();
            }
            catch (Exception ex)
            {
                IMessage ms = new WebFormMessage(Page, ex.Message.Length > 300 ? ex.Message.Substring(0, 300) : ex.Message);
                ms.Show();
            }
        }
    }
}