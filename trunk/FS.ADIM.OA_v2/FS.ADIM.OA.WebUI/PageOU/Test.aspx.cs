using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using FS.ADIM.OU.OutBLL;

namespace FS.ADIM.OA.WebUI.PageOU
{
    public partial class Test : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            this.UC_Company1.UCIsSingle = false;
            this.UC_Company1.UCIDControl = this.TextBox1.ClientID;
            this.UC_Company1.UCNoControl = this.TextBox2.ClientID;
            this.UC_Company1.UCNameControl = this.TextBox3.ClientID;

            //this.UC_Company2.UCIDControl = this.TextBox1.ClientID;
            //this.UC_Company2.UCNoControl = this.TextBox2.ClientID;
            this.UC_Company2.UCNameControl = this.TextBox6.ClientID;

            this.RoleUC1.UCIsSingle = false;
            this.RoleUC1.UCRoleName = "公司领导";
            this.RoleUC1.UCUserIDControl = this.TextBox4.ClientID;
            this.RoleUC1.UCUserNameControl = this.TextBox5.ClientID;



            UCDeptSelect1.UCDeptIDControl = TextBox11.ClientID;
            UCDeptSelect1.UCDeptNameControl = TextBox12.ClientID;
            UCDeptSelect1.UCDeptUserIDControl = TextBox13.ClientID;
            UCDeptSelect1.UCDeptUserNameControl = TextBox14.ClientID;
            UCDeptSelect1.UCRoleUserIDControl = TextBox15.ClientID;
            UCDeptSelect1.UCRoleUserNameControl = TextBox16.ClientID;

            UCDeptSelect1.UCDeptTreeUserIDControl = TextBox17.ClientID;
            UCDeptSelect1.UCDeptTreeUserNameControl = TextBox18.ClientID;

            UCDeptSelect1.UCDeptAndUserControl = TextBox19.ClientID;
            UCDeptSelect1.UCSelectType = drpSelectType.SelectedValue;
            UCDeptSelect1.UCShowDeptID = txtShowDeptID.Text;
            UCDeptSelect1.UCLevel = txtLevel.Text;

            UCDeptSelect1.UCALLChecked = "1";
            UCDeptSelect1.UCFormName = "抄送";
            UCDeptSelect1.UCTemplateName = "公司发文";

            if (CheckBox1.Checked)
            {
                UCDeptSelect1.UCIsSingle = "1";
            }
            else
            {
                UCDeptSelect1.UCIsSingle = "";
            }

            if (CheckBox2.Checked)
            {
                UCDeptSelect1.UCAllSelect = "1";
            }
            else
            {
                UCDeptSelect1.UCAllSelect = "";
            }

            UCDeptSelect1.UCRole = txtRole.Text;

            string type = "";
            for (int i = 0; i < CheckBoxList1.Items.Count; i++)
            {
                if (CheckBoxList1.Items[i].Selected)
                {
                    type += "1";
                }
                else
                {
                    type += "0";
                }
            }
            type += "0";
            UCDeptSelect1.UCDeptShowType = type; //0000


        }

        protected void drpSelectType_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void DropDownList1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }



    }

}
