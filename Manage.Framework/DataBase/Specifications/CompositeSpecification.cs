using System;
namespace Manage.Framework
{
    /// <summary>
    /// 代表复合规约的基类
    /// </summary>
    /// <typeparam name="TEntity">实体类型</typeparam>
    public abstract class CompositeSpecification<TEntity>
         : Specification<TEntity>
         where TEntity : class
    {
        #region Properties

        /// <summary>
        /// 复合规约的左边规约
        /// </summary>
        public abstract ISpecification<TEntity> LeftSideSpecification { get; }

        /// <summary>
        /// 复合规约的右边规约
        /// </summary>
        public abstract ISpecification<TEntity> RightSideSpecification { get; }

        #endregion
    }
}