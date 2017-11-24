namespace Manage.Framework
{
    using System;
    using System.Linq.Expressions;

    /// <summary>
    /// 条件规约接口
    /// </summary>
    /// <typeparam name="TEntity">域实体</typeparam>
    public interface ISpecification<TEntity>
        where TEntity : class
    {
        /// <summary>
        /// 获取条件规约的表达式树
        /// </summary>
        /// <returns>条件规约的表达式树</returns>
        Expression<Func<TEntity, bool>> SatisfiedBy();

        /// <summary>
        /// 使用And连接俩个规约
        /// </summary>
        /// <param name="other">待连接的规约</param>
        /// <returns>返回连接后的规约</returns>
        ISpecification<TEntity> And(ISpecification<TEntity> other);

       /// <summary>
       /// 使用Or连接俩个规约
       /// </summary>
        /// <param name="other">待连接的规约</param>
       /// <returns>返回连接后的规约</returns>
        ISpecification<TEntity> Or(ISpecification<TEntity> other);
       
        /// <summary>
        /// 使用Not否定规约
        /// </summary>
        /// <returns>使用Not否定规约</returns>
        ISpecification<TEntity> Not();
    }
}
