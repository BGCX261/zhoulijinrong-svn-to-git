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
using FS.ADIM.OA.WebUI.PageWF;

namespace FS.ADIM.OA.WebUI.WorkFlow.Receive.ReDetail
{
    public partial class UC_Step : System.Web.UI.UserControl
    {
        //步骤名称
        public String m_strStepName = null;
        /// <summary>
        /// 参与者
        /// </summary>
        public String StepName
        {
            set { m_strStepName = value; }
        }

        /// <summary>
        /// 参与者
        /// </summary>
        public String Participant
        {
            set { txtUser.Text = value; }
        }

        /// <summary>
        /// 提交动作
        /// </summary>
        public String SubmitAction
        {
            set { txtAction.Text = value; }
        }

        /// <summary>
        /// 提交时间
        /// </summary>
        public String SubmitDataTime
        {
            set { txtDateTime.Text = value; }
        }

        /// <summary>
        /// 意见
        /// </summary>
        public String Comment
        {
            set { txtComment.Text = value; }
        }

        /// <summary>
        /// 提示信息
        /// </summary>
        public String Prompt
        {
            set { txtPrompt.Text = value; }
        }

        /// <summary>
        /// 附件信息
        /// </summary>
        public UC_FileControl Attachment
        {
            get { return this.ucAttachment; }
            set { this.ucAttachment = value; }
        }

        public void HiddenAttach()
        {
            this.tdAttach.Visible = false;
        }

        /// <summary>
        /// 隐藏提示信息
        /// </summary>
        public void IsNoPrompt()
        {
            this.trprompt.Visible = false;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                this.ucAttachment.UCIsEditable = false;
            }
        }
    }
}