using System;
using FS.ADIM.OA.BLL.Common.Utility;
using FS.ADIM.OA.WebUI.UIBase;
using FS.ADIM.OA.BLL.Busi;

namespace FS.ADIM.OA.WebUI
{
    public partial class SetFromsID : System.Web.UI.Page
    {
        public B_OldToNew bt = new B_OldToNew();
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        #region 同步所有流程ID
        protected void Button1_Click(object sender, EventArgs e)
        {
            int ret = FormSave.SetID();
            if (ret>0)
            {
                this.Label1.Text = "成功！共" + ret.ToString() + "条语句";
                this.Label1.ForeColor = System.Drawing.Color.Blue;
            }
            else
            {
                this.Label1.Text = "失败";
                this.Label1.ForeColor = System.Drawing.Color.Red;
            }
        }
        #endregion

        #region 公司收文老数据处理
        protected void Button2_Click(object sender, EventArgs e)
        {
            int ret = bt.SetCompanyReceive();
            if (ret > 0)
            {
                this.Label2.Text = "完成! 共" + ret.ToString() + "条语句";
                this.Label2.ForeColor = System.Drawing.Color.Blue;
            }
            else
            {
                this.Label2.Text = "失败";
                this.Label2.ForeColor = System.Drawing.Color.Red;
            }
        }
        #endregion

        protected void Button3_Click(object sender, EventArgs e)
        {
            int ret = bt.SetLetterReceive();
            if (ret > 0)
            {
                this.Label3.Text = "完成! 共" + ret.ToString() + "条语句";
                this.Label3.ForeColor = System.Drawing.Color.Blue;
            }
            else
            {
                this.Label3.Text = "失败";
                this.Label3.ForeColor = System.Drawing.Color.Red;
            }
        }

        protected void Button4_Click(object sender, EventArgs e)
        {
            int ret = bt.SetCompanySend();
            if (ret > 0)
            {
                this.Label4.Text = "完成! 共" + ret.ToString() + "条语句";
                this.Label4.ForeColor = System.Drawing.Color.Blue;
            }
            else
            {
                this.Label4.Text = "失败";
                this.Label4.ForeColor = System.Drawing.Color.Red;
            }
        }

        protected void Button5_Click(object sender, EventArgs e)
        {
            int ret = bt.SetWorkRelation();
            if (ret > 0)
            {
                this.Label5.Text = "完成! 共" + ret.ToString() + "条语句";
                this.Label5.ForeColor = System.Drawing.Color.Blue;
            }
            else
            {
                this.Label5.Text = "失败";
                this.Label5.ForeColor = System.Drawing.Color.Red;
            }
        }

        protected void Button6_Click(object sender, EventArgs e)
        {
            int ret = bt.SetRequestReport();
            if (ret > 0)
            {
                this.Label6.Text = "完成! 共" + ret.ToString() + "条语句";
                this.Label6.ForeColor = System.Drawing.Color.Blue;
            }
            else
            {
                this.Label6.Text = "失败";
                this.Label6.ForeColor = System.Drawing.Color.Red;
            }
        }

        protected void Button7_Click(object sender, EventArgs e)
        {
            int[] ret = bt.RemoveOldDataGF();
            if (ret[0] >=0)
            {
                this.Label7.Text = "完成! 共" + ret[0].ToString() + "条语句,delete" + ret[1].ToString() + "条语句";
                this.Label7.ForeColor = System.Drawing.Color.Blue;
            }
            else
            {
                this.Label7.Text = "失败";
                this.Label7.ForeColor = System.Drawing.Color.Red;
            }
        }

        protected void Button8_Click(object sender, EventArgs e)
        {
            
            int[] ret = bt.RemoveOldDataGS();
            if (ret[0] >= 0)
            {
                this.Label8.Text = "完成! 共" + ret[0].ToString() + "条语句,delete" + ret[1].ToString() + "条语句";
                this.Label8.ForeColor = System.Drawing.Color.Blue;
            }
            else
            {
                this.Label8.Text = "失败";
                this.Label8.ForeColor = System.Drawing.Color.Red;
            }
        }

