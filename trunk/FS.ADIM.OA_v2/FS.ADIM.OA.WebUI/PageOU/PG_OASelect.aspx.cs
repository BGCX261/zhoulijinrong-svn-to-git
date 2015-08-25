using System;
using System.Collections;
using System.Data;
using System.Web.UI.WebControls;
using FounderSoftware.Framework.Business;
using FS.ADIM.OA.BLL.Common;
using FS.ADIM.OA.BLL.Common.Utility;
using FS.ADIM.OA.BLL.SystemM;
using FS.ADIM.OU.OutBLL;
using System.Xml;
using FS.OA.Framework.Caching;

namespace FS.ADIM.OA.WebUI.PageOU
{
    public partial class PG_OASelect : OAPGBase
    {
        //支持回传部门ID 部门名,人员帐号 人员名

        #region 回传变量

        /// <summary>
        /// 部门ID控件
        /// </summary>
        protected String UCDeptIDControl
        {
            get
            {
                return base.GetQueryString("UCDeptIDControl");
            }
        }

        /// <summary>
        /// 部门Names控件 
        /// </summary>
        protected String UCDeptNameControl
        {
            get
            {
                return base.GetQueryString("UCDeptNameControl");
            }
        }

        /// <summary>
        /// 部门的用户ID控件
        /// </summary>
        protected String UCDeptUserIDControl
        {
            get
            {
                return base.GetQueryString("UCDeptUserIDControl");
            }
        }

        /// <summary>
        /// 部门的用户Names控件 
        /// </summary>
        protected String UCDeptUserNameControl
        {
            get
            {
                return base.GetQueryString("UCDeptUserNameControl");
            }
        }

        /// <summary>
        /// 部门和人员名都显示在一个文本框内
        /// </summary>
        protected String UCDeptAndUserControl
        {
            get
            {
                return base.GetQueryString("UCDeptAndUserControl");
            }
        }

        /// <summary>
        /// 部门树上的用户ID控件
        /// </summary>
        protected String UCDeptTreeUserIDControl
        {
            get
            {
                return base.GetQueryString("UCDeptTreeUserIDControl");
            }
        }

        /// <summary>
        /// 部门树上的用户Names控件 回传
        /// </summary>
        protected String UCDeptTreeUserNameControl
        {
            get
            {
                return base.GetQueryString("UCDeptTreeUserNameControl");
            }
        }

        /// <summary>
        /// 角色用户ID控件
        /// </summary>
        protected String UCRoleUserIDControl
        {
            get
            {
                return base.GetQueryString("UCRoleUserIDControl");
            }
        }

        /// <summary>
        /// 角色用户Names控件 回传
        /// </summary>
        protected String UCRoleUserNameControl
        {
            get
            {
                return base.GetQueryString("UCRoleUserNameControl");
            }
        }

        #endregion

        #region 辅助功能

        /// <summary>
        /// 选择类型 0 1 2 ，0是部门 1是人,2是2者都是
        /// </summary>
        protected String UCSelectType
        {
            get
            {
                return base.GetQueryString("UCSelectType");
            }
        }

        /// <summary>
        /// 流程模板的名称
        /// </summary>
        protected String UCTemplateName
        {
            get
            {
                return base.GetQueryString("UCTemplateName");
            }
        }

        /// <summary>
        /// 是否单选
        /// </summary>
        protected Boolean UCIsSingle
        {
            get
            {
                return base.GetQueryBoolean("UCIsSingle");
            }
        }

        /// <summary>
        /// 显示可用的部门
        /// </summary>
        protected String UCShowDeptID
        {
            get
            {
                return base.GetQueryString("UCShowDeptID");
            }
        }

        /// <summary>
        /// 流程表单的名称
        /// </summary>
        protected String UCFormName
        {
            get
            {
                return base.GetQueryString("UCFormName");
            }
        }

