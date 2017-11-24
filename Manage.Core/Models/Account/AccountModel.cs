using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Manage.Core.Models
{
    [Serializable]
    public class AccountModel
    {
        public string UserID { get; set; }
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
        public string ConfirmPassword { get; set; }
    }
}
