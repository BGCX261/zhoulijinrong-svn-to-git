using System;
using System.Data;
using FounderSoftware.Framework.Business;
using FS.ADIM.OA.BLL.Common;
using FS.ADIM.OA.BLL.SystemM;
using FS.ADIM.OU.OutBLL;
using System.Collections.Generic;
using FounderSoftware.ADIM.OU.BLL.Busi;
using System.Collections;

namespace FS.ADIM.OA.WebUI.PageOU
{
    public partial class PG_Company : OAPGBase
    {
        #region 变量

        /// <summary>
        /// ID控件
        /// </summary>
        protected String UCIDControl
        {
            get
            {
                return base.GetQueryString("UCIDControl");
            }
        }

        /// <summary>
        /// NO控件
        /// </summary>
        protected String UCNameControl
        {
            get
            {
                return base.GetQueryString("UCNameControl");
            }
        }

        /// <summary>
        /// Name控件
        /// </summary>
        protected String UCNoControl
        {
            get
            {
                return base.GetQueryString("UCNoControl");
            }
        }

        /// <summary>
        ///  是否单选 true:单选 false:多选 (默认单选)
        /// </summary>
        protected bool UCIsSingle
        {
            get
            {
                return Convert.ToBoolean(Request["UCIsSingle"]);
            }
        }

        ViewBase vb = null;
        #endregion

        #region 页面加载

        /// <summary>
        /// 页面加载
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            base.IsFirstBind = false;
            if (!IsPostBack)
            {
                ClientScriptM.ResponseScript(Page, "GetParent();");
                if (this.UCIsSingle)
                {
                    this.gvCompany.ShowCheckBox = false;
                    this.gvCompany.ShowRadioButton = true;
                    this.gvCompany.CheckTemplateHeaderText = "单选";
                    this.btnClose.Visible = false;
                }
                else
                {
                    this.gvCompany.ShowRadioButton = false;
                    this.gvCompany.ShowCheckBox = true;
                }
            }
        }

        #endregion

        /// <summary>
        /// gridview绑定
        /// </summary>
        private void LoadCompany()
        {
            vb = OADept.GetCompany(this.ddlType.SelectedIndex + 1, this.txtSearchText.Text.Trim());
            if (vb != null)
            {
                this.gvCompany.DataSource = vb.DtTable;
                this.gvCompany.DataBind();
            }
        }

        /// <summary>
        /// 脚本回传
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnOK_Click(object sender, EventArgs e)
        {
            String strIds = String.Empty;
            String strNos = String.Empty;
            String strNames = String.Empty;
            if (UCIsSingle)
            {
                strIds = this.gvCompany.SelectedKey;
            }
            else
            {
                strIds = SysString.GetStringFormatForList(this.gvCompany.SelectedKeys, ",");
            }
            vb = OADept.GetCompany(this.ddlType.SelectedIndex + 1, this.txtSearchText.Text.Trim());
            if (String.IsNullOrEmpty(strIds) == false)
            {
                if (vb != null)
                {
                    vb.Condition = "a.ID in (" + strIds + ")";
                    strIds = vb.GetFieldVals("ID", ";");
                    strNos = vb.GetFieldVals("No", ";");
                    strNames = vb.GetFieldVals("Name", ";")+";";
                }
            }

            String strScript = String.Empty;

            if (this.UCIDControl != String.Empty)
            {
                strScript += base.GetJSscriptValue(this.UCIDControl, strIds);
            }

            if (this.UCNoControl != String.Empty)
            {
                strScript += base.GetJSscriptValue(this.UCNoControl, strNos);
                strScript += base.GetJSscriptTitle(this.UCNoControl, strNames);
            }

            if (this.UCNameControl != String.Empty)
            {
                strScript += base.GetJSscriptValue(this.UCNameControl, strNames);
                strScript += base.GetJSscriptTitle(this.UCNameControl, strNos);
            }
            if (UCIsSingle)
            {
                strScript += String.Format("parent.ClosePopDiv('{0}')", base.divPopDivID + base.UCID);
            }
            //组成一整条js后运行
            ClientScriptM.ResponseScript(this, strScript);
        }

        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            this.LoadCompany();
        }
        /// <summary>
        /// 关闭
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnClose_Click(object sender, EventArgs e)
        {
            string strScript = String.Format("parent.ClosePopDiv('{0}')", base.divPopDivID + base.UCID);
            //组成一整条js后运行
            ClientScriptM.ResponseScript(this, strScript);
        }
        /// <summary>
        /// 清除
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnClear_Click(object sender, EventArgs e)
        {
            this.LoadCompany();//刷新显示
        }

        /// <summary>
        /// 刷新
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSX_Click(object sender, EventArgs e)
        {
            base.IsFirstBind = true;
            this.LoadCompany();
        }
    }
}
