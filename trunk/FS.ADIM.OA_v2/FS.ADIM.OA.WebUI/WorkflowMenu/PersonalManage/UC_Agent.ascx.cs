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
using FS.ADIM.OA.BLL.SystemM;
using FS.ADIM.OA.WebUI.PageOU;
using FS.ADIM.OA.BLL.Common.Utility;
using FS.ADIM.OU.OutBLL;
using FS.ADIM.OA.BLL.Common;
using FounderSoftware.Framework.Business;
using FS.ADIM.OA.BLL.Busi.Menu;
using FS.ADIM.OA.BLL;
using FS.ADIM.OA.BLL.Entity.Menu;
using System.Web.UI.MobileControls;
using System.Collections.Generic;

namespace FS.ADIM.OA.WebUI.WorkflowMenu.PersonalManage
{
    public partial class UC_Agent : System.Web.UI.UserControl
    {
        //支持回传部门ID 部门名,人员帐号 人员名

        #region 回传变量

        /// <summary>
        /// 部门ID控件
        /// </summary>
        public string UCDeptIDControl;

        /// <summary>
        /// 部门Names控件 
        /// </summary>
        public string UCDeptNameControl;

        /// <summary>
        /// 部门的用户ID控件
        /// </summary>
        public string UCDeptUserIDControl;

        /// <summary>
        /// 部门的用户Names控件 
        /// </summary>
        public string UCDeptUserNameControl;

        /// <summary>
        /// 部门和人员名都显示在一个文本框内
        /// </summary>
        public string UCDeptAndUserControl;

        /// <summary>
        /// 部门树上的用户ID控件
        /// </summary>
        public string UCDeptTreeUserIDControl;

        /// <summary>
        /// 部门树上的用户Names控件 回传
        /// </summary>
        public string UCDeptTreeUserNameControl;

        /// <summary>
        /// 角色用户ID控件
        /// </summary>
        public string UCRoleUserIDControl;

        /// <summary>
        /// 角色用户Names控件 回传
        /// </summary>
        public string UCRoleUserNameControl;

        #endregion

        #region 辅助功能

        /// <summary>
        /// 选择类型 0 1 2，0是部门 1是人,2是2者都是
        /// </summary>
        public string UCSelectType;

        /// <summary>
        /// 是否单选
        /// </summary>
        public Boolean UCIsSingle;

        /// <summary>
        /// 显示可用的部门
        /// </summary>
        public String UCShowDeptID;

        /// <summary>
        /// 显示可选择的层级
        /// </summary>
        public String UCLevel;

        /// <summary>
        /// 其他角色
        /// </summary>
        public String UCRole;

        /// <summary>
        /// 部门上显示哪一类人 1111 1负责人 2职位大于副处 3部门领导 4待定
        /// </summary>
        public String UCDeptShowType;

        /// <summary>
        /// 是否全选（1.全选 0.否）
        /// </summary>
        public Boolean UCAllSelect;

        //选中的ID
        protected string SelectedID
        {
            get
            {
                if (ViewState["SelectedID"] == null)
                {
                    ViewState["SelectedID"] = "";
                }
                return ViewState["SelectedID"] as string;
            }
            set
            {
                ViewState["SelectedID"] = value;
            }
        }

        string type1 = string.Empty;
        string type2 = string.Empty;
        string type3 = string.Empty;
        string type4 = string.Empty;

        #endregion

        #region 页面加载

        /// <summary>
        /// 页面加载
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            #region 页面显示控制
            UCSelectType = "1";//只选人
            UCIsSingle = true;//单选
            UCDeptIDControl = hUCDeptID.ClientID;
            UCDeptUserIDControl = hUCDeptUserID.ClientID;
            UCShowDeptID = OADept.GetDeptIDByUser2(CurrentUserInfo.LoginName);
            hUCDeptUserID.Value = B_WFAgent.GetAgentUserID(CurrentUserInfo.UserName);

            if (string.IsNullOrEmpty(UCShowDeptID))
            {
                UCShowDeptID = string.Empty;
            }
            if (string.IsNullOrEmpty(UCDeptShowType))
            {
                UCDeptShowType = string.Empty;
            }

