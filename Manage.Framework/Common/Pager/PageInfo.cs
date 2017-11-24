using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Collections.Specialized;
using System.Linq.Expressions;
using System.Reflection;

namespace Manage.Framework
{
    /// <summary>
    /// 分页器
    /// </summary>
    /// <remarks></remarks>
    [DataContract]
    public class PageInfo
    {

        private int _total = 0;

        /// <summary>
        /// 页面尺寸
        /// </summary>
        public int PageSize { get; set; }

        
        private int _PageIndex;
        /// <summary>
        /// 页码
        /// </summary>
        public int PageIndex { get { return _PageIndex; } set { _PageIndex = value; } }
        public int Page { get { return _PageIndex; } set { _PageIndex = value; } }

        /// <summary>
        /// 总页面数
        /// </summary>
        public int PageCount { get; set; }

        /// <summary>
        /// 总记录数
        /// </summary>
        public int Total
        {
            get
            {
                return _total;
            }
            set
            {
                _total = value;
                if (PageSize == 0) throw new OverflowException("页码PageSize不能为0！");
                if (_total % PageSize == 0)
                    PageCount = _total / PageSize;
                else
                    PageCount = _total / PageSize + 1;
            }
        }

        public PageInfo(int pageSize, int pageIndex)
        {
            this.PageIndex = pageIndex;
            this.PageSize = pageSize;
        }
        public PageInfo()
        {
        }

    }

    /// <summary>
    /// 排序类
    /// </summary>
    [DataContract]
    public class Orderby<TModel> where TModel : IAggregateRoot, new()
    {
        public Expression<Func<TModel, dynamic>> SortPredicate { get; set; }
        public SortOrder OrderBy { get; set; }
        public Orderby(Expression<Func<TModel, dynamic>> sortPredicate, SortOrder orderBy)
        {
            this.SortPredicate = sortPredicate;
            this.OrderBy = orderBy;
        }
    }
}
