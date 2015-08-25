using System;

namespace FS.ADIM.OA.WebUI.PageOU
{
    public partial class UC_OASelect : OAUCBase
    {
        #region 页面加载

        /// <summary>
        /// 页面加载  
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (UCSelectType == "0")
            {
                SHead = "选取部门";
                base.DivWidth = "420";
                base.DivHeight = "420";
            }
            else if (UCSelectType == "1")
            {
                SHead = "选取人员";
                base.DivWidth = "635";
                base.DivHeight = "446";
            }
            else if (UCSelectType == "2")
            {
                SHead = "选取部门及人员";
                base.DivWidth = "635";
                base.DivHeight = "446";
            }
            else
            {
                SHead = "选取部门及人员";
                base.DivWidth = "635";
                base.DivHeight = "446";
            }

            if (UCRole != string.Empty)
            {
                SHead += "+" + UCRole;
                base.DivHeight = (Convert.ToInt32(base.DivHeight) + 50).ToString();
            }
        }

        #endregion

        #region 回传变量

        /// <summary>
        /// 部门名称控件
        /// </summary>
        public String UCDeptNameControl
        {
            get
            {
                if (ViewState["UCDeptNameControl"] == null)
                {
                    ViewState["UCDeptNameControl"] = string.Empty;
                }
                return ViewState["UCDeptNameControl"] as String;
            }
            set
            {
                ViewState["UCDeptNameControl"] = value;
            }
        }

        /// <summary>
        /// 部门ID控件
        /// </summary>
        public String UCDeptIDControl
        {
            get
            {
                if (ViewState["UCDeptIDControl"] == null)
                {
                    ViewState["UCDeptIDControl"] = string.Empty;
                }
                return ViewState["UCDeptIDControl"] as String;
            }
            set
            {
                ViewState["UCDeptIDControl"] = value;
            }
        }

        /// <summary>
        /// 部门用户ID控件--右边选择的
        /// </summary>
        public string UCDeptUserIDControl
        {
            get
            {
                if (ViewState["UCDeptUserIDControl"] == null)
                {
                    ViewState["UCDeptUserIDControl"] = string.Empty;
                }
                return ViewState["UCDeptUserIDControl"] as String;
            }
            set
            {
                ViewState["UCDeptUserIDControl"] = value;
            }
        }

        /// <summary>
        /// 部门用户Name控件 回传
        /// </summary>
        public string UCDeptUserNameControl
        {
            get
            {
                if (ViewState["UCDeptUserNameControl"] == null)
                {
                    ViewState["UCDeptUserNameControl"] = string.Empty;

                }
                return ViewState["UCDeptUserNameControl"] as String;
            }
            set
            {
                ViewState["UCDeptUserNameControl"] = value;

            }
        }

        /// <summary>
        /// 部门和人员名都显示在一个文本框内
        /// </summary>
        public string UCDeptAndUserControl
        {
            get
            {
                if (ViewState["UCDeptAndUserControl"] == null)
                {
                    ViewState["UCDeptAndUserControl"] = string.Empty;
                }
                return ViewState["UCDeptAndUserControl"] as string;
            }
            set
            {
                ViewState["UCDeptAndUserControl"] = value;
            }
        }

        /// <summary>
        /// 角色用户ID控件
        /// </summary>
        public string UCRoleUserIDControl
        {
            get
            {
                if (ViewState["UCRoleUserIDControl"] == null)
                {
                    ViewState["UCRoleUserIDControl"] = string.Empty;
                }
                return ViewState["UCRoleUserIDControl"] as String;
            }
            set
            {
                ViewState["UCRoleUserIDControl"] = value;

            }
        }

        /// <summary>
        /// 角色用户Name控件 回传
        /// </summary>
        public string UCRoleUserNameControl
        {
            get
            {
                if (ViewState["UCRoleUserNameControl"] == null)
                {
                    ViewState["UCRoleUserNameControl"] = string.Empty;
                }
                return ViewState["UCRoleUserNameControl"] as String;
            }
            set
            {
                ViewState["UCRoleUserNameControl"] = value;

            }
        }

        /// <summary>
        /// 部门树的用户ID控件
        /// </summary>
        public string UCDeptTreeUserIDControl
        {
            get
            {
                if (ViewState["UCDeptTreeUserIDControl"] == null)
                {
                    ViewState["UCDeptTreeUserIDControl"] = string.Empty;
                }
                return ViewState["UCDeptTreeUserIDControl"] as String;
            }
            set
            {
                ViewState["UCDeptTreeUserIDControl"] = value;

            }
        }