            //如果需要选人
            if (UCSelectType == "1")
            {
                tvDB.SelectedNodeChanged += new EventHandler(tvDB_SelectedNodeChanged);
            }
            else if (UCSelectType == "2")
            {
                tvDB.SelectedNodeChanged += new EventHandler(tvDB_SelectedNodeChanged);
            }

            if (UCIsSingle || UCAllSelect)
            {
                this.lboxLeft.SelectionMode = System.Web.UI.WebControls.ListSelectionMode.Single;
                tvDB.TreeNodeCheckChanged += new TreeNodeEventHandler(tvDB_TreeNodeCheckChanged);
            }

            #endregion

            if (!IsPostBack)
            {
                //ClientScriptM.ResponseScript(Page, "GetParent();");
                Refresh();
            }
        }

        #endregion

        #region 绑定DB部门树

        private DataTable dtDept = new DataTable();//部门
        private ArrayList hasDeptID = new ArrayList();
        private DataTable dtDeptMember = new DataTable();//部门成员 
        private bool bManager = false;
        private bool bLeader = false;
        private string strPName = string.Empty;

        private int iLevel = -1;

        /// <summary>
        /// 绑定树
        /// </summary>
        private void BindDBTree()
        {
            if (!string.IsNullOrEmpty(UCLevel))
            {
                iLevel = SysConvert.ToInt32(UCLevel);
            }
            hasDeptID = SysString.GetStringToArrayList(UCShowDeptID, ',');
            dtDept = OADept.GetDeptInfo(iLevel).DtTable;
            //this.ShowTitle();
            DataRow[] drs = dtDept.Select("FloorCode=0");//最顶层
            tvDB.Nodes.Clear();
            foreach (DataRow dr in drs)
            {
                TreeNode newNode = new TreeNode();
                newNode = SetTreeNode(newNode, dr); //设置节点上显示的东西
                tvDB.Nodes.Add(newNode);
                DataRow[] child = dtDept.Select("ParentID=" + dr["ID"].ToString());
                if (child.Length > 0)
                {
                    BindDBTreeSub(newNode, child);
                }
            }
        }

        /// <summary>
        /// 绑定子部门
        /// </summary>
        /// <param name="node"></param>
        /// <param name="drs"></param>
        private void BindDBTreeSub(TreeNode node, DataRow[] drs)
        {
            foreach (DataRow dr in drs)
            {
                TreeNode newNode = new TreeNode();
                newNode = SetTreeNode(newNode, dr);

                ////回传绑定
                //if (IsChecked(hUCDeptID.Value, dr["ID"].ToString()))
                //{
                //    newNode.Checked = true;
                //}
                node.ChildNodes.Add(newNode);

                DataRow[] child = dtDept.Select("ParentID=" + dr["ID"].ToString());
                if (child.Length > 0)
                {
                    BindDBTreeSub(newNode, child);
                }
            }
        }

        /// <summary>
        /// 设置节点显示
        /// </summary>
        /// <param name="node"></param>
        /// <param name="dr"></param>
        /// <returns></returns>
        private TreeNode SetTreeNode(TreeNode node, DataRow dr)
        {
            TreeNode newNode = new TreeNode();
            ViewBase vbDeptUser = OAUser.GetUserByDeptID(dr["ID"].ToString(), ConstString.Grade.ZERO);
            string name = dr["Name"].ToString();
            if (vbDeptUser != null)
            {
                name += "[" + vbDeptUser.Count.ToString() + "]";
            }

            if (UCDeptShowType != string.Empty)
            {
                string member = GetDeptMember(dr["ID"].ToString())[1];
                if (string.IsNullOrEmpty(member) == false && member.Length > 0)
                {
                    member = "(" + member + ")";
                }
                name += member;
                if (name.Length > 20)
                {
                    name = name.Substring(0, 20) + "...";
                }
                newNode = new TreeNode(name, dr["ID"].ToString());

                //鼠标移上去提示
                newNode.ToolTip = dr["Name"].ToString() + member;

                GetType1();

                if (type4 != "1")
                {
                    if (string.IsNullOrEmpty(member))
                    {
                        newNode.ShowCheckBox = false; //没有成员 不可选择
                    }
                    else
                    {
                        newNode.ShowCheckBox = true;
                    }
                }

            }
            //node.Value=部门主键ID
            else
            {
                newNode = new TreeNode(name, dr["ID"].ToString());
            }
            if (UCShowDeptID != string.Empty)
            {
                if (hasDeptID.Count > 0)
                {
                    if (hasDeptID.Contains(newNode.Value) == false)
                    {
                        newNode.SelectAction = TreeNodeSelectAction.None;//置灰
                        newNode.ShowCheckBox = false;
                    }
                }
            }
            return newNode;
        }

