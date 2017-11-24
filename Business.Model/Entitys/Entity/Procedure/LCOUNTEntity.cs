using Manage.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Business.Model.Entitys.Entity{
	 	//T_LEGALCASE
		 [Serializable]
    public class LCOUNTEntity : AggregateRoot
	{
       
		/// <summary>
		/// 车牌
        /// </summary>
        public virtual string PLATENUMBER
        {
            get; 
            set; 
        }        	
        public virtual int PCOUNT
        {
            get; 
            set; 
        }        
		   
	}
}