        /// <summary>
        /// 显示可选择的层级
        /// </summary>
        protected String UCLevel
        {
            get
            {
                return base.GetQueryString("UCLevel");
            }
        }

        /// <summary>
        /// 其他角色
        /// </summary>
        protected String UCRole
        {
            get
            {
                return base.GetQueryString("UCRole");
            }
        }

        /// <summary>
        /// 部门上显示哪一类人 1111 1负责人 2职位大于副处 3部门领导 4待定
        /// </summary>
        protected String UCDeptShowType
        {
            get
            {
                return base.GetQueryString("UCDeptShowType");
            }
        }

        /// <summary>
        /// 控制父节点选中子节点是否也选中（1.全选 0.否）
        /// </summary>
        protected Boolean UCAllSelect
        {
            get
            {
                return base.GetQueryBoolean("UCAllSelect");
            }

        }

        /// <summary>
        /// 控制CheckBox是否全选（默认不全选）
        /// </summary>
        protected Boolean UCALLChecked
        {
            get
            {
                return base.GetQueryBoolean("UCALLChecked");
            }
        }


        String type1 = String.Empty;
        String type2 = String.Empty;
        String type3 = String.Empty;
        String type4 = String.Empty;

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

            //如果需要选人
            if (UCSelectType == "1" || UCSelectType == "2")
            {
                this.tvDeptList.SelectedNodeChanged += new EventHandler(tvDeptList_SelectedNodeChanged);
            }

            if (UCIsSingle || UCAllSelect)
            {
                this.lbxLeft.SelectionMode = ListSelectionMode.Single;
                this.tvDeptList.TreeNodeCheckChanged += new TreeNodeEventHandler(tvDeptList_TreeNodeCheckChanged);
                this.tvDeptList_TreeNodeCheckChanged(null, null);
            }
            if (string.IsNullOrEmpty(this.UCTemplateName) == false && string.IsNullOrEmpty(this.UCFormName) == false)
            {
                this.tvRoleList.SelectedNodeChanged += new EventHandler(tvRoleList_SelectedNodeChanged);
            }

            #endregion

            if (!IsPostBack)
            {
                //如果只选部门
                if (UCSelectType == "0")
                {
                    //如果是单选
                    if (UCIsSingle)
                    {
                        this.tvDeptList.Attributes.Add("onclick", "javascript:onPost()");
                        //tvDeptList.Width = new Unit(450);
                    }
                    divDeptList.Style.Add("width", "400px");
                    this.PanelUser.Visible = false;
                }
                //如果需要选人
                if (UCSelectType == "1")
                {
                    this.tvDeptList.ShowCheckBoxes = TreeNodeTypes.None;

                    //如果是单选
                    if (UCIsSingle)
                    {
                        this.btnAddAll.Visible = false;
                        this.btnDeleteAll.Visible = false;
                    }
                }
                ClientScriptM.ResponseScript(Page, "GetParent();");
                if (UCRole == String.Empty)
                {
                    this.PanelRole.Visible = false;
                }


            }
        }

        #endregion

        #region 绑定树

        private DataTable m_dtbDepartment = null;
        private ArrayList hasDeptID = new ArrayList();
        private DataTable dtDeptMember = new DataTable();//部门成员 
        private Boolean m_blnIsManager = false;
        private Boolean m_blnIsLeader = false;
        private String strPName = String.Empty;
        private int iLevel = -1;

