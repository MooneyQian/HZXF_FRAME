using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Aspose.Cells;
using System.Data;

namespace Manage.Helpers
{
    public static class AsposeExcelHelper
    {

        #region 生成Excel
        /// <summary>
        /// 生成Excel
        /// </summary>
        /// <param name="dataTable">数据源,TableName为标题(为空不输出)；ColumnName为列名（列名以[_right]、[_center]、[_left]结尾可以控制单元格对其方向）；单元格值以$span$结尾表示不填值，以_$rowspan$2$或_$colspan$2$表示合并单元格 </param>
        public static byte[] OutputExcel(DataTable dataTable, List<double> columnWidths, string secondTitle = null)
        {
            return GetWorkbook(dataTable, columnWidths, secondTitle).SaveToStream().GetBuffer();
            //return File(ExcelHelper.OutputExcel(dt, columnWidths), "application/vnd.ms-excel", HttpUtility.UrlEncode(title.ExamName + "统计表.xls"));
        }

        /// <summary>
        /// 生成excel通用方法
        /// </summary>
        /// <param name="dataTable"> </param>
        /// <returns></returns>
        private static Workbook GetWorkbook(DataTable dataTable, List<double> columnWidths, string secondTitle)
        {
            Workbook workbook = new Workbook();//工作簿 
            Worksheet sheet = (Worksheet)workbook.Worksheets[0];//工作表 
            Cells cells = sheet.Cells;//单元格 

            #region 设置样式
            //为标题设置样式     
            Style styleTitle = workbook.Styles[workbook.Styles.Add()];//新增样式 
            styleTitle.HorizontalAlignment = TextAlignmentType.Center;//文字居中 
            styleTitle.Font.Name = "宋体";//文字字体 
            styleTitle.Font.Size = 18;//文字大小 
            styleTitle.Font.IsBold = true;//粗体 

            //为负标题设置样式     
            Style styleTitle2 = workbook.Styles[workbook.Styles.Add()];//新增样式 
            styleTitle2.HorizontalAlignment = TextAlignmentType.Left;
            styleTitle2.Font.Name = "宋体";//文字字体 
            styleTitle2.Font.Size = 14;//文字大小 
            //styleTitle2.Font.IsBold = false;//粗体 

            //列名
            Style style2 = workbook.Styles[workbook.Styles.Add()];//新增样式 
            style2.HorizontalAlignment = TextAlignmentType.Center;//文字居中 
            style2.Font.Name = "宋体";//文字字体 
            style2.Font.Size = 13;//文字大小 
            style2.Font.IsBold = true;//粗体 
            style2.IsTextWrapped = true;//单元格内容自动换行 
            style2.Borders[BorderType.LeftBorder].LineStyle = CellBorderType.Thin;
            style2.Borders[BorderType.RightBorder].LineStyle = CellBorderType.Thin;
            style2.Borders[BorderType.TopBorder].LineStyle = CellBorderType.Thin;
            style2.Borders[BorderType.BottomBorder].LineStyle = CellBorderType.Thin;

            //内容样式-局中
            Style style3 = workbook.Styles[workbook.Styles.Add()];//新增样式 
            style3.HorizontalAlignment = TextAlignmentType.Center;//文字居中 
            style3.Font.Name = "宋体";//文字字体 
            style3.Font.Size = 12;//文字大小 
            style3.IsTextWrapped = true;//单元格内容自动换行
            style3.Borders[BorderType.LeftBorder].LineStyle = CellBorderType.Thin;
            style3.Borders[BorderType.RightBorder].LineStyle = CellBorderType.Thin;
            style3.Borders[BorderType.TopBorder].LineStyle = CellBorderType.Thin;
            style3.Borders[BorderType.BottomBorder].LineStyle = CellBorderType.Thin;

            //内容样式-左对齐
            Style style4 = workbook.Styles[workbook.Styles.Add()];//新增样式 
            style4.HorizontalAlignment = TextAlignmentType.Left;//文字居中 
            style4.Font.Name = "宋体";//文字字体 
            style4.Font.Size = 12;//文字大小 
            style4.IsTextWrapped = true;//单元格内容自动换行
            style4.Borders[BorderType.LeftBorder].LineStyle = CellBorderType.Thin;
            style4.Borders[BorderType.RightBorder].LineStyle = CellBorderType.Thin;
            style4.Borders[BorderType.TopBorder].LineStyle = CellBorderType.Thin;
            style4.Borders[BorderType.BottomBorder].LineStyle = CellBorderType.Thin;
            //内容样式-右对齐
            Style style5 = workbook.Styles[workbook.Styles.Add()];//新增样式 
            style5.HorizontalAlignment = TextAlignmentType.Right;//文字居中 
            style5.Font.Name = "宋体";//文字字体 
            style5.Font.Size = 12;//文字大小 
            style5.IsTextWrapped = true;//单元格内容自动换行
            style5.Borders[BorderType.LeftBorder].LineStyle = CellBorderType.Thin;
            style5.Borders[BorderType.RightBorder].LineStyle = CellBorderType.Thin;
            style5.Borders[BorderType.TopBorder].LineStyle = CellBorderType.Thin;
            style5.Borders[BorderType.BottomBorder].LineStyle = CellBorderType.Thin;
            #endregion

            int ColumnNum = dataTable.Columns.Count;//表格列数
            int RowNum = dataTable.Rows.Count;//内容行数
            int CurrentRow = 0;//当前行

            //如果表名不为空，创建标题
            if (!string.IsNullOrEmpty(dataTable.TableName))
            {
                //生成标题行
                cells.Merge(0, 0, 1, ColumnNum);//合并单元格 
                cells[0, 0].PutValue(dataTable.TableName);//填写内容 
                cells[0, 0].SetStyle(styleTitle);//设置样式
                cells.SetRowHeight(0, 38);//设置行高

                CurrentRow++;
            }
            //设置负标题
            if (secondTitle != null)
            {
                cells.Merge(CurrentRow, 0, 1, ColumnNum);//合并单元格 
                cells[CurrentRow, 0].PutValue(secondTitle);//填写内容 
                cells[CurrentRow, 0].SetStyle(styleTitle2);//设置样式
                cells.SetRowHeight(CurrentRow, 27);//设置行高

                CurrentRow++;
            }
            //创建列名
            for (int i = 0; i < ColumnNum; i++)
            {
                cells[CurrentRow, i].PutValue(dataTable.Columns[i].ColumnName.Replace("[_right]", "").Replace("[_center]", "").Replace("[_left]", ""));
                cells[CurrentRow, i].SetStyle(style2);
                cells.SetRowHeight(CurrentRow, 25);
                cells.SetColumnWidth(i, columnWidths[i]);
            }
            CurrentRow++;
            //创建内容
            foreach (DataRow row in dataTable.Rows)
            {
                for (int i = 0; i < ColumnNum; i++)
                {
                    //处理合并单元格
                    if (row[i].ToString().IndexOf("_$rowspan$") > -1 || row[i].ToString().IndexOf("_$colspan$") > -1)
                    {
                        bool iscolspan = row[i].ToString().IndexOf("_$rowspan$") > -1 ? false : true;//合并方向，0列合并，1行合并。
                        int count = row[i].ToString().Split('$')[row[i].ToString().Split('$').Length - 2].Convert<int>(1);//合并数
                        cells.Merge(CurrentRow, i, iscolspan ? 1 : count, iscolspan ? count : 1);
                        //内容中移除合并单元格编码
                        string replaceStr = iscolspan ? "_$colspan$" + count + "$" : "_$rowspan$" + count + "$";
                        cells[CurrentRow, i].PutValue(row[i].ToString().Replace(replaceStr, ""));
                    }
                    else
                    {
                        if (row[i].ToString() != "$span$")//合并单元格，不需要填值
                            cells[CurrentRow, i].PutValue(row[i]);
                    }
                    if (dataTable.Columns[i].ColumnName.IndexOf("[_right]") > -1)
                        cells[CurrentRow, i].SetStyle(style5);
                    else if (dataTable.Columns[i].ColumnName.IndexOf("[_center]") > -1)
                        cells[CurrentRow, i].SetStyle(style3);
                    else
                        cells[CurrentRow, i].SetStyle(style4);
                    cells.SetRowHeight(CurrentRow, 25);
                }
                CurrentRow++;
            }

            return workbook;
        }