        /// <summary>
        /// 树节点显示的内容类型
        /// </summary>
        private void GetType1()
        {
            if (UCDeptShowType != string.Empty)
            {
                if (UCDeptShowType.Length != 4)
                {
                    throw new Exception("UCDeptShowType必须是四位数字。");
                }
                else
                {
                    type1 = UCDeptShowType.Substring(0, 1);
                    type2 = UCDeptShowType.Substring(1, 1);
                    type3 = UCDeptShowType.Substring(2, 1);
                    type4 = UCDeptShowType.Substring(3, 1);
                }
            }
        }

        /// <summary>
        /// 获得部门成员(负责人、部门领导、大于副处长)
        /// </summary>
        private string[] GetDeptMember(string strDeptID)
        {
            GetType1();
            if (type1 == "1") //负责人
            {
                bManager = true;
            }
            if (type2 == "1") //职位大于副处长
            {
                strPName = OUConstString.PostName.FUCHUZHANG;
            }
            if (type3 == "1") //部门领导
            {
                bLeader = true;
            }
            return OAUser.GetUserByDeptPostArray(strDeptID, strPName, bManager, bLeader);
        }

        #endregion

        #region 部门树选中节点后筛选人员

        protected void tvDB_SelectedNodeChanged(object sender, EventArgs e)
        {
            BindLeftUser();
        }

        #endregion

        #region 移动人员

        /// <summary>
        /// 加选中的人
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnAddOne_Click(object sender, EventArgs e)
        {
            int[] iSelected = lboxLeft.GetSelectedIndices();
            List<B_WFAgent> enList2 = new List<B_WFAgent>();

            if (iSelected.Length == 0)
            {
                JScript.Alert("请选择人员。", true);
                return;
            }
            else
            {
                for (int i = 0; i < iSelected.Length; i++)
                {
                    if (lboxLeft.Items[iSelected[i]].Value == CurrentUserInfo.UserName)
                    {
                        JScript.Alert("不能选择自己为代理人。", true);
                        return;
                    }
                    if (B_WFAgent.IsUse(lboxLeft.Items[iSelected[i]].Value, CurrentUserInfo.UserName))
                    {
                        JScript.Alert(lboxLeft.Items[iSelected[i]].Text + " 已被他人设为代理人，请选择其他人员进行代理。", true);
                        return;
                    }
                }

                foreach (ListItem itm in this.lboxRight.Items)
                {
                    B_WFAgent entity = new B_WFAgent();
                    entity.AgentUserID = itm.Value;
                    entity.OperateUserID = CurrentUserInfo.UserName;
                    enList2.Add(entity);
                }
                if (UCIsSingle)//如果是单选
                {
                    lboxRight.Items.Clear();
                }
            }
            List<B_WFAgent> enList = new List<B_WFAgent>();
            for (int i = 0; i < iSelected.Length; i++)
            {
                lboxRight.Items.Add(new ListItem(OAUser.GetUserName(lboxLeft.Items[iSelected[i]].Value), lboxLeft.Items[iSelected[i]].Value));
                B_WFAgent entity = new B_WFAgent();
                entity.AgentUserID = lboxLeft.Items[iSelected[i]].Value;
                entity.OperateUserID = CurrentUserInfo.UserName;
                enList.Add(entity);
            }
            if (enList.Count > 0)
            {
                B_WFAgent bllAgent = new B_WFAgent();
                if (!bllAgent.IsAddAgentSuc(enList, enList2))
                {
                    JScript.Alert(ConstString.PromptInfo.ACTION_OPERATE_FAIL, true);
                    return;
                }
                else
                {
                    lboxLeft.ClearSelection();
                    lboxRight.ClearSelection();
                    BindLeftUser();
                    JScript.Alert("添加成功。", true);
                }
            }
        }

