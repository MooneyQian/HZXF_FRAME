using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Manage.Framework
{
    /// <summary>
    /// 在实体转换中，声明某个属性不需要交换数值。
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class AdapterMapperIgnore : Attribute
    {
    }
}
