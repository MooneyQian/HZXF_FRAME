namespace Manage.Framework
{
    using System;
    using System.Linq.Expressions;


    /// <summary>
    /// 条件规约抽象类
    /// </summary>
    /// <typeparam name="TEntity">域实体</typeparam>
    public abstract class Specification<TEntity>
         : ISpecification<TEntity>
         where TEntity : class
    {

        /// <summary>
        /// 创建一个条件规约
        /// </summary>
        /// <param name="expression">规约的表达式</param>
        /// <returns>创建后的条件规约</returns>
        public static Specification<TEntity> Create(Expression<Func<TEntity, bool>> expression)
        {
            return new DirectSpecification<TEntity>(expression);
        }
        
        #region Override Operators

        /// <summary>
        /// 重载&连接符号，实现俩个条件规约的And连接
        /// </summary>
        /// <param name="leftSideSpecification">第一个条件规约</param>
        /// <param name="rightSideSpecification">第二个条件规约</param>
        /// <returns>And连接后的条件规约</returns>
        public static Specification<TEntity> operator &(Specification<TEntity> leftSideSpecification, Specification<TEntity> rightSideSpecification)
        {
            return new AndSpecification<TEntity>(leftSideSpecification, rightSideSpecification);
        }

        /// <summary>
        /// 重载|连接符号，实现俩个条件规约的Or连接
        /// </summary>
        /// <param name="leftSideSpecification">第一个条件规约</param>
        /// <param name="rightSideSpecification">第二个条件规约</param>
        /// <returns>Or连接后的条件规约</returns>
        public static Specification<TEntity> operator |(Specification<TEntity> leftSideSpecification, Specification<TEntity> rightSideSpecification)
        {
            return new OrSpecification<TEntity>(leftSideSpecification, rightSideSpecification);
        }

        /// <summary>
        /// 重载!连接符号，实现俩个条件规约的非连接
        /// </summary>
        /// <param name="specification">条件规约</param>
        /// <returns>非操作后的条件规约</returns>
        public static Specification<TEntity> operator !(Specification<TEntity> specification)
        {
            return new NotSpecification<TEntity>(specification);
        }

        /// <summary>
        /// [本方法未实现]重载false连接符号，实现俩个条件规约的Or连接
        /// </summary>
        /// <param name="specification">条件规约</param>
        /// <returns>false操作后的条件规约</returns>
        public static bool operator false(Specification<TEntity> specification)
        {
            return false;
        }

        /// <summary>
        /// [本方法未实现]重载true连接符号，实现俩个条件规约的Or连接
        /// </summary>
        /// <param name="specification">条件规约</param>
        /// <returns>true操作后的条件规约</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "specification")]
        public static bool operator true(Specification<TEntity> specification)
        {
            return false;
        }

        #endregion

        #region ISpecification<TValueObject> Members

        /// <summary>
        /// 获取条件规约的表达式树
        /// </summary>
        /// <returns>条件规约的表达式树</returns>
        public abstract Expression<Func<TEntity, bool>> SatisfiedBy();

        /// <summary>
        /// 使用And连接俩个规约
        /// </summary>
        /// <param name="other">待连接的规约</param>
        /// <returns>返回连接后的规约</returns>
        public ISpecification<TEntity> And(ISpecification<TEntity> other)
        {
            return new AndSpecification<TEntity>(this, other);
        }
        /// <summary>
        /// 使用Or连接俩个规约
        /// </summary>
        /// <param name="other">待连接的规约</param>
        /// <returns>返回连接后的规约</returns>
        public ISpecification<TEntity> Or(ISpecification<TEntity> other)
        {
            return new OrSpecification<TEntity>(this, other);
        }

        /// <summary>
        /// 使用Not否定规约
        /// </summary>
        /// <returns>使用Not否定规约</returns>
        public ISpecification<TEntity> Not()
        {
            return new NotSpecification<TEntity>(this);
        }

        #endregion
    }
}

