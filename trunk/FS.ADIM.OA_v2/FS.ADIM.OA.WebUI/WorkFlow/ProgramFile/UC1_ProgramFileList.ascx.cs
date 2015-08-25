/*****************************************************************/
// Copyright (C) 2010 方正国际软件有限公司
//
// 文件功能描述：程序文件流程发起
//
// 创 建 者：黄 琦
// 创建标识: C_2010.01.13
//
// 修改标识：renjinquan 2010-5-10
// 修改描述：修改SelectRowBindDetail、SelectRowBindDetails，去除使用GetText函数转换字符串

// 修改标识：renjinquan 2010-5-11
// 修改描述：修改RedirectUrl，对url中'转换。
/*****************************************************************/
using System;
using System.Data;
using System.Web.UI.WebControls;

using FounderSoftware.Framework.UI.WebPageFrame;
using FounderSoftware.Framework.UI.WebCtrls;
using FS.ADIM.OU.OutBLL;
using FS.ADIM.OA.BLL;
using FS.ADIM.OA.BLL.Busi.InfoMaintain;
using FS.ADIM.OA.BLL.Common;
using FS.ADIM.OA.BLL.Busi;
using FS.ADIM.OA.BLL.SystemM;
namespace FS.ADIM.OA.WebUI.WorkFlow.ProgramFile
{
    public partial class UC1_ProgramFileList : System.Web.UI.UserControl
    {
        /// <summary>
        /// PageLoad事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadClickCreateTabStyle();
                this.MultiView.ActiveViewIndex = 0;
                gvProgramFileList.PageType = PageType.InteriorPage;
                BindProgramFileListForCreate(0, 0, "");