        /// <summary>
        /// 移除选中的人
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnDeleteOne_Click(object sender, EventArgs e)
        {
            ArrayList arrValue = new ArrayList();
            int[] iSelected = lboxRight.GetSelectedIndices();

            if (iSelected.Length == 0)
            {
                JScript.Alert("请选择人员。", true);
                return;
            }
            //else
            //{
            //    if (UCIsSingle)//如果是单选
            //    {
            //        lboxRight.Items.Clear();
            //    }
            //}
            for (int i = 0; i < iSelected.Length; i++)
            {
                arrValue.Add(lboxRight.Items[iSelected[i]].Value);
            }
            List<B_WFAgent> enList = new List<B_WFAgent>();
            for (int i = 0; i < arrValue.Count; i++)
            {
                lboxRight.Items.Remove(lboxRight.Items.FindByValue(arrValue[i].ToString()));
                B_WFAgent entity = new B_WFAgent();
                entity.OperateUserID = CurrentUserInfo.UserName;
                entity.AgentUserID = arrValue[i].ToString();
                enList.Add(entity);
            }
            if (enList.Count > 0)
            {
                B_WFAgent bllAgent = new B_WFAgent();
                if (!bllAgent.IsUpdateSuc(enList))
                {
                    JScript.Alert(ConstString.PromptInfo.ACTION_OPERATE_FAIL, true);
                    return;
                }
                if (UCIsSingle)//如果是单选
                {
                    lboxRight.Items.Clear();
                }
                BindLeftUser();
                JScript.Alert("取消成功。", true);
            }
            else
            {
                JScript.Alert("请选择人员。", true);
                return;
            }

        }

