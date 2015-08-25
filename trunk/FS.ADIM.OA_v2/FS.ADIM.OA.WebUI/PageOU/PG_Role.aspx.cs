using System;
using System.Collections;
using System.Data;
using System.Web.UI.WebControls;
using FounderSoftware.Framework.Business;
using FS.ADIM.OA.BLL.SystemM;
using FS.ADIM.OU.OutBLL;

namespace FS.ADIM.OA.WebUI.PageOU
{
    public partial class PG_Role : OAPGBase
    {
        #region 变量

        /// <summary>
        /// 角色名
        /// </summary>
        protected string UCRoleName
        {
            get
            {
                return base.GetQueryString("UCRoleName");
            }
        }

        /// <summary>
        /// 用户IDs控件集合
        /// </summary>
        protected string UCUserIDControl
        {
            get
            {
                return base.GetQueryString("UCUserIDControl");
            }
        }

        /// <summary>
        /// 用户Names控件集合
        /// </summary>
        protected string UCUserNameControl
        {
            get
            {
                return base.GetQueryString("UCUserNameControl");
            }
        }

        /// <summary>
        ///  是否单选 true:单选 false:多选 (默认多选)
        /// </summary>
        protected bool UCIsSingle
        {
            get
            {
                return Convert.ToBoolean(Request["UCIsSingle"]);
            }
        }

        #endregion

        #region 页面加载

        /// <summary>
        /// 页面加载
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ClientScriptM.ResponseScript(Page, "GetParent();");

                if (this.UCIsSingle)
                {
                    this.gvRole.Columns[0].Visible = false;
                }
                else
                {
                    this.gvRole.Columns[1].Visible = false;
                }
            }
        }

        #endregion

        #region gridview绑定

        /// <summary>
        /// gridview绑定
        /// </summary>
        protected void Bind()
        {
            ViewBase vb = OAUser.GetUserByRole(this.UCRoleName);

            if (vb != null && vb.Count > 0)
            {
                this.gvRole.DataSource = vb.DtTable;
                this.gvRole.DataBind();
            }
        }

        #endregion

        #region 事件

        /// <summary>
        /// 确定按钮事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnOK_Click(object sender, EventArgs e)
        {
            ArrayList arrIDS = new ArrayList();
            ArrayList arrNameS = new ArrayList();
            foreach (ListItem item in this.fsltbRoleUser.Items)
            {
                arrIDS.Add(item.Value);
                arrNameS.Add(item.Text);
            }
            String strClientScript = String.Empty;
            String strIds = base.GetStringText(arrIDS).Replace(@"\", @"\\");//帐号
            String strNames = base.GetStringText(arrNameS);// 姓名

            //chenye
            if (UCIsSingle)
            {
                if (UCUserIDControl != String.Empty)
                {
                    strClientScript += base.GetJSscriptXMLValue(UCUserIDControl, strIds);
                }
                if (UCUserNameControl != String.Empty)
                {
                    strClientScript += base.GetJSscriptXMLValue(UCUserNameControl, strNames);
                }
            }
            else
            {
                if (UCUserIDControl != String.Empty)
                {
                    strClientScript += base.GetJSscriptValue(UCUserIDControl, strIds);
                }
                if (UCUserNameControl != String.Empty)
                {
                    strClientScript += base.GetJSscriptValue(UCUserNameControl, strNames);
                }                
            }
            strClientScript += string.Format("parent.ClosePopDiv('{0}')", base.divPopDivID + base.UCID);
            //组成一整条js后运行
            ClientScriptM.ResponseScript(this, strClientScript);

        }

        /// <summary>
        /// 行的绑定事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvRole_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (!base.IsFirstBind)
            {
                return;
            }
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                CheckBox chkStatus = new CheckBox();
                if (this.UCIsSingle)
                {
                    chkStatus = e.Row.FindControl("rbtnStatus") as RadioButton;
                }
                else
                {
                    chkStatus = e.Row.FindControl("chkStatus") as CheckBox;
                }
                string userID = (e.Row.Cells[4].FindControl("lblUserID") as Label).Text;
                string userName = e.Row.Cells[3].Text;
                ListItem item = new ListItem(userName, userID);
                //if (this.fsltbRoleUser.Items.Contains(item))
                //{
                //    chkStatus.Checked = true;
                //}
                if (string.IsNullOrEmpty(hUserID.Value) == false && base.IsChecked(hUserID.Value, userID))
                {
                    base.SelectedID = (e.Row.DataItem as DataRowView)["ID"].ToString();
                    chkStatus.Checked = true;
                    this.fsltbRoleUser.Items.Add(item);
                }
            }
        }

        /// <summary>
        /// 复选框按钮事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void chkStatus_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox cb = sender as CheckBox;
            string strID = cb.ToolTip.Split('|')[0];
            string strName = cb.ToolTip.Split('|')[1];
            if (cb.Checked)
            {
                ListItem item = new ListItem(strName, strID);
                if (this.ListBoxExists(this.fsltbRoleUser, strID) == false)
                {
                    this.fsltbRoleUser.Items.Add(item);
                }
            }
            else
            {
                if (this.ListBoxExists(this.fsltbRoleUser, strID))
                {
                    foreach (ListItem item in this.fsltbRoleUser.Items)
                    {
                        if (item.Value == strID)
                        {
                            this.fsltbRoleUser.Items.Remove(item);
                            break;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 单选按钮事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void rbtnStatus_CheckedChanged(object sender, EventArgs e)
        {
            this.fsltbRoleUser.Items.Clear();

            //首先设置原来的选中属性为false,
            for (int i = 0; i < this.gvRole.Rows.Count; i++)
            {
                RadioButton rb = gvRole.Rows[i].Cells[1].FindControl("rbtnStatus") as RadioButton;
                if (this.gvRole.Rows[i].Cells[2].Text == base.SelectedID)
                {
                    rb.Checked = false;
                }
            }

            //记录这次选中的信息
            for (int i = 0; i < this.gvRole.Rows.Count; i++)
            {
                RadioButton rb = this.gvRole.Rows[i].Cells[1].FindControl("rbtnStatus") as RadioButton;
                string strID = rb.ToolTip.Split('|')[0];
                string strName = rb.ToolTip.Split('|')[1];
                if (rb.Checked)
                {
                    base.SelectedID = this.gvRole.Rows[i].Cells[2].Text;
                    this.fsltbRoleUser.Items.Add(new ListItem(strName, strID));
                }
            }
        }

        /// <summary>
        /// 刷新
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSX_Click(object sender, EventArgs e)
        {
            base.IsFirstBind = true;
            this.Bind();
        }

        /// <summary>
        /// ListBox中是否已经存在某项
        /// </summary>
        /// <param name="lb">数据源</param>
        /// <param name="value">对比值</param>
        /// <returns>bool</returns>
        protected bool ListBoxExists(ListControl lb, string value)
        {
            Boolean bExist = false;
            foreach (ListItem li in lb.Items)
            {
                if (li.Value == value)
                {
                    bExist = true;
                }
            }
            return bExist;
        }

        /// <summary>
        /// 清空事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnClear_Click(object sender, EventArgs e)
        {
            this.fsltbRoleUser.Items.Clear();
            foreach (GridViewRow item in this.gvRole.Rows)
            {
                if (item.Cells[0].FindControl("chkStatus") != null)
                {
                    (item.Cells[0].FindControl("chkStatus") as CheckBox).Checked = false;
                }
                if (item.Cells[1].FindControl("rbtnStatus") != null)
                {
                    (item.Cells[1].FindControl("rbtnStatus") as RadioButton).Checked = false;
                }
            }
        }

        #endregion
    }
}