        #endregion

        #region 填充Excel

        /// <summary>
        /// 填充Excel模版
        /// </summary>
        /// <param name="dataTable">数据源,TableName为标题(为空不输出)；ColumnName为列名（列名以_right、_center、_left结尾可以控制单元格对其方向）；单元格值以$span$结尾表示不填值，以_$rowspan$2$或_$colspan$2$表示合并单元格</param>
        /// <param name="TemplateFilePath">模版路径</param>
        /// <param name="StartRowIndex">开始填充行序号</param>
        /// <param name="StartColumnIndex">开始填充列序号</param>
        /// <param name="isFillColumnName">是否生成列名行</param>
        /// <param name="titleRowNum">标题行号[可空]</param>
        /// <param name="titleColumnNum">标题列号[可空]</param>
        /// <param name="secondTitleRowNum">副标题行号[可空]</param>
        /// <param name="secondTitleColumnNum">副标题列号[可空]</param>
        /// <param name="secondTitle">副标题[可空]</param>
        /// <param name="columnWidths">列宽[可空]</param>
        /// <returns></returns>
        public static byte[] FillingExcel(DataTable dataTable, string TemplateFilePath, int StartRowIndex, int StartColumnIndex, bool isFillColumnName,
            int titleRowNum = -1, int titleColumnNum = -1, int secondTitleRowNum = -1, int secondTitleColumnNum = -1, string secondTitle = null, List<double> columnWidths = null)
        {
            return FillingWorkbook(dataTable, TemplateFilePath, StartRowIndex, StartColumnIndex, isFillColumnName,
                titleRowNum, titleColumnNum, secondTitleRowNum, secondTitleColumnNum, secondTitle, columnWidths).SaveToStream().GetBuffer();
        }

