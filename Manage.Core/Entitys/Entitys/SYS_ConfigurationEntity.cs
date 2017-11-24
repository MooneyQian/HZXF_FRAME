using Manage.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Manage.Core.Entitys
{
	 	//SYS_Configuration
		 
    public class SYS_ConfigurationEntity : AggregateRoot
	{
        ///// <summary>
        ///// ID
        ///// </summary>
        public virtual string ID
        {
            get;
            set;
        }        
		/// <summary>
		/// USERID
        /// </summary>
        public virtual string USERID
        {
            get; 
            set; 
        }        
		/// <summary>
		/// SETTEXT
        /// </summary>
        public virtual string SETTEXT
        {
            get; 
            set; 
        }        
		   
	}
}