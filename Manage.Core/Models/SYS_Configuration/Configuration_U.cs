using Manage.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
namespace Manage.Core.Models
{
	 	//SYS_Configuration
		 [Serializable]
    public class Configuration_U : BaseModel
	{
   		             
		/// <summary>
		/// USERID
        /// </summary>		
		private string _userid;
        public string USERID
        {
            get{ return _userid; }
            set{ _userid = value; }
        }        
		/// <summary>
		/// SETTEXT
        /// </summary>		
		private string _settext;
        public string SETTEXT
        {
            get{ return _settext; }
            set{ _settext = value; }
        }        
		   
	}
}