        /// <summary>
        /// 全加
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnAddAll_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < lboxLeft.Items.Count; i++)
            {
                lboxRight.Items.Add(new ListItem(OAUser.GetUserName(lboxLeft.Items[i].Value), lboxLeft.Items[i].Value));
            }
            lboxLeft.ClearSelection();
            lboxRight.ClearSelection();
            BindLeftUser();
        }

        /// <summary>
        /// 全删
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnDeleteAll_Click(object sender, EventArgs e)
        {
            lboxRight.Items.Clear();
            BindLeftUser();
        }

        /// <summary>
        /// 绑定左边列表框
        /// </summary>
        private void BindLeftUser()
        {
            //得到人员
            ViewBase vbUser = null;
            lboxLeft.Items.Clear();
            if (this.tvDB.SelectedNode != null)
            {
                vbUser = OAUser.GetUserByDeptID(this.tvDB.SelectedNode.Value, ConstString.Grade.ZERO);
            }
            if (vbUser != null)
            {
                foreach (FounderSoftware.ADIM.OU.BLL.Busi.User user in vbUser.Ens)
                {
                    string strPostName = string.Empty;
                    ViewBase vbDeptPost = user.DeptPosts;
                    if (vbDeptPost != null && vbDeptPost.DtTable != null)
                    {
                        vbDeptPost.Condition = "a.FK_DeptID=" + this.tvDB.SelectedNode.Value;
                        strPostName = vbDeptPost.DtTable.Rows[0]["PostName"] + " " + vbDeptPost.DtTable.Rows[0]["LeaderManager"];
                    }
                    string show = user.Name + "(" + user.UserID + ")" + " " + strPostName;
                    lboxLeft.Items.Add(new ListItem(show, user.DomainUserID));
                }
                for (int i = 0; i < lboxRight.Items.Count; i++)
                {
                    lboxLeft.Items.Remove(lboxLeft.Items.FindByValue(lboxRight.Items[i].Value));
                }
            }
            else
            {
                JScript.Alert("请点击部门树节点");
            }


        }

        /// <summary>
        /// 绑定到右边列表框
        /// </summary>
        private void BindRight()
        {
            if (hUCDeptUserID.Value != string.Empty)
            {
                string[] sArrUserID = hUCDeptUserID.Value.Split(';');
                foreach (string item in sArrUserID)
                {
                    this.lboxRight.Items.Add(new ListItem(OAUser.GetUserName(item), item));
                }
            }
        }

        #endregion

        #region 脚本的回传值

        /// <summary>
        /// 确定
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnOK_Click(object sender, EventArgs e)
        {
            #region 获得部门ID和Name
            string deptID = string.Empty; //部门id
            string deptName = string.Empty;

            ArrayList arrCheckedDeptID = new ArrayList();

            foreach (TreeNode node in tvDB.CheckedNodes)
            {
                arrCheckedDeptID.Add(node.Value);
            }

            if (arrCheckedDeptID.Count > 0)
            {
                foreach (object o in arrCheckedDeptID)
                {
                    if (o.ToString() != string.Empty)
                    {
                        deptID += ";" + o.ToString();
                        deptName += ";" + OADept.GetDeptName(o.ToString());
                    }
                }
                if (deptID.Length > 0)
                {
                    deptID = deptID.Substring(1);
                }
                if (deptName.Length > 0)
                {
                    deptName = deptName.Substring(1);
                }
            }
            #endregion

            #region 获得部门成员ID和Name
            string deptUserID = string.Empty;
            string deptUserName = string.Empty;//用户名字

            ArrayList arrDeptUserName = new ArrayList();
            ArrayList arrDeptUserID = new ArrayList();

            foreach (ListItem item in lboxRight.Items)
            {
                arrDeptUserID.Add(item.Value);
                arrDeptUserName.Add(OAUser.GetUserName(item.Value));
            }
            //帐号
            deptUserID = GetStringText(arrDeptUserID).Replace(@"\", @"\\");
            //姓名
            deptUserName = GetStringText(arrDeptUserName);

            #endregion

            //#region  获得角色成员ID和Name
            //string roleUserID = string.Empty;
            //string roleUserName = string.Empty;  //角色用户名
            //ArrayList arrRoleUserName = new ArrayList();
            //ArrayList arrRoleUserID = new ArrayList();
            //foreach (ListItem item in chkRole.Items)
            //{
            //    if (item.Selected)
            //    {
            //        arrRoleUserID.Add(item.Value);
            //        arrRoleUserName.Add(item.Text);
            //    }
            //}
            ////帐号
            //roleUserID = base.GetStringText(arrRoleUserID).Replace(@"\", @"\\");
            ////姓名
            //roleUserName = base.GetStringText(arrRoleUserName);

            //#endregion

            #region 获得部门树上成员ID和Name

            string deptTreeUserID = string.Empty;
            string deptTreeUserName = string.Empty;//用户名字
            string[] strUsers = new string[2];

            GetType1();

            if (UCDeptTreeUserIDControl != string.Empty || UCDeptTreeUserNameControl != string.Empty)
            {
                foreach (string deptid in arrCheckedDeptID)
                {
                    strUsers = this.GetDeptMember(deptid);
                    deptTreeUserID += ";" + strUsers[0];
                    deptTreeUserName += ";" + strUsers[1];
                }
                if (deptTreeUserName.Length > 0)
                {
                    deptTreeUserName = deptTreeUserName.Substring(1);
                }
                if (deptTreeUserID.Length > 0)
                {
                    deptTreeUserID = deptTreeUserID.Substring(1).Replace(@"\", @"\\");
                }

            }

            #endregion

            #region 获得回传脚本
            string script = string.Empty;

            ////1
            //if (UCDeptIDControl != string.Empty)
            //{
            //    script += base.GetJSscriptValue(UCDeptIDControl, deptID);
            //}
            //if (UCDeptNameControl != string.Empty)
            //{
            //    script += base.GetJSscriptValue(UCDeptNameControl, deptName);
            //    script += base.GetJSscriptTitle(UCDeptNameControl, deptID);
            //}

            //2
            if (UCDeptUserIDControl != string.Empty)
            {
                //script += base.GetJSscriptValue(UCDeptUserIDControl, deptUserID);

            }
            if (UCDeptUserNameControl != string.Empty)
            {
                //script += base.GetJSscriptValue(UCDeptUserNameControl, deptUserName);
                //script += base.GetJSscriptTitle(UCDeptUserNameControl, deptUserID);
            }

            ////3
            //if (UCRoleUserIDControl != string.Empty)
            //{
            //    script += base.GetJSscriptValue(UCRoleUserIDControl, roleUserID);
            //}
            //if (UCRoleUserNameControl != string.Empty)
            //{
            //    script += base.GetJSscriptValue(UCRoleUserNameControl, roleUserName);
            //    script += base.GetJSscriptTitle(UCRoleUserNameControl, roleUserID);
            //}
            if (UCDeptTreeUserIDControl != string.Empty)
            {
                //script += base.GetJSscriptValue(UCDeptTreeUserIDControl, deptTreeUserID);
            }
            if (UCDeptTreeUserNameControl != string.Empty)
            {
                //script += base.GetJSscriptValue(UCDeptTreeUserNameControl, deptTreeUserName);
                //script += base.GetJSscriptTitle(UCDeptTreeUserNameControl, deptTreeUserID);
            }

            //如果是部门和人员选到一个文本框上
            if (this.UCDeptAndUserControl != string.Empty)
            {
                string strDeptAndUser = string.Empty;
                if (deptName != string.Empty)
                {
                    strDeptAndUser += deptName;
                }
                if (deptUserName != string.Empty)
                {
                    if (strDeptAndUser != string.Empty)
                    {
                        strDeptAndUser += ";";
                    }
                    strDeptAndUser += deptUserName;
                }
                //script += base.GetJSscriptValue(this.UCDeptAndUserControl, strDeptAndUser);
            }

            //script += string.Format("parent.ClosePopDiv('{0}')", base.divPopDivID + base.UCID);

            #endregion

            //组成一整条js后运行
            //ClientScriptM.ResponseScript(this, script);
            JScript.Alert(deptUserID + "," + deptUserName, true);
        }

        #endregion

        #region 脚本控件刷新

        /// <summary>
        /// 刷新
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSX_Click(object sender, EventArgs e)
        {
            Refresh();
        }

        private void Refresh()
        {
            #region 页面显示控制
            //IsFirstBind = true;

            //全选
            if (this.UCAllSelect)
            {
                this.tvDB.Attributes.Add("onclick", "javascript:postBackByObject()");
            }

            //如果只选部门
            if (UCSelectType == "0")
            {
                //如果是单选
                if (UCIsSingle)
                {
                    tvDB.Attributes.Add("onclick", "javascript:onPost()");
                }
                PanelUser.Visible = false;
            }
            //如果需要选人
            if (UCSelectType == "1")
            {
                tvDB.ShowCheckBoxes = TreeNodeTypes.None;

                //如果是单选
                if (UCIsSingle)
                {
                    btnAddAll.Visible = false;
                    btnDeleteAll.Visible = false;
                }
            }

            //if (UCRole == string.Empty)
            //{
            //    PanelRole.Visible = false;
            //}
            //else
            //{
            //    //绑定其他角色成员 适应一些特殊需求
            //    if (UCRole != string.Empty)
            //    {
            //        drpRole.Items.Clear();
            //        //drpRole.SelectedIndex = drpRole.Items.IndexOf(drpRole.Items.FindByValue(this.UCRole));
            //        this.drpRole.Items.Add(new ListItem(this.UCRole, this.UCRole));
            //        drpRole.Enabled = false;
            //        drpRole_SelectedIndexChanged(null, null);
            //    }
            //}


            #endregion

            BindDBTree();

            tvDB.ExpandAll();

            BindRight();
        }

        #endregion

        #region 事件

        /// <summary>
        /// 树节点改变事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void tvDB_TreeNodeCheckChanged(object sender, TreeNodeEventArgs e)
        {
            if (UCIsSingle)
            {
                if (tvDB.CheckedNodes.Count > 1)
                {
                    for (int i = 0; i < tvDB.CheckedNodes.Count; i++)
                    {
                        TreeNode tntemp = tvDB.CheckedNodes[i];
                        if (tntemp.Value == SelectedID)
                        {
                            tntemp.Checked = false;
                        }
                    }
                }
                if (tvDB.CheckedNodes.Count != 0)
                {
                    TreeView tv = (TreeView)sender;
                    TreeNode tn = tvDB.CheckedNodes[0];
                    SelectedID = tn.Value;
                }
            }
        }

        #endregion

        //从ArrList里组成分号分隔的字符串
        private string GetStringText(ArrayList arrList)
        {
            string str = "";
            foreach (object o in arrList)
            {
                if (o.ToString() != string.Empty)
                    str += ";" + o.ToString();
            }
            if (str.Length > 0)
            {
                str = str.Substring(1);
            }
            return str;
        }
    }
}