                //绑定本处室
                OADept.GetDeptByUser(ddlDept, CurrentUserInfo.LoginName, 1, false);
                OADept.GetDeptByUser(ddlDept2, CurrentUserInfo.LoginName, 1, false);
            }
        }

        /// <summary>
        /// 升版、撤销程序切换
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void rdolstStyle_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.gvProgramFilesList.SelectedIndex != -1)
            {
                GridViewRow row = this.gvProgramFilesList.SelectedRow;
                if (rdolstStyle.SelectedIndex != -1 && rdolstStyle.SelectedIndex == 0)
                {
                    //版次
                    int edition = int.Parse(row.Cells[EDITION].Text.ToString()) + 1;
                    this.lblEditions.Text = edition.ToString();
                }
                else
                {
                    this.lblEditions.Text = row.Cells[EDITION].Text.ToString();
                }
            }
        }

        #region 创建程序TAB
        /// <summary>
        /// 加载创建程序TAB样式
        /// </summary>
        private void LoadClickCreateTabStyle()
        {
            tab_create.Style.Clear();
            tab_create.Style.Add("height", "10px");
            tab_create.Style.Add("border", "solid 1px #8E9699");
            tab_create.Style.Add("border-bottom-color", "#fff");
            tab_create.Style.Add("z-index", "2");
            tab_create.Style.Add("padding", "10px 30px");
            tab_create.Style.Add("position", "relative");

            tab_update.Style.Clear();
            tab_update.Style.Add("background-color", "#ECE9D8");
            tab_update.Style.Add("border", "solid 1px #ECE9D8");
            tab_update.Style.Add("height", "10px");
            tab_update.Style.Add("padding", "10px 30px");
        }

        /// <summary>
        /// 加载升版程序TAB样式
        /// </summary>
        private void LoadClickUpdateTabStyle()
        {
            tab_update.Style.Clear();
            tab_update.Style.Add("height", "10px");
            tab_update.Style.Add("border", "solid 1px #8E9699");
            tab_update.Style.Add("border-bottom-color", "#fff");
            tab_update.Style.Add("z-index", "2");
            tab_update.Style.Add("padding", "10px 30px");
            tab_update.Style.Add("position", "relative");

            tab_create.Style.Clear();
            tab_create.Style.Add("background-color", "#ECE9D8");
            tab_create.Style.Add("border", "solid 1px #ECE9D8");
            tab_create.Style.Add("height", "10px");
            tab_create.Style.Add("padding", "10px 30px");
        }

        private void BindProgramFileListForCreate(int proTypeID, int proSubTypeID, string sort)
        {
            B_ProgramTypeSub bProTypeSub = new B_ProgramTypeSub();
            B_ProgramFileInfo bPf = new B_ProgramFileInfo();

            DataTable dt = bPf.GetProgamFileForCreateProgram(proTypeID, proSubTypeID, sort);
            if (dt != null)
            {
                if (dt.Rows.Count == 0)
                { gvProgramFileList.Visible = false; }
                else
                {
                    gvProgramFileList.DataSource = dt;
                    gvProgramFileList.DataBind();
                }
            }
        }

        /// <summary>
        /// 创建程序TAB
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkbtnCreateProgram_Click(object sender, EventArgs e)
        {
            LoadClickCreateTabStyle();
            BindProgramFileListForCreate(0, 0, "");
            this.MultiView.ActiveViewIndex = 0;
        }

        //protected void gvProgramFileList_PageIndexChanged(object sender, EventArgs e)
        //{
        //    ClearData();
        //}

        #region 创建程序

        //程序编码
        private const int CODE = 1;
        //程序名称
        private const int NAME = 2;
        //程序分类
        private const int SORT = 3;
        //程序类型
        private const int TYPE = 4;
        //程序子类
        private const int SUBTYPE = 5;
        //程序类型ID
        private const int TYPE_ID = 6;
        //程序子类ID
        private const int SUBTYPE_ID = 7;
        //程序文件ID
        private const int PF_ID = 8;

        #endregion

        /// <summary>
        /// GIREVIEW绑定行事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvProgramFileList_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow || e.Row.RowType == DataControlRowType.Header || e.Row.RowType == DataControlRowType.Footer)
            {
                e.Row.Cells[0].Attributes.Add("style", "border:solid 1px #b8b4a3; border-collapse:collapse");
                e.Row.Cells[TYPE_ID].Visible = false;
                e.Row.Cells[SUBTYPE_ID].Visible = false;
                e.Row.Cells[PF_ID].Visible = false;
            }
        }

        private void SelectRowBindDetail()
        {
            this.btnConfirm.Enabled = true;

            GridViewRow row = this.gvProgramFileList.SelectedRow;

            //程序名称
            this.txtName.Text = row.Cells[NAME].Text.ToString();

            //程序编码
            this.txtCode.Text = row.Cells[CODE].Text.ToString();

            //程序类型 
            this.txtType.Text = row.Cells[TYPE].Text.ToString();

            //程序子类
            this.txtSubType.Text = row.Cells[SUBTYPE].Text.ToString();

            this.lblEdition.Visible = true;

            //程序分类
            this.hfSort.Value = row.Cells[SORT].Text.ToString().Trim();
        }

        protected void gvProgramFileList_SelectedIndexChanged(object sender, EventArgs e)
        {
            SelectRowBindDetail();
        }

        /// <summary>
        /// 清空数据
        /// </summary>
        private void ClearData()
        {
            txtCode.Text = string.Empty;
            txtName.Text = string.Empty;
            txtSubType.Text = string.Empty;
            txtType.Text = string.Empty;
            lblEdition.Visible = false;
            btnConfirm.Enabled = false;
            gvProgramFileList.SelectedIndex = -1;
            hfSort.Value = string.Empty;
        }

        /// <summary>
        /// 绑定程序类型Dropdownlist
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
        /// 绑定程序子类Dropdownlist
        /// </summary>
        /// <param name="ddl"></param>
        /// <param name="proTypeID"></param>
        private void BindProgramSubType(DropDownList ddl, int proTypeID)
        {
            ddl.Items.Clear();
            B_ProgramTypeSub bllProTypeSub = new B_ProgramTypeSub();
            DataTable dt = bllProTypeSub.GetProgamSubTypeByTypeID(proTypeID);
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
        /// 清空类型
        /// </summary>
        private void ClearType(DropDownList ddl)
        {
            ddl.Items.Clear();
            ListItem li = new ListItem();
            li.Text = "请选择分类";
            li.Value = "0";
            ddl.Items.Insert(0, li);
        }

        /// <summary>
        /// 清空子类
        /// </summary>
        private void ClearSubType(DropDownList ddl)
        {
            ddl.Items.Clear();
            ListItem li = new ListItem();
            li.Text = "请选择类型";
            li.Value = "0";
            ddl.Items.Insert(0, li);
        }

        /// <summary>
        /// 程序分类选择
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlSort_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlSort.SelectedIndex == 0)
            {
                //清空类型
                ClearType(ddlProgramType);
                //清空子类
                //ClearSubType(ddlProgramSubType);
            }
            else
            {
                BindProgramType(ddlProgramType, ddlSort.SelectedItem.Text.ToString().Trim());
            }

            //清空子类
            ClearSubType(ddlProgramSubType);
        }

        /// <summary>
        /// 程序类型选择
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

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            string sort = string.Empty;
            if (ddlSort.SelectedIndex != 0 && ddlSort.SelectedIndex != -1)
            {
                sort = ddlSort.SelectedItem.Text.ToString().Trim();
            }

            BindProgramFileListForCreate(int.Parse(ddlProgramType.SelectedItem.Value.ToString()),
                int.Parse(ddlProgramSubType.SelectedItem.Value.ToString()), sort);
        }

        /// <summary>
        /// 提交（创建程序）
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnConfirm_Click(object sender, EventArgs e)
        {
            if (ddlDept.SelectedIndex == -1)
            {
                IMessage ms = new WebFormMessage(Page, ConstString.PromptInfo.ACTION_CHECK_HOST_NONE);
                ms.Show();
                return;
            }
            if (string.IsNullOrEmpty(ddlDept.SelectedValue) && ddlDept.SelectedIndex == 0)
            {
                IMessage ms = new WebFormMessage(Page, ConstString.PromptInfo.ACTION_CHECK_HOST_UNSELECTED);
                ms.Show();
                return;
            }
            GridViewRow row = this.gvProgramFileList.SelectedRow;

            B_ProgramFileInfo bProFile = new B_ProgramFileInfo();
            bProFile.ID = int.Parse(row.Cells[PF_ID].Text.ToString());//ID
            bProFile.Name = txtName.Text.ToString().Trim();
            bProFile.Edition = lblEdition.Text.ToString().Trim();
            bProFile.Code = txtCode.Text.ToString().Trim();
            bProFile.Sort = row.Cells[SORT].Text.ToString().Trim();
            bProFile.ProTypId = int.Parse(row.Cells[TYPE_ID].Text.ToString().Trim());
            bProFile.ProTypSubId = int.Parse(row.Cells[SUBTYPE_ID].Text.ToString().Trim());
            bProFile.ArchiveState = ConstString.ProgramFile.PROGRAM_UNFINISHED;//据ArchiveState is null判断需要发起的程序文件
            B_DocumentNo_A docNo_A = new B_DocumentNo_A();
            bProFile.SerialID = docNo_A.GetNo(ProcessConstString.TemplateName.PROGRAM_FILE);//3位流水号
            bProFile.Year = DateTime.Now.Year.ToString();
            bProFile.ApplyStyle = ConstString.ProgramFile.PROGRAM_CREATE;

            if (bProFile.Save())
            {
                RedirectUrl(bProFile.ID.ToString(), bProFile.Name, bProFile.Code, bProFile.Edition, bProFile.ApplyStyle,
                    this.hfSort.Value, ddlDept.SelectedValue, bProFile.SerialID);
            }
            else
            {
                IMessage im = new WebFormMessage(Page, "提交失败。");
                im.Show();
            }

        }
        #endregion

        #region 升版、注销程序
        /// <summary>
        /// 升版、注销程序TAB
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkbtnUpdateProgram_Click(object sender, EventArgs e)
        {
            LoadClickUpdateTabStyle();
            BindProgramTypes();
            BindProgramFileListForUpdate(false, 0, "", "", "", "");
            this.MultiView.ActiveViewIndex = 1;
        }

        /// <summary>
        /// 绑定程序类型
        /// </summary>
        private void BindProgramTypes()
        {
            B_ProgramType bProType = new B_ProgramType();
            DataTable dt = bProType.GetAllProgramType();
            if (dt != null)
            {
                this.ddlProgramTypes.DataSource = dt;
                ddlProgramTypes.DataValueField = "ID";
                ddlProgramTypes.DataTextField = "Name";
                ddlProgramTypes.DataBind();
                ListItem li = new ListItem();
                li.Text = "--请选择--";
                li.Value = "0";
                ddlProgramTypes.Items.Insert(0, li);
            }
        }

        /// <summary>
        /// 绑定升版、注销程序Gridview
        /// </summary>
        /// <param name="proTypeID">类型ID</param>
        /// <param name="fileName">程序名称</param>
        /// <param name="code">程序代码</param>
        /// <param name="writerName">编写人</param>
        /// <param name="authorized">批准人</param>
        private void BindProgramFileListForUpdate(int proTypeID, string fileName, string code, string writerName, string authorized)
        {
            B_ProgramFileInfo bllProFile = new B_ProgramFileInfo();
            DataTable dt = bllProFile.GetProgamFileForUpdateProgram(proTypeID, fileName, code, writerName, authorized);
            if (dt != null)
            {
                gvProgramFilesList.PageType = PageType.InteriorPage;

                gvProgramFilesList.DataSource = dt;
                gvProgramFilesList.DataBind();
            }
        }

        /// <summary>
        /// 绑定升版、注销程序Gridview(外分页)
        /// </summary>
        /// <param name="isSearch"></param>
        /// <param name="proTypeID"></param>
        /// <param name="fileName"></param>
        /// <param name="code"></param>
        /// <param name="writerName"></param>
        /// <param name="authorized"></param>
        private void BindProgramFileListForUpdate(bool isSearch, int proTypeID, string fileName, string code, string writerName, string authorized)
        {
            B_ProgramFileInfo bllProFile = new B_ProgramFileInfo();

            int iStart = 0;
            int iEnd = 0;
            int iCount = 0;
            if (isSearch)
            {
                iCount = bllProFile.GetFileCountExteriorPageForUpdate(proTypeID, fileName, code, writerName, authorized, 1, gvProgramFilesList.PageSize, ref iStart, ref iEnd);
            }
            else
            {
                iCount = bllProFile.GetFileCountExteriorPageForUpdate(proTypeID, fileName, code, writerName, authorized, gvProgramFilesList.PageIndex + 1, gvProgramFilesList.PageSize, ref iStart, ref iEnd);
            }
            DataTable dtList = bllProFile.GetFileExteriorPageForUpdate(proTypeID, fileName, code, writerName, authorized, iStart, iEnd);
            this.gvProgramFilesList.PageType = PageType.ExteriorPage;
            this.gvProgramFilesList.RecordCount = iCount;
            gvProgramFilesList.ShowPagerRow = true;

            //绑定数据
            if (iCount == 0)
            {
                this.gvProgramFilesList.Visible = false;
            }
            else
            {
                this.gvProgramFilesList.DataSource = dtList;
                this.gvProgramFilesList.DataBind();
            }
        }

        /// <summary>
        /// 清空数据
        /// </summary>
        private void ClearDatas()
        {
            txtCodes.Text = string.Empty;
            lblEditions.Text = string.Empty;
            txtNames.Text = string.Empty;
            txtTypes.Text = string.Empty;
            txtSubTypes.Text = string.Empty;
            gvProgramFilesList.SelectedIndex = -1;
            hfSort.Value = string.Empty;
        }

        //程序代码
        private const int CODES = 1;
        //程序名称
        private const int FILENAME = 2;
        //程序类型
        private const int TYPENAME = 3;
        //程序子类
        private const int SUBTYPENAME = 4;
        //版次
        private const int EDITION = 7;
        //分类
        private const int SORTS = 9;
        //类别ID
        private const int TYPES_ID = 10;
        //子类别ID
        private const int SUBTYPES_ID = 11;

        /// <summary>
        /// 选择行绑定显示详细信息
        /// </summary>
        private void SelectRowBindDetails()
        {
            this.btnConfirms.Enabled = true;

            GridViewRow row = this.gvProgramFilesList.SelectedRow;

            //程序代码
            this.txtCodes.Text = row.Cells[CODES].Text.ToString();

            //程序名称 
            this.txtNames.Text =row.Cells[FILENAME].Text.ToString();

            //程序类型
            this.txtTypes.Text = row.Cells[TYPENAME].Text.ToString();

            //程序子类
            this.txtSubTypes.Text =row.Cells[SUBTYPENAME].Text.ToString();

            if (rdolstStyle.SelectedIndex != -1 && rdolstStyle.SelectedIndex == 0)
            {
                //版次
                int edition = int.Parse(row.Cells[EDITION].Text.ToString()) + 1;
                this.lblEditions.Text = edition.ToString();
            }
            else
            {
                this.lblEditions.Text = row.Cells[EDITION].Text.ToString();
            }

            this.hfSorts.Value = row.Cells[SORTS].Text.ToString().Trim();

        }

        /// <summary>
        /// 升版、注销程序信息列表
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvProgramFilesList_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow || e.Row.RowType == DataControlRowType.Header || e.Row.RowType == DataControlRowType.Footer)
            {
                e.Row.Cells[0].Attributes.Add("style", "border:solid 1px #b8b4a3; border-collapse:collapse");
                e.Row.Cells[SORTS].Visible = false;
                e.Row.Cells[TYPES_ID].Visible = false;
                e.Row.Cells[SUBTYPES_ID].Visible = false;
            }
        }

        /// <summary>
        /// 升版程序Gridview选择行事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvProgramFilesList_SelectedIndexChanged(object sender, EventArgs e)
        {
            SelectRowBindDetails();
        }

        /// <summary>
        /// 升版程序Gridview外部分页
        /// </summary>
        /// <param name="e"></param>
        protected void gvProgramFilesList_ExteriorPaging(GridViewPageEventArgs e)
        {
            ClearDatas();
            BindProgramFileListForUpdate(false, int.Parse(ddlProgramTypes.SelectedItem.Value.ToString()),
               txtNamess.Text.ToString(), txtCodess.Text, txtWriter.Text.ToString(), txtAuthorized.Text.ToString());
        }

        /// <summary>
        /// 升版程序查询按钮事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSearchs_Click(object sender, EventArgs e)
        {
            this.gvProgramFilesList.PageIndex = 0;
            BindProgramFileListForUpdate(true, int.Parse(ddlProgramTypes.SelectedItem.Value.ToString()),
                txtNamess.Text.ToString(), txtCodess.Text, txtWriter.Text.ToString(), txtAuthorized.Text.ToString());
        }

        /// <summary>
        /// 升版程序提交按钮事件（升版、注销）
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnConfirms_Click(object sender, EventArgs e)
        {
            if (ddlDept2.SelectedIndex == -1)
            {
                IMessage ms = new WebFormMessage(Page, ConstString.PromptInfo.ACTION_CHECK_HOST_NONE);
                ms.Show();
                return;
            }
            if (string.IsNullOrEmpty(ddlDept2.SelectedValue) && ddlDept2.SelectedIndex == 0)
            {
                IMessage ms = new WebFormMessage(Page, ConstString.PromptInfo.ACTION_CHECK_HOST_UNSELECTED);
                ms.Show();
                return;
            }
            bool isUsed = false;
            bool isRelUsed = false;
            if (rdolstStyle.SelectedIndex == 0)
            {
                isUsed = B_ProgramFileInfo.IsFileUse(txtNames.Text, lblEditions.Text, ConstString.ProgramFile.PROGRAM_UPDATE);
                int iEdition = int.Parse(lblEdition.Text) - 1;
                isRelUsed = B_ProgramFileInfo.IsFileUse(txtNames.Text, iEdition.ToString(), ConstString.ProgramFile.PROGRAM_DELETE);
            }//升版
            else
            {
                isUsed = B_ProgramFileInfo.IsFileUse(txtNames.Text, lblEditions.Text, ConstString.ProgramFile.PROGRAM_DELETE);
                int iEdition = int.Parse(lblEdition.Text) + 1;
                isRelUsed = B_ProgramFileInfo.IsFileUse(txtNames.Text, iEdition.ToString(), ConstString.ProgramFile.PROGRAM_UPDATE);
            }//注销
            if (isUsed)
            {
                IMessage ms = new WebFormMessage(Page, txtNames.Text + "第" + lblEditions.Text + "版，已在流程流转中。");
                ms.Show();
                return;
            }//所选程序文件已占用
            if (isRelUsed)
            {
                string strPrompt = string.Empty;
                strPrompt = rdolstStyle.SelectedIndex == 0 ? "正在注销流转中，不能升版。" : "正在升版流转中，不能注销。";
                IMessage ms = new WebFormMessage(Page, txtNames.Text + strPrompt);
                ms.Show();
                return;
            }

            GridViewRow row = this.gvProgramFilesList.SelectedRow;

            B_ProgramFileInfo bProFile = new B_ProgramFileInfo();
            bProFile.Name = txtNames.Text.ToString().Trim();
            bProFile.Edition = lblEditions.Text.ToString().Trim();
            bProFile.Code = txtCodes.Text.ToString().Trim();
            bProFile.Sort = row.Cells[SORTS].Text.ToString().Trim();
            bProFile.ProTypId = int.Parse(row.Cells[TYPES_ID].Text.ToString().Trim());
            bProFile.ProTypSubId = int.Parse(row.Cells[SUBTYPES_ID].Text.ToString().Trim());
            bProFile.ArchiveState = ConstString.ProgramFile.PROGRAM_UNFINISHED;//未完成
            B_DocumentNo_A docNo_A = new B_DocumentNo_A();
            bProFile.SerialID = docNo_A.GetNo(ProcessConstString.TemplateName.PROGRAM_FILE);//3位流水号
            bProFile.Year = DateTime.Now.Year.ToString();
            bProFile.ApplyStyle = rdolstStyle.SelectedIndex == 0 ? ConstString.ProgramFile.PROGRAM_UPDATE : ConstString.ProgramFile.PROGRAM_DELETE;
            string deptID = ddlDept2.SelectedValue;
            //string serialID = txtSerialID2.Text.Trim();

            //if (rdolstStyle.SelectedIndex == 0)
            //{
            if (bProFile.Save())
            {
                RedirectUrl(bProFile.ID.ToString(), bProFile.Name, bProFile.Code, bProFile.Edition, bProFile.ApplyStyle,
                    this.hfSorts.Value, ddlDept2.SelectedValue, bProFile.SerialID);
            }
            else
            {
                IMessage im = new WebFormMessage(Page, "提交失败。");
                im.Show();
            }
            //}//升版
            //else
            //{
            //    RedirectUrl(bProFile.ID.ToString(), bProFile.Name, bProFile.Code, bProFile.Edition, bProFile.ApplyStyle, deptID);
            //}//注销

        }

        /// <summary>
        /// 重定向
        /// </summary>
        /// <param name="fileID">程序文件ID</param>
        /// <param name="fileName">程序文件名称</param>
        /// <param name="code">程序编码</param>
        /// <param name="edition">版次</param>
        /// <param name="serialID">流水号</param>
        /// <param name="style">类型</param>
        private void RedirectUrl(string fileID, string fileName, string code, string edition, string style, string sort, string deptID, string num)
        {
            string url = string.Format(@"Container.aspx?ClassName=FS.ADIM.OA.WebUI.WorkFlow.ProgramFile.PG2_ProgramFile&name={0}&code={1}&edition={2}&style={3}&sort={4}&id={5}&deptID={6}&num={7}&TemplateName={8}&StepName=编制",
                    Server.UrlEncode(fileName), Server.UrlEncode(code), Server.UrlEncode(edition), Server.UrlEncode(style),
                    Server.UrlEncode(sort), Server.UrlEncode(fileID), Server.UrlEncode(deptID), Server.UrlEncode(num), Request.QueryString[ConstString.QueryString.TEMPLATE_NAME].ToString());

            ClientScriptM.ResponseScript(Page, "location.replace('" +SysString.UrlFilter(url) + "');", false);//renjinquan改，对url中的'转换
        }
        #endregion
    }
}