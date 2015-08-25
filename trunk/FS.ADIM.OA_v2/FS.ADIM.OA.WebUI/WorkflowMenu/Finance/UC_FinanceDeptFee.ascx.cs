using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FounderSoftware.Framework.UI.WebPageFrame;
using FounderSoftware.Framework.UI.WebCtrls;
using System.Data;
using FS.ADIM.OA.BLL.Busi.InfoMaintain;
using FS.ADIM.OA.BLL.Common;
using FS.ADIM.OU.OutBLL;
using FounderSoftware.Framework.Business;

namespace FS.ADIM.OA.WebUI.WorkflowMenu.Finance
{
    public partial class UC_FinanceDeptFee : UCBase
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
                this.gvFinanceFeeList.PageType = PageType.InteriorPage;
                BindFinanceList();
                //绑定部门
                BindDeptList(ddlDept);
            }
        }

        /// <summary>
        /// 绑定部门
        /// </summary>
        /// <param name="ddl"></param>
        private void BindDeptList(DropDownList ddl)
        {
            OADept.GetDeptByIfloor(ddlDept, 1);
        }

        /// <summary>
        /// 
        /// </summary>
        private void BindFinanceList()
        {
            B_FinanceDeptInfo bllFinanceList = new B_FinanceDeptInfo();
            //得到所有财务费用
            DataTable dt = bllFinanceList.GetAllFinanceDeptInfo();
            if (dt != null)
            {
                gvFinanceFeeList.DataSource = dt;
                gvFinanceFeeList.DataBind();
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

            GridViewRow row = gvFinanceFeeList.SelectedRow;

            ListItem YearItem = this.ddlFinanceYear.Items.FindByText(row.Cells[1].Text.ToString());
            ddlFinanceYear.ClearSelection();
            if (YearItem != null)
            {
                YearItem.Selected = true;
            }
            else
            {
                ddlFinanceYear.SelectedIndex = 0;
            }

            ListItem li = this.ddlDept.Items.FindByText(row.Cells[2].Text.ToString());
            ddlDept.ClearSelection();
            if (li != null)
            {
                li.Selected = true;
            }
            else
            {
                ddlDept.SelectedIndex = 0;
            }

            this.txtTripBudget.Text = row.Cells[3].Text.ToString();
            this.txtTripUse.Text = row.Cells[4].Text.ToString();
            this.txtTrainingBudget.Text = row.Cells[5].Text.ToString();
            this.txtTrainingUse.Text = row.Cells[6].Text.ToString();
            this.txtZDBudget.Text = row.Cells[7].Text.ToString();
            this.txtZDUse.Text = row.Cells[8].Text.ToString();
        }

        /// <summary>
        /// 添加一条数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkbtnAdd_Click(object sender, EventArgs e)
        {
            B_FinanceDeptInfo financeDeptInfo = new B_FinanceDeptInfo();
            financeDeptInfo.FinanceYear = this.ddlFinanceYear.SelectedValue;
            financeDeptInfo.DeptID = this.ddlDept.SelectedValue;
            financeDeptInfo.TripBudgetCost = this.txtTripBudget.Text.Trim();
            financeDeptInfo.TrainingBudgetCost = this.txtTrainingBudget.Text;
            financeDeptInfo.ZDBudgetCost = this.txtZDBudget.Text;
            financeDeptInfo.TripUseCost ="0";
            financeDeptInfo.TrainingUseCost = "0";
            financeDeptInfo.ZDBudgetCost = "0";
            if (financeDeptInfo.Save())
            {
                //得到所有财务费用
                BindFinanceList();
                //清空页面数据
                ClearData();

                IMessage ms = new WebFormMessage(Page, ConstString.PromptInfo.ACTION_ADD_SUC);
                ms.Show();
            }
            else
            {
                IMessage im = new WebFormMessage(Page, financeDeptInfo.ErrMsgs[0].ToString());
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
            int index = gvFinanceFeeList.SelectedIndex;
            int id = int.Parse(gvFinanceFeeList.DataKeys[index]["ID"].ToString());

            B_FinanceDeptInfo financeDeptInfo = new B_FinanceDeptInfo();
            financeDeptInfo.ID = id;
            financeDeptInfo.FinanceYear = this.ddlFinanceYear.SelectedValue;
            financeDeptInfo.DeptID = this.ddlDept.SelectedValue;
            financeDeptInfo.TripBudgetCost = this.txtTripBudget.Text.Trim();
            financeDeptInfo.TrainingBudgetCost = this.txtTrainingBudget.Text;          
            financeDeptInfo.ZDBudgetCost = this.txtZDBudget.Text;
            financeDeptInfo.TripUseCost = this.txtTripUse.Text;
            financeDeptInfo.TrainingUseCost = this.txtTrainingUse.Text;
            financeDeptInfo.ZDUseCost = this.txtZDUse.Text;
            if (financeDeptInfo.Save())
            {
                //得到所有财务费用
                BindFinanceList();
                //清空页面数据
                ClearData();
                IMessage ms = new WebFormMessage(Page, ConstString.PromptInfo.ACTION_EDIT_SUC);
                ms.Show();

            }
            else
            {
                IMessage ms = new WebFormMessage(Page, financeDeptInfo.ErrMsgs[0].ToString());
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
            int index = this.gvFinanceFeeList.SelectedIndex;
            int id = int.Parse(gvFinanceFeeList.DataKeys[index]["ID"].ToString());

            B_FinanceDeptInfo financeDeptInfo = new B_FinanceDeptInfo();
            financeDeptInfo.Load(id);
            financeDeptInfo.DeleteById(id.ToString());

            BindFinanceList();
            //清空页面数据
            ClearData();
            IMessage ms = new WebFormMessage(Page, ConstString.PromptInfo.ACTION_DEL_SUC);
            ms.Show();
        }

        /// <summary>
        /// 清空页面数据
        /// </summary>
        private void ClearData()
        {
            this.lnkbtnModify.Enabled = false;
            this.lnkbtnDelete.Enabled = false;
            gvFinanceFeeList.SelectedIndex = -1;
            hfSelectedIndex.Value = string.Empty;

            txtTripBudget.Text = string.Empty;
            txtTrainingBudget.Text = string.Empty;
            txtZDBudget.Text = string.Empty;
            txtTripUse.Text = string.Empty;
            txtTrainingUse.Text = string.Empty;
            txtZDUse.Text = string.Empty;       
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
        protected void gvFinanceFeeList_SelectedIndexChanged(object sender, EventArgs e)
        {
            SelectRowBindDetail();
        }
    }
}