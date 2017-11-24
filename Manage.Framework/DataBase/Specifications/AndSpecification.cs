namespace Manage.Framework
{
    using System;
    using System.Linq.Expressions;

    /// <summary>
    /// 代表一个条件And运算
    /// </summary>
    /// <typeparam name="TEntity">实体类型</typeparam>
    public sealed class AndSpecification<TEntity>
       : CompositeSpecification<TEntity>
       where TEntity : class
    {
        #region Members

        private ISpecification<TEntity> _RightSideSpecification = null;
        private ISpecification<TEntity> _LeftSideSpecification = null;

        #endregion

        #region Public Constructor

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="leftSide">左边规约</param>
        /// <param name="rightSide">右边规约</param>
        public AndSpecification(ISpecification<TEntity> leftSide, ISpecification<TEntity> rightSide)
        {
            if (leftSide == (ISpecification<TEntity>)null)
                throw new ArgumentNullException("leftSide");

            if (rightSide == (ISpecification<TEntity>)null)
                throw new ArgumentNullException("rightSide");

            this._LeftSideSpecification = leftSide;
            this._RightSideSpecification = rightSide;
        }

        #endregion

        #region Composite Specification overrides

        /// <summary>
        ///左边规约
        /// </summary>
        public override ISpecification<TEntity> LeftSideSpecification
        {
            get { return _LeftSideSpecification; }
        }

        /// <summary>
        /// 右边规约
        /// </summary>
        public override ISpecification<TEntity> RightSideSpecification
        {
            get { return _RightSideSpecification; }
        }

        /// <summary>
        /// 满足条件的Lambda表达
        /// </summary>
        /// <returns>满足条件的Lambda表达</returns>
        public override Expression<Func<TEntity, bool>> SatisfiedBy()
        {
            Expression<Func<TEntity, bool>> left = _LeftSideSpecification.SatisfiedBy();
            Expression<Func<TEntity, bool>> right = _RightSideSpecification.SatisfiedBy();

            return (left.And(right));

        }

        #endregion
    }
}
