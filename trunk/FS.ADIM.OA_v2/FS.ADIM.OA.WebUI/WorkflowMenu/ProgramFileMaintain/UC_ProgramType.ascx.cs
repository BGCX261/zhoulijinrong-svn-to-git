/*****************************************************************/
// Copyright (C) 2010 方正国际软件有限公司
//
// 文件功能描述：程序文件类型维护
//
// 创 建 者：黄 琦
// 创建标识: C_2010.01.12
//
// 修改标识：
// 修改描述：
/*****************************************************************/
using System;
using System.Data;
using System.Web.UI.WebControls;

using FS.ADIM.OA.BLL.Busi.InfoMaintain;
using FS.ADIM.OA.BLL.Common;
using FounderSoftware.Framework.UI.WebCtrls;
using FounderSoftware.Framework.UI.WebPageFrame;

namespace FS.ADIM.OA.WebUI.WorkflowMenu.ProgramFileMaintain
{
    public partial class UC_ProgramType : UCBase
    {
        #region 定义GridView单元格索引
        private const int NAME = 1;//程序类型
        private const int DESCRIPTION = 2;//程序类型描述
        private const int SORT = 3;//程序分类
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
                //绑定程序类型列表
                this.gvProgramTypeList.PageType = PageType.InteriorPage;
                BindProgramTypeList();
                //绑定程序分类
                BindProgramSort(ddlProgramSort);
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
        /// 将检索得到所有的程序类型绑定到程序类型列表
        /// </summary>
        private void BindProgramTypeList()
        {
            B_ProgramType bllProgramType = new B_ProgramType();
            //得到所有程序类型
            DataTable dt = bllProgramType.GetAllProgramType();
            if (dt != null)
            {
                gvProgramTypeList.DataSource = dt;
                gvProgramTypeList.DataBind();
            }
        }


        /// <summary>
        /// 根据选择列表中的行加载显示相应的数据
        /// </summary>
        private void SelectRowBindDetail()
        {
            this.lnkbtnModify.Enabled = true;
            this.lnkbtnDelete.Enabled = true;
            hfSelectedIndex.Value = "selected";

            GridViewRow row = gvProgramTypeList.SelectedRow;

            //程序类型
            this.txtProgramType.Text = row.Cells[NAME].ToolTip.ToString();
            //程序类型描述
            this.txtProgramTypeDesc.Text = row.Cells[DESCRIPTION].ToolTip.ToString();
            //程序分类dropdownlist
            ListItem li = this.ddlProgramSort.Items.FindByText(row.Cells[SORT].ToolTip.ToString());
            ddlProgramSort.ClearSelection();
            if (li != null)
            {
                li.Selected = true;
            }
            else
            {
                ddlProgramSort.SelectedIndex = 0;
            }
        }

        /// <summary>
        /// 添加一条数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkbtnAdd_Click(object sender, EventArgs e)
        {
            B_ProgramType enProType = new B_ProgramType();

            //程序类型
            enProType.Name = this.txtProgramType.Text.Trim();
            //程序类型描述
            enProType.Description = this.txtProgramTypeDesc.Text.Trim();
            //程序分类
            enProType.Sort = this.ddlProgramSort.SelectedItem.Text;

            if (enProType.Save())
            {
                //绑定程序类型列表
                BindProgramTypeList();
                //清空页面数据
                ClearData();

                IMessage ms = new WebFormMessage(Page, ConstString.PromptInfo.ACTION_ADD_SUC);
                ms.Show();
            }
            else
            {
                IMessage im = new WebFormMessage(Page, enProType.ErrMsgs[0].ToString());
                im.Show();
            }
        }

        /// <summary>
        /// 修改选择的数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkbtnModify_Click(object sender, EventArgs e)
        {
            int index = gvProgramTypeList.SelectedIndex;
            int id = int.Parse(gvProgramTypeList.DataKeys[index]["ID"].ToString());

            B_ProgramType enProType = new B_ProgramType();

            enProType.ID = id;
            enProType.Name = this.txtProgramType.Text.ToString().Trim();//程序类型
            enProType.Description = this.txtProgramTypeDesc.Text.ToString().Trim();//程序类型描述
            enProType.Sort = this.ddlProgramSort.SelectedItem.Text;//程序分类

            if (enProType.Save())
            {
                //绑定程序类型列表
                BindProgramTypeList();
                //清空页面数据
                ClearData();
                IMessage ms = new WebFormMessage(Page, ConstString.PromptInfo.ACTION_EDIT_SUC);
                ms.Show();

            }
            else
            {
                IMessage ms = new WebFormMessage(Page, enProType.ErrMsgs[0].ToString());
                ms.Show();
            }
        }

        /// <summary>
        /// 删除选择的数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkbtnDelete_Click(object sender, EventArgs e)
        {
            int index = this.gvProgramTypeList.SelectedIndex;
            int id = int.Parse(gvProgramTypeList.DataKeys[index]["ID"].ToString());

            B_ProgramType enProType = new B_ProgramType();

            enProType.ID = id;

            if (enProType.Delete())
            {
                //绑定程序类型列表
                BindProgramTypeList();
                //清空页面数据
                ClearData();
                IMessage ms = new WebFormMessage(Page, ConstString.PromptInfo.ACTION_DEL_SUC);
                ms.Show();
            }
            else
            {
                IMessage ms = new WebFormMessage(Page, enProType.ErrMsgs[0].ToString());
                ms.Show();
            }
        }

        /// <summary>
        /// 清空页面数据
        /// </summary>
        private void ClearData()
        {
            this.lnkbtnModify.Enabled = false;
            this.lnkbtnDelete.Enabled = false;
            gvProgramTypeList.SelectedIndex = -1;
            hfSelectedIndex.Value = string.Empty;
            txtProgramType.Text = string.Empty;
            txtProgramTypeDesc.Text = string.Empty;
            ddlProgramSort.SelectedIndex = 0;
        }

        /// <summary>
        /// 翻页之后触发的事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvProgramTypeList_PageIndexChanged(object sender, EventArgs e)
        {
            ClearData();
        }

        /// <summary>
        /// 选择行事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvProgramTypeList_SelectedIndexChanged(object sender, EventArgs e)
        {
            SelectRowBindDetail();
        }

        /// <summary>
        /// 绑定行事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvProgramTypeList_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DataRowView row = e.Row.DataItem as DataRowView;
                e.Row.Cells[1].ToolTip = row["Name"].ToString().Trim();
                e.Row.Cells[2].ToolTip = row["Description"].ToString().Trim();
                e.Row.Cells[3].ToolTip = row["Sort"].ToString().Trim();

                e.Row.Cells[1].Text = row["Name"].ToString().Length > 10 ? row["Name"].ToString().Substring(0, 10) + "..." : row["Name"].ToString().Trim().Trim();
                e.Row.Cells[2].Text = row["Description"].ToString().Length > 20 ? row["Description"].ToString().Substring(0, 20) + "..." : row["Description"].ToString().Trim();
                e.Row.Cells[3].Text = row["Sort"].ToString().Length > 10 ? row["Sort"].ToString().Substring(0, 10) + "..." : row["Sort"].ToString().Trim();
            }
        }
    }
}