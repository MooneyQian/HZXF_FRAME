using Manage.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Manage.Core.Facades
{
    public interface IAdminFacade
    {
        /// <summary>
        /// 修改用户密码
        /// </summary>
        /// <param name="UserID"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        bool ChangePwd(string UserID, string password);
        /// <summary>
        /// 对比密码是否正确
        /// </summary>
        /// <param name="UserID"></param>
        /// <param name="Password"></param>
        /// <returns></returns>
        bool ComparePassword(string UserID, string Password);
    }
}
