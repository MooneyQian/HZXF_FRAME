using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using Manage.Framework;

namespace Manage.Framework
{
    /// <summary>
    /// 代表的聚合根的基类。
    /// </summary>
    [Serializable]
    [DataContract]
    public abstract class AggregateRoot : IAggregateRoot
    {
        //public byte[] _MongoId;
        public AggregateRoot() { }

        #region Public Methods
        /// <summary>
        /// 获取聚合根HashCode,表示直接根据ID获取HashCode
        /// </summary>
        /// <returns>根据ID获取HashCode的值</returns>
        public override int GetHashCode()
        {
            return this.ID.GetHashCode();
        }
       /// <summary>
        /// 相等比较 
       /// </summary>
       /// <param name="obj">待比较对象</param>
       /// <returns>true:俩个对象相等;false:俩个对象不相等</returns>
        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;
            AggregateRoot ar = obj as AggregateRoot;
            if ((object)ar == null)
                return false;
            return this.Equals((IAggregateRoot)ar);

        }
        #endregion

        #region IEntity Members
        /// <summary>
        /// 获取或设置集合的根的标识符。
        /// </summary>
        [DataMember]
        public string ID { get; set; }
        /// <summary>
        /// 获取或设置云存储标识符。
        /// </summary>
        [DataMember]
        public virtual string GlobalID { get; set; }

        /// <summary>
        /// 操作类型
        /// </summary>
        public virtual OperationType OperationType { get; set; }


        /// <summary>
        /// Sql脚本代码块
        /// </summary>
        public virtual Dictionary<string, Dictionary<string, object>> SqlBlocks { get; set; }
        #endregion


    }
}