        private static Workbook FillingWorkbook(DataTable dataTable, string TemplateFilePath, int StartRowIndex, int StartColumnIndex, bool isFillColumnName,
            int titleRowNum, int titleColumnNum, int secondTitleRowNum, int secondTitleColumnNum, string secondTitle, List<double> columnWidths)
        {
            Workbook workbook = new Workbook();//工作簿 
            workbook.Open(TemplateFilePath);
            Worksheet sheet = (Worksheet)workbook.Worksheets[0];//工作表 
            Cells cells = sheet.Cells;//单元格 

            #region 设置样式
            //为标题设置样式     
            Style styleTitle = workbook.Styles[workbook.Styles.Add()];//新增样式 
            styleTitle.HorizontalAlignment = TextAlignmentType.Center;//文字居中 
            styleTitle.Font.Name = "宋体";//文字字体 
            styleTitle.Font.Size = 18;//文字大小 
            styleTitle.Font.IsBold = true;//粗体 

            //为负标题设置样式     
            Style styleTitle2 = workbook.Styles[workbook.Styles.Add()];//新增样式 
            styleTitle2.HorizontalAlignment = TextAlignmentType.Left;
            styleTitle2.Font.Name = "宋体";//文字字体 
            styleTitle2.Font.Size = 14;//文字大小 
            //styleTitle2.Font.IsBold = false;//粗体 

            //列名
            Style style2 = workbook.Styles[workbook.Styles.Add()];//新增样式 
            style2.HorizontalAlignment = TextAlignmentType.Center;//文字居中 
            style2.Font.Name = "宋体";//文字字体 
            style2.Font.Size = 13;//文字大小 
            style2.Font.IsBold = true;//粗体 
            style2.IsTextWrapped = true;//单元格内容自动换行 
            style2.Borders[BorderType.LeftBorder].LineStyle = CellBorderType.Thin;
            style2.Borders[BorderType.RightBorder].LineStyle = CellBorderType.Thin;
            style2.Borders[BorderType.TopBorder].LineStyle = CellBorderType.Thin;
            style2.Borders[BorderType.BottomBorder].LineStyle = CellBorderType.Thin;

            //内容样式-局中
            Style style3 = workbook.Styles[workbook.Styles.Add()];//新增样式 
            style3.HorizontalAlignment = TextAlignmentType.Center;//文字居中 
            style3.Font.Name = "宋体";//文字字体 
            style3.Font.Size = 12;//文字大小 
            style3.Borders[BorderType.LeftBorder].LineStyle = CellBorderType.Thin;
            style3.Borders[BorderType.RightBorder].LineStyle = CellBorderType.Thin;
            style3.Borders[BorderType.TopBorder].LineStyle = CellBorderType.Thin;
            style3.Borders[BorderType.BottomBorder].LineStyle = CellBorderType.Thin;

            //内容样式-左对齐
            Style style4 = workbook.Styles[workbook.Styles.Add()];//新增样式 
            style4.HorizontalAlignment = TextAlignmentType.Left;//文字居中 
            style4.Font.Name = "宋体";//文字字体 
            style4.Font.Size = 12;//文字大小 
            style4.Borders[BorderType.LeftBorder].LineStyle = CellBorderType.Thin;
            style4.Borders[BorderType.RightBorder].LineStyle = CellBorderType.Thin;
            style4.Borders[BorderType.TopBorder].LineStyle = CellBorderType.Thin;
            style4.Borders[BorderType.BottomBorder].LineStyle = CellBorderType.Thin;
            //内容样式-右对齐
            Style style5 = workbook.Styles[workbook.Styles.Add()];//新增样式 
            style5.HorizontalAlignment = TextAlignmentType.Right;//文字居中 
            style5.Font.Name = "宋体";//文字字体 
            style5.Font.Size = 12;//文字大小 
            style5.Borders[BorderType.LeftBorder].LineStyle = CellBorderType.Thin;
            style5.Borders[BorderType.RightBorder].LineStyle = CellBorderType.Thin;
            style5.Borders[BorderType.TopBorder].LineStyle = CellBorderType.Thin;
            style5.Borders[BorderType.BottomBorder].LineStyle = CellBorderType.Thin;
            #endregion

            int ColumnNum = dataTable.Columns.Count;//表格列数
            int RowNum = dataTable.Rows.Count;//内容行数
            int CurrentRow = StartRowIndex;//当前行

            //如果表名不为空，创建标题
            if (!string.IsNullOrEmpty(dataTable.TableName))
            {
                //已有标题行
                if (titleRowNum != -1 && titleColumnNum != -1)
                {
                    cells[titleRowNum, titleColumnNum].PutValue(dataTable.TableName);//填写内容 
                }
                else
                {
                    //生成标题行
                    cells.Merge(CurrentRow, StartColumnIndex, 1, ColumnNum);//合并单元格 
                    cells[CurrentRow, StartColumnIndex].PutValue(dataTable.TableName);//填写内容 
                    cells[CurrentRow, StartColumnIndex].SetStyle(styleTitle);//设置样式
                    cells.SetRowHeight(CurrentRow, 38);//设置行高

                    CurrentRow++;
                }
            }
            //设置负标题
            if (secondTitle != null)
            {
                //如果已有副标题行
                if (secondTitleRowNum != -1 && secondTitleColumnNum != -1)
                {
                    cells[secondTitleRowNum, secondTitleColumnNum].PutValue(secondTitle);//填写内容 
                }
                else
                {
                    cells.Merge(CurrentRow, StartColumnIndex, 1, ColumnNum);//合并单元格 
                    cells[CurrentRow, StartColumnIndex].PutValue(secondTitle);//填写内容 
                    cells[CurrentRow, StartColumnIndex].SetStyle(styleTitle2);//设置样式
                    cells.SetRowHeight(CurrentRow, 27);//设置行高

                    CurrentRow++;
                }
            }
            //创建列名
            if (isFillColumnName)
            {
                for (int i = StartColumnIndex; i < ColumnNum; i++)
                {
                    cells[CurrentRow, i].PutValue(dataTable.Columns[i].ColumnName.Replace("[_right]", "").Replace("[_center]", "").Replace("[_left]", ""));
                    cells[CurrentRow, i].SetStyle(style2);
                    cells.SetRowHeight(CurrentRow, 25);
                    if (columnWidths != null)
                        cells.SetColumnWidth(i, columnWidths[i]);
                }
                CurrentRow++;
            }
            //创建内容
            foreach (DataRow row in dataTable.Rows)
            {
                for (int i = StartColumnIndex; i < ColumnNum; i++)
                {
                    //处理合并单元格
                    if (row[i].ToString().IndexOf("_$rowspan$") > -1 || row[i].ToString().IndexOf("_$colspan$") > -1)
                    {
                        bool iscolspan = row[i].ToString().IndexOf("_$rowspan$") > -1 ? false : true;//合并方向，0列合并，1行合并。
                        int count = row[i].ToString().Split('$')[row[i].ToString().Split('$').Length - 2].Convert<int>(1);//合并数
                        cells.Merge(CurrentRow, i, iscolspan ? 1 : count, iscolspan ? count : 1);
                        //内容中移除合并单元格编码
                        string replaceStr = iscolspan ? "_$colspan$" + count + "$" : "_$rowspan$" + count + "$";
                        cells[CurrentRow, i].PutValue(row[i].ToString().Replace(replaceStr, ""));
                    }
                    else
                    {
                        if (row[i].ToString() != "$span$")//合并单元格，不需要填值
                            cells[CurrentRow, i].PutValue(row[i]);
                    }
                    if (dataTable.Columns[i].ColumnName.IndexOf("[_right]") > -1)
                        cells[CurrentRow, i].SetStyle(style5);
                    else if (dataTable.Columns[i].ColumnName.IndexOf("[_center]") > -1)
                        cells[CurrentRow, i].SetStyle(style3);
                    else
                        cells[CurrentRow, i].SetStyle(style4);
                    cells.SetRowHeight(CurrentRow, 25);
                }
                CurrentRow++;
            }

            return workbook;
        }