        /// <summary>
        /// 绑定树
        /// </summary>
        private void LoadDeptTree()
        {
            if (UCLevel != String.Empty)
            {
                iLevel = SysConvert.ToInt32(UCLevel);
            }
            hasDeptID = SysString.GetStringToArrayList(UCShowDeptID, ',');
            m_dtbDepartment = OADept.GetDeptInfo(iLevel).DtTable;

            this.tvDeptList.Nodes.Clear();

            DataRow[] l_dtrTopRows = m_dtbDepartment.Select("FloorCode=0");//最顶层

            foreach (DataRow l_dtrDataRow in l_dtrTopRows)
            {
                TreeNode l_objNode = new TreeNode();
                l_objNode = this.SetTreeNode(l_objNode, l_dtrDataRow);

                this.tvDeptList.Nodes.Add(l_objNode);

                DataRow[] l_dtrChildRows = m_dtbDepartment.Select("ParentID=" + l_dtrDataRow["ID"].ToString());
                if (l_dtrChildRows.Length > 0)
                {
                    this.LoadSubDeptTree(l_objNode, l_dtrChildRows);
                }
            }
        }

        /// <summary>
        /// 绑定角色组树
        /// </summary>
        private void LoadRoleTree(string strTemplateName, string strFormName)
        {

            DataCache cache = new DataCache();
            cache.CacheName = "SelectGroup";
            cache.CacheItemName = "SelectGroupItem";
            XmlDocument doc = cache.GetCache() as XmlDocument;
            if (doc == null)
            {
                doc = new XmlDocument();
                string strPath = AppDomain.CurrentDomain.BaseDirectory + @"Config\SelectGroup.xml";
                if (string.IsNullOrEmpty(strPath) == false)
                {
                    doc.Load(strPath);
                    cache.ExpireTime = 1440;
                    cache.SetCache(doc);
                }
            }

            XmlNode xmlRootNode = doc.SelectSingleNode("*");
            TreeNode rootNode = new TreeNode();
            if (xmlRootNode != null)
            {
                //添加根节点
                rootNode.Text = xmlRootNode.Name;
                this.tvRoleList.Nodes.Add(rootNode);
            }

            string strXpath = "*/" + strTemplateName + "/" + strFormName;
            XmlNode xmlNewNode = doc.SelectSingleNode(strXpath);

            if (xmlNewNode != null && string.IsNullOrEmpty(xmlNewNode.InnerText) == false)
            {
                foreach (string item in xmlNewNode.InnerText.Split(';'))
                {
                    rootNode.ChildNodes.Add(new TreeNode(item));
                }
                this.tvRoleList.Visible = true;
                string strAttibutes = xmlNewNode.Attributes["isCheck"].Value;
                if (string.IsNullOrEmpty(strAttibutes) == false && strAttibutes.ToLower() == "true")
                {
                    this.tvRoleList.ShowCheckBoxes = TreeNodeTypes.Leaf;
                }
            }
        }

