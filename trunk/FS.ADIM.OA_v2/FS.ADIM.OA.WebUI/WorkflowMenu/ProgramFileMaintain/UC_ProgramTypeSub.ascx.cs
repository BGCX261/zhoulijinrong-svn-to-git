/*****************************************************************/
// Copyright (C) 2010 方正国际软件有限公司
//
// 文件功能描述：程序文件子类维护
//
// 创 建 者：黄 琦
// 创建标识: C_2010.01.12
//
// 修改标识：renjinquan 2010-5-11
// 修改描述：修改lnkbtnSearch_Click函数，查询时过滤危险字符
/*****************************************************************/
using System;
using System.Data;
using System.Web.UI.WebControls;
 
using FounderSoftware.Framework.UI.WebCtrls;
using FounderSoftware.Framework.UI.WebPageFrame;
using FS.ADIM.OA.BLL.Busi.InfoMaintain;
using FS.ADIM.OA.BLL.Common;

namespace FS.ADIM.OA.WebUI.WorkflowMenu.ProgramFileMaintain
{
    public partial class UC_ProgramTypeSub : UCBase
    {
        #region 定义GridView单元格索引
        private const int NAME = 1;//程序类型
        private const int SUBNAME = 2;//程序子类
        private const int DESCRIPTION = 3;//程序类型描述
        private const int CODEFRAME = 4;//编码结构
        private const int SORT = 5;//分类
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindProgramType();
                this.gvProgramSubTypeList.ShowPagerRow = true;
                this.gvProgramSubTypeList.PageType = PageType.ExteriorPage;
                BindProgramSubTypeList(false, -1, "", "", "");
            }
        }

        /// <summary>
        /// 绑定DropDownList(程序类型)
        /// </summary>
        private void BindProgramType()
        {
            B_ProgramType bllProType = new B_ProgramType();
            DataTable dt = bllProType.GetAllProgramType();
            if (dt != null)
            {
                this.ddlProgramType.DataSource = dt;
                this.ddlProgramType.DataValueField = "ID";
                this.ddlProgramType.DataTextField = "Name";
                this.ddlProgramType.DataBind();
                ListItem li = new ListItem();
                li.Text = "--请选择--";
                li.Value = "0";
                this.ddlProgramType.Items.Insert(0, li);
            }
        }

        /// <summary>
        /// 绑定程序子类列表
        /// </summary>
        /// <param name="proTypeID">程序类型ID</param>
        /// <param name="name">程序子类名称</param>
        /// <param name="codeFrame">编码结构</param>
        /// <param name="description">子类描述</param>
        private void BindProgramSubTypeList(bool isSearch, int proTypeID, string name, string codeFrame, string description)
        {
            B_ProgramTypeSub bllProTypeSub = new B_ProgramTypeSub();

            int iStart = 0;
            int iEnd = 0;
            int iCount = 0;

            if (isSearch)
            {
                this.gvProgramSubTypeList.PageIndex = 0;
                iCount = bllProTypeSub.GetProgamSubTypeCount(proTypeID, name, codeFrame, description, 1, gvProgramSubTypeList.PageSize, ref iStart, ref iEnd);
            }
            else
            {
                iCount = bllProTypeSub.GetProgamSubTypeCount(proTypeID, name, codeFrame, description, gvProgramSubTypeList.PageIndex + 1, gvProgramSubTypeList.PageSize, ref iStart, ref iEnd);
            }
            DataTable dt = bllProTypeSub.GetProgamSubType(proTypeID, name, codeFrame, description, iStart, iEnd);
            if (dt != null)
            {
                this.gvProgramSubTypeList.RecordCount = iCount;
                this.gvProgramSubTypeList.DataSource = dt;
                this.gvProgramSubTypeList.DataBind();
            }
        }

        /// <summary>
        /// 添加按钮事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkbtnAdd_Click(object sender, EventArgs e)
        {
            if (ddlProgramType.SelectedIndex == -1)
            {
                IMessage ms = new WebFormMessage(Page, ConstString.PromptInfo.ACTION_CHECK_PROTYPESUB_IS_EMPTY);
                ms.Show();
                return;
            }
            B_ProgramTypeSub enProTypeSub = new B_ProgramTypeSub();

            enProTypeSub.Name = txtProgramSubType.Text.ToString().Trim();
            enProTypeSub.ProTypId = int.Parse(this.ddlProgramType.SelectedValue);
            enProTypeSub.Description = txtProgramSubTypeDesc.Text.ToString().Trim();
            enProTypeSub.CodeFrame = txtCodeFrame.Text.ToString().Trim();
            
            if (enProTypeSub.Save())
            {
                BindProgramSubTypeList(true, -1, "", "", "");
                ClearData();
                IMessage ms = new WebFormMessage(Page, ConstString.PromptInfo.ACTION_ADD_SUC);
                ms.Show();
            }
            else
            {
                IMessage ms = new WebFormMessage(Page, enProTypeSub.ErrMsgs[0].ToString());
                ms.Show();
            }
        }

        /// <summary>
        /// 修改按钮事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkbtnModify_Click(object sender, EventArgs e)
        {
            if (ddlProgramType.SelectedIndex == -1)
            {
                IMessage ms = new WebFormMessage(Page, ConstString.PromptInfo.ACTION_CHECK_PROTYPESUB_IS_EMPTY);
                ms.Show();
                return;
            }

            int index = gvProgramSubTypeList.SelectedIndex;
            int id = int.Parse(gvProgramSubTypeList.DataKeys[index]["ID"].ToString());

            B_ProgramTypeSub enProTypeSub = new B_ProgramTypeSub();

            enProTypeSub.ID = id;
            enProTypeSub.Name = txtProgramSubType.Text.ToString().Trim();
            enProTypeSub.ProTypId = int.Parse(ddlProgramType.SelectedValue.ToString());
            enProTypeSub.CodeFrame = txtCodeFrame.Text.ToString().Trim();
            enProTypeSub.Description = txtProgramSubTypeDesc.Text.ToString().Trim();
            
            if (enProTypeSub.Save())
            {
                BindProgramSubTypeList(false, -1, "", "", "");
                ClearData();

                IMessage ms = new WebFormMessage(Page, ConstString.PromptInfo.ACTION_EDIT_SUC);
                ms.Show();
            }
            else
            {
                IMessage ms = new WebFormMessage(Page, enProTypeSub.ErrMsgs[0].ToString());
                ms.Show();
            }
        }

        /// <summary>
        /// 删除按钮事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkbtnDelete_Click(object sender, EventArgs e)
        {
            int index = gvProgramSubTypeList.SelectedIndex;
            int id = int.Parse(gvProgramSubTypeList.DataKeys[index]["ID"].ToString());

            B_ProgramTypeSub enProTypeSub = new B_ProgramTypeSub();

            enProTypeSub.ID = id;

            if (enProTypeSub.Delete())
            {
                BindProgramSubTypeList(true, -1, "", "", "");
                ClearData();

                IMessage ms = new WebFormMessage(Page, ConstString.PromptInfo.ACTION_DEL_SUC);
                ms.Show();

            }
            else
            {
                IMessage ms = new WebFormMessage(Page, enProTypeSub.ErrMsgs[0].ToString());
                ms.Show();
            }
        }

        /// <summary>
        /// 选择行事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvProgramSubTypeList_SelectedIndexChanged(object sender, EventArgs e)
        {
            SelectRowBindDetail();
        }

        /// <summary>
        /// 根据选中行显示具体信息
        /// </summary>
        private void SelectRowBindDetail()
        {
            this.lnkbtnModify.Enabled = true;
            this.lnkbtnDelete.Enabled = true;
            this.hfSelectedIndex.Value = "selected";

            GridViewRow row = gvProgramSubTypeList.SelectedRow;

            txtProgramSubType.Text = row.Cells[SUBNAME].ToolTip.ToString();
            txtCodeFrame.Text = row.Cells[CODEFRAME].ToolTip.ToString();
            txtProgramSubTypeDesc.Text = row.Cells[DESCRIPTION].ToolTip.ToString();
 
            ListItem li = ddlProgramType.Items.FindByText(row.Cells[NAME].ToolTip.ToString());
            ddlProgramType.ClearSelection();

            if (li != null)
            {
                li.Selected = true;
            }
            else
            {
                ddlProgramType.SelectedIndex = 0;
            }
        }

        /// <summary>
        /// 清空数据
        /// </summary>
        private void ClearData()
        {
            this.hfSelectedIndex.Value = string.Empty;
            this.lnkbtnDelete.Enabled = false;
            this.lnkbtnModify.Enabled = false;
            this.txtProgramSubTypeDesc.Text = string.Empty;
            this.txtProgramSubType.Text = string.Empty;
            this.txtCodeFrame.Text = string.Empty;
            this.ddlProgramType.SelectedIndex = 0;
            this.gvProgramSubTypeList.SelectedIndex = -1;

        }

        /// <summary>
        /// 查询按钮事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkbtnSearch_Click(object sender, EventArgs e)//过滤危险字符
        {
            BindProgramSubTypeList(true, int.Parse(ddlProgramType.SelectedValue.ToString()),
               FormsMethod.Filter(txtProgramSubType.Text.ToString()),FormsMethod.Filter(txtCodeFrame.Text.ToString()),FormsMethod.Filter(txtProgramSubTypeDesc.Text.ToString()));
        }

        /// <summary>
        /// 绑定行事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvProgramSubTypeList_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DataRowView row = e.Row.DataItem as DataRowView;
                e.Row.Cells[1].ToolTip = row["TypeName"].ToString().Trim();
                e.Row.Cells[2].ToolTip = row["SubTypeName"].ToString().Trim();
                e.Row.Cells[3].ToolTip = row["Description"].ToString().Trim();
                e.Row.Cells[4].ToolTip = row["CodeFrame"].ToString().Trim();

                e.Row.Cells[1].Text = row["TypeName"].ToString().Length > 10 ? row["TypeName"].ToString().Substring(0, 10) + "..." : row["TypeName"].ToString().Trim();
                e.Row.Cells[2].Text = row["SubTypeName"].ToString().Length > 10 ? row["SubTypeName"].ToString().Substring(0, 10) + "..." : row["SubTypeName"].ToString().Trim();
                e.Row.Cells[3].Text = row["Description"].ToString().Length > 20 ? row["Description"].ToString().Substring(0, 20) + "..." : row["Description"].ToString().Trim();
                e.Row.Cells[4].Text = row["CodeFrame"].ToString().Length > 20 ? row["CodeFrame"].ToString().Substring(0, 20) + "..." : row["CodeFrame"].ToString().Trim();
            }
        }

        /// <summary>
        /// 翻页事件
        /// </summary>
        /// <param name="e"></param>
        protected void gvProgramSubTypeList_ExteriorPaging(GridViewPageEventArgs e)
        {
            ClearData();
            BindProgramSubTypeList(false, int.Parse(ddlProgramType.SelectedValue.ToString()),
               txtProgramSubType.Text.ToString(), txtCodeFrame.Text.ToString(), txtProgramSubTypeDesc.Text.ToString());
        }

    }
}