        #endregion

        #region 读取Excel
        /// <summary>
        /// 读取excel文件到DataSet中
        /// </summary>
        /// <param name="excelPath">文件路径</param>
        /// <param name="hasTitle">首行是否为标题</param>
        /// <returns></returns>
        public static DataSet InputExcelToDataSet(string excelPath, bool hasTitle = false)
        {
            try
            {
                DataSet ds = new DataSet();
                Workbook workbook = new Workbook();
                workbook.Open(excelPath);
                for (int m = 0; m < workbook.Worksheets.Count; m++)
                {
                    Cells cells = workbook.Worksheets[m].Cells;
                    DataTable dt = new DataTable();
                    if (cells.MaxDataRow > 0)
                    {
                        if (hasTitle)
                        {
                            for (int j = 0; j < cells.MaxDataColumn + 1; j++)
                            {
                                dt.Columns.Add(cells[0, j].StringValue.Trim());
                            }
                        }
                        else
                        {
                            for (int j = 0; j < cells.MaxDataColumn + 1; j++)
                            {
                                dt.Columns.Add(j.ToString());
                            }
                        }

                        for (int i = (hasTitle ? 1 : 0); i < cells.MaxDataRow + 1; i++)
                        {
                            DataRow dr = dt.NewRow();
                            for (int j = 0; j < cells.MaxDataColumn + 1; j++)
                            {
                                dr[j] = cells[i, j].StringValue.Trim();
                            }
                            dt.Rows.Add(dr);
                        }
                    }
                    ds.Tables.Add(dt);
                }
                return ds;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion
    }
}