        /// <summary>
        /// 绑定子部门
        /// </summary>
        /// <param name="node"></param>
        /// <param name="drs"></param>
        private void LoadSubDeptTree(TreeNode p_objParentNode, DataRow[] p_dtrDataRows)
        {
            foreach (DataRow l_dtrDataRow in p_dtrDataRows)
            {
                TreeNode l_objNode = new TreeNode();
                l_objNode = this.SetTreeNode(l_objNode, l_dtrDataRow);

                //回传绑定
                if (base.IsChecked(hUCDeptID.Value, l_dtrDataRow["ID"].ToString()))
                {
                    l_objNode.Checked = true;
                }
                p_objParentNode.ChildNodes.Add(l_objNode);

                DataRow[] l_dtrChildRows = m_dtbDepartment.Select("ParentID=" + l_dtrDataRow["ID"].ToString());
                if (l_dtrChildRows.Length > 0)
                {
                    this.LoadSubDeptTree(l_objNode, l_dtrChildRows);
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
            ViewBase vbDeptUser = OAUser.GetUserByDeptID(dr["ID"].ToString(), ConstString.Grade.ALL);
            String name = dr["Name"].ToString();
            if (vbDeptUser != null)
            {
                name += "[" + vbDeptUser.Count.ToString() + "]";
            }

            if (UCDeptShowType != String.Empty)
            {
                String member = GetDeptMember(dr["ID"].ToString())[1];
                if (String.IsNullOrEmpty(member) == false && member.Length > 0)
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

                if (UCALLChecked == true)
                {
                    newNode.ShowCheckBox = true;
                }
                else
                {
                    if (String.IsNullOrEmpty(member))
                    {
                        newNode.ShowCheckBox = false; //没有成员 不可选择
                    }
                    else
                    {
                        newNode.ShowCheckBox = true;
                    }
                }

            }
            else
            {
                newNode = new TreeNode(name, dr["ID"].ToString());
            }
            if (UCShowDeptID != String.Empty)
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
            if (UCDeptShowType != String.Empty)
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
        private String[] GetDeptMember(String strDeptID)
        {
            GetType1();
            if (type1 == "1") //负责人
            {
                m_blnIsManager = true;
            }
            if (type2 == "1") //职位大于副处长
            {
                strPName = OUConstString.PostName.FUCHUZHANG;
            }
            if (type3 == "1") //部门领导
            {
                m_blnIsLeader = true;
            }
            return OAUser.GetUserByDeptPostArray(strDeptID, strPName, m_blnIsManager, m_blnIsLeader);
        }

        #endregion

        #region 部门树选中节点后筛选人员

        protected void tvDeptList_SelectedNodeChanged(object sender, EventArgs e)
        {
            this.LoadDeptUser();
        }

        protected void tvRoleList_SelectedNodeChanged(object sender, EventArgs e)
        {
            this.LoadRoleUser();
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
            if (UCIsSingle)//如果是单选
            {
                this.lbxRight.Items.Clear();
            }
            int[] iSelected = this.lbxLeft.GetSelectedIndices();
            for (int i = 0; i < iSelected.Length; i++)
            {
                this.lbxRight.Items.Add(new ListItem(OAUser.GetUserName(this.lbxLeft.Items[iSelected[i]].Value), this.lbxLeft.Items[iSelected[i]].Value));
            }
            this.lbxLeft.ClearSelection();
            this.lbxRight.ClearSelection();
            this.LoadDeptUser();
            this.LoadRoleUser();
        }

        /// <summary>
        /// 移除选中的人
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnDeleteOne_Click(object sender, EventArgs e)
        {
            if (UCIsSingle)//如果是单选
            {
                this.lbxRight.Items.Clear();
            }
            ArrayList arrValue = new ArrayList();
            int[] iSelected = this.lbxRight.GetSelectedIndices();
            for (int i = 0; i < iSelected.Length; i++)
            {
                arrValue.Add(this.lbxRight.Items[iSelected[i]].Value);
            }
            for (int i = 0; i < arrValue.Count; i++)
            {
                this.lbxRight.Items.Remove(this.lbxRight.Items.FindByValue(arrValue[i].ToString()));
            }
            this.LoadDeptUser();
            this.LoadRoleUser();
        }

        /// <summary>
        /// 全加
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnAddAll_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < lbxLeft.Items.Count; i++)
            {
                this.lbxRight.Items.Add(new ListItem(OAUser.GetUserName(this.lbxLeft.Items[i].Value), this.lbxLeft.Items[i].Value));
            }
            this.lbxLeft.ClearSelection();
            this.lbxRight.ClearSelection();
            this.LoadDeptUser();
            this.LoadRoleUser();
        }

        /// <summary>
        /// 全删
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnDeleteAll_Click(object sender, EventArgs e)
        {
            this.lbxRight.Items.Clear();
            this.LoadDeptUser();
            this.LoadRoleUser();
        }

        /// <summary>
        /// 绑定左边列表框
        /// </summary>
        private void LoadDeptUser()
        {
            //得到人员
            ViewBase vbUser = null;
            if (this.tvDeptList.SelectedNode != null)
            {
                this.lbxLeft.Items.Clear();
                vbUser = OAUser.GetUserByDeptID(this.tvDeptList.SelectedNode.Value, ConstString.Grade.ALL);
                if (vbUser != null && vbUser.DtTable != null)
                {
                    foreach (FounderSoftware.ADIM.OU.BLL.Busi.User user in vbUser.Ens)
                    {
                        String strPostName = String.Empty;
                        ViewBase vbDeptPost = user.DeptPosts;
                        if (vbDeptPost != null && vbDeptPost.DtTable != null)
                        {
                            String strpost = String.Empty;
                            vbDeptPost.Condition = "a.FK_DeptID=" + this.tvDeptList.SelectedNode.Value;
                            foreach (DataRow dr in vbDeptPost.DtTable.Rows)
                            {
                                strpost += dr["PostName"] + " ";
                            }
                            if (vbDeptPost.DtTable.Rows.Count > 0)
                            {
                                strPostName = strpost + " " + vbDeptPost.DtTable.Rows[0]["LeaderManager"];
                            }
                        }
                        String show = user.Name + "(" + user.UserID + ")" + " " + strPostName;
                        ListItem item = new ListItem(show, user.DomainUserID);
                        if (this.lbxLeft.Items.Contains(item) == false)
                        {
                            this.lbxLeft.Items.Add(item);
                        }
                    }
                    for (int i = 0; i < this.lbxRight.Items.Count; i++)
                    {
                        this.lbxLeft.Items.Remove(this.lbxLeft.Items.FindByValue(this.lbxRight.Items[i].Value));
                    }
                }
            }
        }

        /// <summary>
        /// 加载角色成员
        /// </summary>
        private void LoadRoleUser()
        {
            ViewBase vbRole = null;

            if (this.tvRoleList.SelectedNode != null)
            {
                this.lbxLeft.Items.Clear();
                string strShow = string.Empty;
                vbRole = OAUser.GetUserByRole(this.tvRoleList.SelectedNode.Text);
                if (vbRole != null)
                {
                    foreach (FounderSoftware.ADIM.OU.BLL.Busi.User user in vbRole.Ens)
                    {
                        strShow = user.Name + "(" + user.UserID + ")";
                        ListItem listItem = new ListItem(strShow, user.DomainUserID);
                        if (this.lbxLeft.Items.Contains(listItem) == false)
                        {
                            this.lbxLeft.Items.Add(listItem);
                        }
                    }
                }
                for (int i = 0; i < this.lbxRight.Items.Count; i++)
                {
                    this.lbxLeft.Items.Remove(this.lbxLeft.Items.FindByValue(this.lbxRight.Items[i].Value));
                }

            }
        }

        /// <summary>
        /// 绑定到右边列表框
        /// </summary>
        private void BindRight()
        {
            if (hUCDeptUserID.Value != String.Empty)
            {
                String[] sArrUserID = hUCDeptUserID.Value.Split(';');
                foreach (String item in sArrUserID)
                {
                    this.lbxRight.Items.Add(new ListItem(OAUser.GetUserName(item), item));
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
            String deptID = String.Empty;
            String deptName = String.Empty;

            ArrayList arrCheckedDeptID = new ArrayList();

            foreach (TreeNode node in this.tvDeptList.CheckedNodes)
            {
                arrCheckedDeptID.Add(node.Value);
            }

            if (arrCheckedDeptID.Count > 0)
            {
                foreach (object o in arrCheckedDeptID)
                {
                    if (o.ToString() != String.Empty)
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
            String deptUserID = String.Empty;
            String deptUserName = String.Empty;

            ArrayList arrDeptUserName = new ArrayList();
            ArrayList arrDeptUserID = new ArrayList();

            foreach (ListItem item in lbxRight.Items)
            {
                arrDeptUserID.Add(item.Value);
                arrDeptUserName.Add(OAUser.GetUserName(item.Value));
            }
            //帐号
            deptUserID = base.GetStringText(arrDeptUserID).Replace(@"\", @"\\");
            //姓名
            deptUserName = base.GetStringText(arrDeptUserName);

            #endregion

            #region  获得角色成员ID和Name
            String roleUserID = String.Empty;
            String roleUserName = String.Empty;
            ArrayList arrRoleUserName = new ArrayList();
            ArrayList arrRoleUserID = new ArrayList();
            foreach (ListItem item in chkRole.Items)
            {
                if (item.Selected)
                {
                    arrRoleUserID.Add(item.Value);
                    arrRoleUserName.Add(item.Text);
                }
            }

            //帐号
            roleUserID = base.GetStringText(arrRoleUserID).Replace(@"\", @"\\");
            //姓名
            roleUserName = base.GetStringText(arrRoleUserName);

            #endregion

            #region 获得部门树上成员ID和Name

            String deptTreeUserID = String.Empty;
            String deptTreeUserName = String.Empty;//用户名字
            String[] strUsers = new String[2];

            GetType1();

            if (UCDeptTreeUserIDControl != String.Empty || UCDeptTreeUserNameControl != String.Empty)
            {
                if (this.type1 == "1" || this.type2 == "1" || this.type3 == "1")
                {
                    foreach (String deptid in arrCheckedDeptID)
                    {
                        strUsers = this.GetDeptMember(deptid);
                        if (strUsers[0] != null && strUsers[1] != null)
                        {
                            deptTreeUserID += ";" + strUsers[0];
                            deptTreeUserName += ";" + strUsers[1];
                        }
                    }
                    if (deptTreeUserName.Length > 0)
                    {
                        deptTreeUserName = FormsMethod.FilterRepeat(deptTreeUserName.Substring(1));
                    }
                    if (deptTreeUserID.Length > 0)
                    {
                        deptTreeUserID = FormsMethod.FilterRepeat(deptTreeUserID.Substring(1)).Replace(@"\", @"\\");
                    }
                }
            }

            #endregion

            #region 获得角色组的名称

            string strGroupName = string.Empty;

            foreach (TreeNode node in this.tvRoleList.CheckedNodes)
            {
                strGroupName += ";" + node.Text;
            }
            if (strGroupName.Length > 0)
            {
                strGroupName = strGroupName.Substring(1);
            }

            #endregion

            #region 获得回传脚本
            String script = String.Empty;

            if (UCDeptIDControl != String.Empty)
            {
                script += base.GetJSscriptValue(UCDeptIDControl, deptID);
            }
            if (UCDeptNameControl != String.Empty)
            {
                script += base.GetJSscriptValue(UCDeptNameControl, deptName);
                script += base.GetJSscriptTitle(UCDeptNameControl, deptID);
            }
            if (UCDeptUserIDControl != String.Empty)
            {
                script += base.GetJSscriptValue(UCDeptUserIDControl, deptUserID);
            }
            if (UCDeptUserNameControl != String.Empty)
            {
                script += base.GetJSscriptValue(UCDeptUserNameControl, deptUserName);
                script += base.GetJSscriptTitle(UCDeptUserNameControl, deptUserID);
            }
            if (UCRoleUserIDControl != String.Empty)
            {
                script += base.GetJSscriptValue(UCRoleUserIDControl, roleUserID);
            }
            if (UCRoleUserNameControl != String.Empty)
            {
                script += base.GetJSscriptValue(UCRoleUserNameControl, roleUserName);
                script += base.GetJSscriptTitle(UCRoleUserNameControl, roleUserID);
            }
            if (UCDeptTreeUserIDControl != String.Empty)
            {
                script += base.GetJSscriptValue(UCDeptTreeUserIDControl, deptTreeUserID);
            }
            if (UCDeptTreeUserNameControl != String.Empty)
            {
                script += base.GetJSscriptValue(UCDeptTreeUserNameControl, deptTreeUserName);
                script += base.GetJSscriptTitle(UCDeptTreeUserNameControl, deptTreeUserID);
            }

            //如果是部门和人员选到一个文本框上
            if (this.UCDeptAndUserControl != String.Empty)
            {
                String strDeptAndUser = String.Empty;
                if (deptName != String.Empty)
                {
                    strDeptAndUser += deptName;
                }
                if (deptUserName != String.Empty)
                {
                    if (strDeptAndUser != String.Empty)
                    {
                        strDeptAndUser += ";";
                    }
                    strDeptAndUser += deptUserName;
                }
                if (strGroupName != string.Empty)
                {
                    if (strDeptAndUser != string.Empty)
                    {
                        strDeptAndUser += ";";
                    }
                    strDeptAndUser += strGroupName;
                }
                script += base.GetJSscriptValue(UCDeptAndUserControl, strDeptAndUser);
            }

            script += String.Format("parent.ClosePopDiv('{0}')", base.divPopDivID + base.UCID);

            #endregion

            //组成一整条js后运行
            ClientScriptM.ResponseScript(this, script);
        }

        #endregion

        #region 脚本控件刷新

        /// <summary>
        /// 取消
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSX_Click(object sender, EventArgs e)
        {
            base.IsFirstBind = true;

            #region 页面显示控制
            //全选
            if (this.UCAllSelect)
            {
                this.tvDeptList.Attributes.Add("onclick", "javascript:postBackByObject()");
            }

            if (UCRole == String.Empty)
            {
                this.PanelRole.Visible = false;
            }
            else
            {
                this.drpRole.Items.Clear();
                this.drpRole.Items.Add(new ListItem(this.UCRole, this.UCRole));
                this.drpRole.Enabled = false;
                this.drpRole_SelectedIndexChanged(null, null);
            }
            #endregion

            this.LoadDeptTree();
            this.tvDeptList.ExpandAll();
            this.BindRight();
            if (string.IsNullOrEmpty(this.UCTemplateName) == false && string.IsNullOrEmpty(this.UCFormName) == false)
            {
                this.LoadRoleTree(this.UCTemplateName, this.UCFormName);
            }
        }
        #endregion

        #region 事件

        /// <summary>
        /// 树节点改变事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void tvDeptList_TreeNodeCheckChanged(object sender, TreeNodeEventArgs e)
        {
            if (UCIsSingle)
            {
                if (this.tvDeptList.CheckedNodes.Count > 1)
                {
                    for (int i = 0; i < this.tvDeptList.CheckedNodes.Count; i++)
                    {
                        TreeNode tntemp = this.tvDeptList.CheckedNodes[i];
                        if (tntemp.Value == SelectedID)
                        {
                            tntemp.Checked = false;
                        }
                    }
                }
                if (this.tvDeptList.CheckedNodes.Count != 0)
                {
                    TreeView tv = (TreeView)sender;
                    TreeNode tn = this.tvDeptList.CheckedNodes[0];
                    SelectedID = tn.Value;
                }
            }
        }

        /// <summary>
        /// 下拉菜单
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void drpRole_SelectedIndexChanged(object sender, EventArgs e)
        {
            ViewBase vb = OAUser.GetUserByRole(this.UCRole);
            this.chkRole.Items.Clear();
            if (vb.Count > 0)
            {
                foreach (FounderSoftware.ADIM.OU.BLL.Busi.User user in vb.Ens)
                {
                    this.chkRole.Items.Add(new ListItem(user.Name, user.DomainUserID));
                }
            }
            if (hUCRoleUserID.Value != String.Empty)
            {
                String[] sArrUserID = hUCRoleUserID.Value.Split(';');
                for (int i = 0; i < this.chkRole.Items.Count; i++)
                {
                    for (int k = 0; k < sArrUserID.Length; k++)
                    {
                        if (chkRole.Items[i].Value == sArrUserID[0])
                        {
                            this.chkRole.Items[i].Selected = true;
                        }
                    }
                }
            }
        }

        #endregion


    }
}
