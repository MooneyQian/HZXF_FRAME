using Manage.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Business.Model.Models
{
    /// <summary>
    /// 
    /// <summary>
    [Serializable]
    public class SysUser_S : BaseModel
    {
        /// <summary>
        /// 登录名
        /// </summary>		
        private string _userloginname;
        public string UserLoginName
        {
            get { return _userloginname; }
            set { _userloginname = value; }
        }
        /// <summary>
        /// 显示名
        /// </summary>		
        private string _userdisplayname;
        public string UserDisplayName
        {
            get { return _userdisplayname; }
            set { _userdisplayname = value; }
        }
        /// <summary>
        /// 登录密码
        /// </summary>		
        private string _userpassword;
        public string UserPassword
        {
            get { return _userpassword; }
            set { _userpassword = value; }
        }
        /// <summary>
        /// 用户类型
        /// </summary>		
        private int _usertype;
        public int UserType
        {
            get { return _usertype; }
            set { _usertype = value; }
        }
        /// <summary>
        /// 用户电话
        /// </summary>		
        private string _userphone;
        public string UserPhone
        {
            get { return _userphone; }
            set { _userphone = value; }
        }
        /// <summary>
        /// 启用状态
        /// </summary>		
        private int _recordstatus;
        public int RecordStatus
        {
            get { return _recordstatus; }
            set { _recordstatus = value; }
        }
        /// <summary>
        /// Extend1
        /// </summary>		
        private string _extend1;
        public string Extend1
        {
            get { return _extend1; }
            set { _extend1 = value; }
        }
        /// <summary>
        /// Extend2
        /// </summary>		
        private string _extend2;
        public string Extend2
        {
            get { return _extend2; }
            set { _extend2 = value; }
        }
        /// <summary>
        /// Extend3
        /// </summary>		
        private string _extend3;
        public string Extend3
        {
            get { return _extend3; }
            set { _extend3 = value; }
        }
        /// <summary>
        /// Extend4
        /// </summary>		
        private string _extend4;
        public string Extend4
        {
            get { return _extend4; }
            set { _extend4 = value; }
        }
        /// <summary>
        /// Extend5
        /// </summary>		
        private string _extend5;
        public string Extend5
        {
            get { return _extend5; }
            set { _extend5 = value; }
        }

        #region

        #endregion
    }
}