        protected void Button9_Click(object sender, EventArgs e)
        {
            int[] ret = bt.RemoveOldDataHF();
            if (ret[0] >=0)
            {
                this.Label9.Text = "完成! 共" + ret[0].ToString() + "条语句,delete" + ret[1].ToString() + "条语句";
                this.Label9.ForeColor = System.Drawing.Color.Blue;
            }
            else
            {
                this.Label9.Text = "失败";
                this.Label9.ForeColor = System.Drawing.Color.Red;
            }
        }

        protected void Button10_Click(object sender, EventArgs e)
        {
            int[] ret = bt.RemoveOldDataHS();
            if (ret[0] >= 0)
            {
                this.Label10.Text = "完成! 共" + ret[0].ToString() + "条语句,delete" + ret[1].ToString() + "条语句";
                this.Label10.ForeColor = System.Drawing.Color.Blue;
            }
            else
            {
                this.Label10.Text = "失败";
                this.Label10.ForeColor = System.Drawing.Color.Red;
            }
        }

        protected void Button11_Click(object sender, EventArgs e)
        {
            int[] ret = bt.RemoveOldDataWR();
            if (ret[0] >= 0)
            {
                this.Label11.Text = "完成! 共" + ret[0].ToString() + "条语句,delete" + ret[1].ToString() + "条语句";
                this.Label11.ForeColor = System.Drawing.Color.Blue;
            }
            else
            {
                this.Label11.Text = "失败";
                this.Label11.ForeColor = System.Drawing.Color.Red;
            }
        }
       
        protected void Button12_Click(object sender, EventArgs e)
        {
            int[] ret = bt.RemoveOldDataRR();
            if (ret[0] >= 0)
            {
                this.Label12.Text = "完成! 共" + ret[0].ToString() + "条语句,delete" + ret[1].ToString() + "条语句";
                this.Label12.ForeColor = System.Drawing.Color.Blue;
            }
            else
            {
                this.Label12.Text = "失败";
                this.Label12.ForeColor = System.Drawing.Color.Red;
            }
        }

        protected void Button13_Click(object sender, EventArgs e)
        {
            int[] ret = bt.RemoveOldDataPF();
            if (ret[0] >= 0)
            {
                this.Label13.Text = "完成! 共" + ret[0].ToString() + "条语句,delete" + ret[1].ToString() + "条语句";
                this.Label13.ForeColor = System.Drawing.Color.Blue;
            }
            else
            {
                this.Label13.Text = "失败";
                this.Label13.ForeColor = System.Drawing.Color.Red;
            }
        }

        protected void Button14_Click(object sender, EventArgs e)
        {
            int ret = bt.SetCompanyReceiveDate(false);
            if (ret > 0)
            {
                this.Label14.Text = "完成! 共" + ret.ToString() + "条语句";
                this.Label14.ForeColor = System.Drawing.Color.Blue;
            }
            else
            {
                this.Label14.Text = "失败";
                this.Label14.ForeColor = System.Drawing.Color.Red;
            }
        }

        protected void Button15_Click(object sender, EventArgs e)
        {
            int ret = bt.SetCompanyReceiveDate(true);
            if (ret > 0)
            {
                this.Label15.Text = "完成! 共" + ret.ToString() + "条语句";
                this.Label15.ForeColor = System.Drawing.Color.Blue;
            }
            else
            {
                this.Label15.Text = "失败";
                this.Label15.ForeColor = System.Drawing.Color.Red;
            }
        }

        protected void DJGTF_Click(object sender, EventArgs e)
        {
            int[] ret = bt.RemoveOldDataDJGTF();
            if (ret[0] >= 0)
            {
                this.Label16.Text = "完成! 共" + ret[0].ToString() + "条语句,delete" + ret[1].ToString() + "条语句";
                this.Label16.ForeColor = System.Drawing.Color.Blue;
            }
            else
            {
                this.Label16.Text = "失败";
                this.Label16.ForeColor = System.Drawing.Color.Red;
            }
        }

        protected void DJGTS_Click(object sender, EventArgs e)
        {
            int[] ret = bt.RemoveOldDataDJGTS();
            if (ret[0] >= 0)
            {
                this.Label17.Text = "完成! 共" + ret[0].ToString() + "条语句,delete" + ret[1].ToString() + "条语句";
                this.Label17.ForeColor = System.Drawing.Color.Blue;
            }
            else
            {
                this.Label17.Text = "失败";
                this.Label17.ForeColor = System.Drawing.Color.Red;
            }
        }
    }
}