        /// <summary>
        /// 部门树上的用户Names控件 回传
        /// </summary>
        public string UCDeptTreeUserNameControl
        {
            get
            {
                if (ViewState["UCDeptTreeUserNameControl"] == null)
                {
                    ViewState["UCDeptTreeUserNameControl"] = string.Empty;
                }
                return ViewState["UCDeptTreeUserNameControl"] as String;
            }
            set
            {
                ViewState["UCDeptTreeUserNameControl"] = value;

            }
        }


        #endregion

        #region 辅助功能

        /// <summary>
        /// 选择类型 0 1 2 ，0是部门 1是人,2是2者都是
        /// </summary>
        public String UCSelectType
        {
            get
            {
                if (ViewState["UCSelectType"] == null)
                {
                    ViewState["UCSelectType"] = string.Empty;
                }
                return ViewState["UCSelectType"] as String;
            }
            set
            {
                ViewState["UCSelectType"] = value;
            }
        }

        /// <summary>
        /// 流程模板的名称
        /// </summary>
        public string UCTemplateName
        {
            get
            {
                if (ViewState["UCTemplateName"] == null)
                {
                    ViewState["UCTemplateName"] = string.Empty;
                }
                return ViewState["UCTemplateName"] as string;
            }
            set
            {
                ViewState["UCTemplateName"] = value;
            }
        }

        /// <summary>
        /// 是否单选
        /// </summary>
        public String UCIsSingle
        {
            get
            {
                if (ViewState["UCIsSingle"] == null)
                {
                    ViewState["UCIsSingle"] = string.Empty;
                }
                return ViewState["UCIsSingle"] as String;
            }
            set
            {
                ViewState["UCIsSingle"] = value;
            }
        }

        /// <summary>
        /// 显示可用的部门
        /// </summary>
        public String UCShowDeptID
        {
            get
            {
                if (ViewState["UCShowDeptID"] == null)
                {
                    ViewState["UCShowDeptID"] = string.Empty;
                }
                return ViewState["UCShowDeptID"] as String;
            }
            set
            {
                ViewState["UCShowDeptID"] = value;
            }
        }

        /// <summary>
        /// 显示可选择的层级
        /// </summary>
        public String UCLevel
        {
            get
            {
                if (ViewState["UCLevel"] == null)
                {
                    ViewState["UCLevel"] = string.Empty;
                }
                return ViewState["UCLevel"] as String;
            }
            set
            {
                ViewState["UCLevel"] = value;
            }
        }

        /// <summary>
        /// 其他角色
        /// </summary>
        public String UCRole
        {
            get
            {
                if (ViewState["UCRole"] == null)
                {
                    ViewState["UCRole"] = string.Empty;
                }
                return ViewState["UCRole"] as String;
            }
            set
            {
                ViewState["UCRole"] = value;
            }
        }

        /// <summary>
        /// 部门上显示哪一类人 1111 1负责人 2职位大于副处 3部门领导 4待定
        /// </summary>
        public String UCDeptShowType
        {
            get
            {
                if (ViewState["UCDeptShowType"] == null)
                {
                    ViewState["UCDeptShowType"] = string.Empty;
                }
                return ViewState["UCDeptShowType"] as String;
            }
            set
            {
                ViewState["UCDeptShowType"] = value;
            }
        }

        /// <summary>
        /// 控制父节点选中子节点是否也选中( "1":全选 "0":不选中)
        /// </summary>
        public String UCAllSelect
        {
            get
            {
                if (ViewState["UCAllSelect"] == null)
                {
                    ViewState["UCAllSelect"] = string.Empty;
                }
                return ViewState["UCAllSelect"] as String;
            }
            set
            {
                ViewState["UCAllSelect"] = value;
            }
        }

        /// <summary>
        /// CheckBox是否全选(默认不全选，“1”为全选)
        /// </summary>
        public string UCALLChecked
        {
            get
            {
                if (ViewState["UCALLChecked"] == null)
                {
                    ViewState["UCALLChecked"] = string.Empty;
                }
                return ViewState["UCALLChecked"] as string;
            }
            set
            {
                ViewState["UCALLChecked"] = value;
            }
        }

        /// <summary>
        ///  流程表单的名称
        /// </summary>
        public string UCFormName
        {
            get
            {
                if (ViewState["UCFormName"] == null)
                {
                    ViewState["UCFormName"] = string.Empty;
                }
                return ViewState["UCFormName"] as string;
            }
            set
            {
                ViewState["UCFormName"] = value;
            }
        }

        #endregion
    }
}