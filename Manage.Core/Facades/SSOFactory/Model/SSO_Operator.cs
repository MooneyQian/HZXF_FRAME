using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Manage.Core.Facades.SSOFactory
{
    /// <summary>
    /// 验证中心用户实体
    /// </summary>
    [Serializable]
    public class SSO_Operator
    {
        /// <summary>
        /// 用户ID
        /// </summary>
        public decimal? USER_ID { get; set; }

        /// <summary>
        /// 登录名
        /// </summary>
        public string VC_OP_NAME { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        public string VC_OP_PASSWORD { get; set; }

        /// <summary>
        /// 用户角色(角色ID)
        /// </summary>
        public string VC_OP_ROLES { get; set; }

        /// <summary>
        /// 用户状态(1激活 0注销)
        /// </summary>
        public char C_OP_STATUS { get; set; }

        /// <summary>
        /// 用户所在地区
        /// </summary>
        public string VC_OP_AREA { get; set; }

        /// <summary>
        /// 用户名称
        /// </summary>
        public string VC_COMPANY_NAME { get; set; }

        /// <summary>
        /// 用户地址地址
        /// </summary>
        public string VC_COMPANY_ADDR { get; set; }

        /// <summary>
        /// EMAIL
        /// </summary>
        public string VC_EMAIL { get; set; }

        /// <summary>
        /// 联系人
        /// </summary>
        public string VC_CONTACT_PERSON { get; set; }

        /// <summary>
        /// 电话
        /// </summary>
        public string VC_TEL { get; set; }

        /// <summary>
        /// 传真
        /// </summary>
        public string VC_FAX { get; set; }

        /// <summary>
        /// 注册时间
        /// </summary>
        public decimal? N_REGISTE_TIME { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string VC_REMARK { get; set; }

        /// <summary>
        /// 手机
        /// </summary>
        public string VC_MOBILE { get; set; }
    }
}
