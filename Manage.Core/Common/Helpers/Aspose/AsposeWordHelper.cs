using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Aspose.Words;
using System.Data;
using Aspose.Words.Tables;
using System.IO;
using Aspose.Words.Saving;

namespace Manage.Helpers
{
    public static class AsposeWordHelper
    {
        #region 基础方法
        /// <summary>
        /// 生成word
        /// </summary>
        /// <param name="templatePath">模版路径</param>
        /// <param name="dataTable">数据源</param>
        public static void GenerateWord(string templatePath, string filePath, DataTable dataTable)
        {
            //载入模板
            var doc = new Aspose.Words.Document(templatePath);
            //合并模版，相当于页面的渲染
            doc.MailMerge.Execute(dataTable);
            //更新所有域的值（为公式域重新计算值）
            doc.UpdateFields();
            //生成文件
            doc.Save(filePath);
        }

        /// <summary>
        /// 生成word
        /// </summary>
        /// <param name="templatePath">模版路径</param>
        /// <param name="dataTable">数据源</param>
        /// <param name="dataSet">生成表格数据源，每个DataTable的TableName为表格的名称</param>
        public static byte[] GenerateWord(string templatePath, DataTable dataTable, DataSet dataSet = null)
        {
            //载入模板
            var doc = new Aspose.Words.Document(templatePath);
            //合并模版，相当于页面的渲染
            if (dataTable != null)
                doc.MailMerge.Execute(dataTable);
            if (dataSet != null)
                doc.MailMerge.ExecuteWithRegions(dataSet);
            //更新所有域的值（为公式域重新计算值）
            doc.UpdateFields();
            var docStream = new MemoryStream();
            doc.Save(docStream, SaveOptions.CreateSaveOptions(SaveFormat.Doc));
            return docStream.ToArray();
        }


        /// <summary>
        /// 生成word with BookMark
        /// </summary>
        /// <param name="templatePath">模版路径</param>
        /// <param name="filePath">生成文件路径</param>
        /// <param name="dataTable">数据源</param>
        /// <param name="tables">需要生成表格的数据源，每个DataTable的TableName为对应的书签名</param>
        public static void GenerateWordWithBookmark(string templatePath, string filePath, DataTable dataTable, DataSet tables = null, List<double> columnsWidth = null)
        {
            //载入模板
            var doc = new Aspose.Words.Document(templatePath);
            DocumentBuilder builder = new DocumentBuilder(doc);
            foreach (DataColumn col in dataTable.Columns)
            {
                //判断是否有该标签
                if (doc.Range.Bookmarks[col.ColumnName] != null)
                {
                    //标签值替换
                    doc.Range.Bookmarks[col.ColumnName].Text = dataTable.Rows[0][col.ColumnName].ToString();
                }
            }
            //判断需要创建表格
            if (tables != null)
            {
                doc.MailMerge.ExecuteWithRegions(tables);
            }
            //更新所有域的值（为公式域重新计算值）
            doc.UpdateFields();

            //保存合并后的文档
            doc.Save(filePath);
        }


        /// <summary>
        /// 将word导出成pdf
        /// </summary>
        /// <param name="WordFilePath"></param>
        /// <param name="PdfFilePath"></param>
        public static void GeneratePdf(string WordFilePath, string PdfFilePath)
        {
            //载入word
            var doc = new Aspose.Words.Document(WordFilePath);

            //导出pdf
            doc.Save(PdfFilePath, SaveFormat.Pdf);
        }

        /// <summary>
        /// 生成多个模版内容合并的文档
        /// </summary>
        /// <param name="templatePath">模版路径</param>
        /// <param name="filePath">生成文件路径</param>
        /// <param name="dataSet">每个模版对应的数据，每个DataTable的列名对应模版签名名</param>
        /// <param name="insertPageBreak">每段内容间是否插入分页符</param>
        public static void GenerateWordWithBookmarkMultiple(string templatePath, string filePath, DataSet dataSet, bool insertPageBreak = false)
        {

            //载入模板
            var temp_doc = new Aspose.Words.Document(templatePath);
            //创建生成文件
            var new_doc = new Aspose.Words.Document();
            //新文档build
            var new_bulid = new DocumentBuilder(new_doc);
            int i = 0;
            foreach (DataTable dataTable in dataSet.Tables)
            {
                foreach (DataColumn col in dataTable.Columns)
                {
                    //判断是否有该标签
                    if (temp_doc.Range.Bookmarks[col.ColumnName] != null)
                    {
                        //标签值替换
                        temp_doc.Range.Bookmarks[col.ColumnName].Text = dataTable.Rows[0][col.ColumnName].ToString();
                    }
                }
                //追加到新文档
                new_bulid.MoveToDocumentEnd();//移动到文档末尾
                if (i != 0 && insertPageBreak)
                    new_bulid.InsertBreak(BreakType.SectionBreakNewPage);//插入分页符
                InsertDocument(new_bulid.CurrentParagraph, temp_doc);//插入模版内容
                //删除空行
                if (!new_bulid.CurrentParagraph.HasChildNodes)
                    new_bulid.CurrentParagraph.Remove();
                i++;
            }

            //更新所有域的值（为公式域重新计算值）
            new_doc.UpdateFields();
            //保存合并后的文档
            new_doc.Save(filePath);
        }

        #endregion

        #region Insert a Document into another Document 官方方法
        /// <summary>
        /// Inserts content of the external document after the specified node.
        /// Section breaks and section formatting of the inserted document are ignored.
        /// </summary>
        /// <param name="insertAfterNode">Node in the destination document after which the content 
        /// should be inserted. This node should be a block level node (paragraph or table).</param>
        /// <param name="srcDoc">The document to insert.</param>
        private static void InsertDocument(Node insertAfterNode, Document srcDoc)
        {
            // Make sure that the node is either a paragraph or table.
            if ((!insertAfterNode.NodeType.Equals(NodeType.Paragraph)) &
              (!insertAfterNode.NodeType.Equals(NodeType.Table)))
                throw new ArgumentException("The destination node should be either a paragraph or table.");

            // We will be inserting into the parent of the destination paragraph.
            CompositeNode dstStory = insertAfterNode.ParentNode;

            // This object will be translating styles and lists during the import.
            NodeImporter importer = new NodeImporter(srcDoc, insertAfterNode.Document, ImportFormatMode.KeepSourceFormatting);

            // Loop through all sections in the source document.
            foreach (Section srcSection in srcDoc.Sections)
            {
                // Loop through all block level nodes (paragraphs and tables) in the body of the section.
                foreach (Node srcNode in srcSection.Body)
                {
                    // Let's skip the node if it is a last empty paragraph in a section.
                    if (srcNode.NodeType.Equals(NodeType.Paragraph))
                    {
                        Paragraph para = (Paragraph)srcNode;
                        if (para.IsEndOfSection && !para.HasChildNodes)
                            continue;
                    }

                    // This creates a clone of the node, suitable for insertion into the destination document.
                    Node newNode = importer.ImportNode(srcNode, true);

                    // Insert new node after the reference node.
                    dstStory.InsertAfter(newNode, insertAfterNode);
                    insertAfterNode = newNode;
                }
            }
        }

        //Inserting a Document at a Bookmark
        //InsertDocument(bookmark.BookmarkStart.ParentNode, subDoc);

        //Inserting a Document During Mail Merge
        //builder.MoveToMergeField(e.DocumentFieldName);
        //InsertDocument(builder.CurrentParagraph, subDoc);
        #endregion
    }
}
