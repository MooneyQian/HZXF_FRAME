using Aspose.Cells;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Manage.Common.Helpers
{
    /// <summary>
    /// Aspose导出Excel扩展
    /// </summary>
    public class AsposeExcelExtend
    {
        /// <summary>
        /// 模板生成excel
        /// </summary>
        /// <param name="ds">数据源（模版中用 &=columnname 来定义列名）</param>
        /// <param name="param">参数（模版中用 &=$paramname 来定义）</param>
        /// <param name="templateFilePath">模版绝对路径</param>
        /// <returns></returns>
        public static byte[] OutputExcelTemplate(DataTable dt, Dictionary<string, object> param, string templateFilePath)
        {
            DataSet ds = new DataSet();
            ds.Tables.Add(dt);
            return OutputExcelTemplate(ds, param, templateFilePath);
        }

        /// <summary>
        /// 模板生成excel
        /// </summary>
        /// <param name="ds">数据源（模版中用 &=tablename.columnname 来定义表名和列名）</param>
        /// <param name="param">参数（模版中用 &=$paramname 来定义）</param>
        /// <param name="templateFilePath">模版绝对路径</param>
        /// <returns></returns>
        public static byte[] OutputExcelTemplate(DataSet ds, Dictionary<string, object> param, string templateFilePath)
        {
            //创建一个workbookdesigner对象
            WorkbookDesigner designer = new WorkbookDesigner();
            designer.Open(templateFilePath);
            //设置datatable对象
            designer.SetDataSource(ds);
            if (param != null)
            {
                foreach (var keyvalue in param)
                {
                    designer.SetDataSource(keyvalue.Key, keyvalue.Value);
                }
            }

            //赋值
            designer.Process();

            return designer.Workbook.SaveToStream().GetBuffer();
        }
    }
}
