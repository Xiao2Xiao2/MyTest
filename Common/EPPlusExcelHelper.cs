using System;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;

using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using OfficeOpenXml;
using OfficeOpenXml.DataValidation;
using OfficeOpenXml.Utils;
using System.Text.RegularExpressions;
using System.Drawing;

namespace Nicpbp.PhyCheck.Excel
{
    public class EPPlusExcelHelper
    {
        #region Constants and Fields

        public const int MaxSheetRows2007 = 1048576;

        #endregion

        #region Public Methods

        /// <summary>
        /// 导出功能,DataTable导出为Excel
        /// </summary>
        /// <param name="table">要导出的DataTable</param>
        /// <param name="fileName">要导出的Excel</param>
        public static void Export(DataTable table, string fileName)
        {
            var rows = 0;
            Export(table, fileName, string.Empty, ref rows);
        }

        

        public static void Export(DataTable table, string fileName, string sheetName, ref int rowWrited)
        {
            if (table == null || table.Rows.Count == 0)
            {
                return;
            }
            if (string.IsNullOrEmpty(sheetName))
            {
                sheetName = "Sheet";
            }
            //if (table.Rows.Count > ExcelUtil.GetMaxRowSupported(fileName))
            //    throw new ArgumentException(string.Format("data rows cann't be more than {0}",
            //        ExcelUtil.GetMaxRowSupported(fileName)));
            FileInfo fi;
            var excel = new ExcelPackage(fi = new FileInfo(fileName));
            using (excel)
            {
                WriteSheets(table, excel, sheetName);
                //excel.Save();
                excel.SaveAs(new FileInfo(fileName));

            }
        }


     

        public static DataTable Import(string fileName)
        {
            var dt = new DataTable();
            using (var excel = new ExcelPackage(new FileInfo(fileName)))
            {
                try
                {
                    var sheet = excel.Workbook.Worksheets.First();
                    if (sheet == null)
                    {
                        return null;
                    }
                    foreach (var cell in sheet.Cells[1, 1, 1, sheet.Dimension.End.Column])
                    {
                        if (cell != null && cell.Value != null)
                        {
                            dt.Columns.Add(cell.Value.ToString());
                        }
                        else
                        {
                            continue;
                        }
                    }
                    var rows = sheet.Dimension.End.Row;
                    for (var i = 2; i <= rows; i++)
                    {
                        var row = sheet.Cells[i, 1, i, sheet.Dimension.End.Column];
                        var cellArray = row.Select(cell => cell.Value).ToArray();
                        if (cellArray != null)
                        {
                            var count = cellArray.Where(cv => cv==null).Count();
                            if (count < sheet.Dimension.End.Column)
                                dt.Rows.Add(cellArray);
                        }
                    }
                }
                catch (Exception e)
                {
                }
                return dt;
            }
        }

        /// <summary>
        /// 为excel中的列创建下拉框
        /// </summary>
        /// <param name="filename">包含全路径的文件名</param>
        /// <param name="columnCount">列数量</param>
        /// <param name="valueSource">下拉框数据源，key:列名 value:数据源集合</param>
        public static void CreateDropdown(string filename, int columnCount, Dictionary<string, List<string>> valueSource)
        {
            //System.Threading.Thread.Sleep(10000);
            //FileInfo fi = new FileInfo(filename);

            int dropDownCount = 0;
            using (var package = new ExcelPackage(new FileInfo(filename)))
            {

                if (package.Workbook.Worksheets.Count > 0)
                {
                    var sheet = package.Workbook.Worksheets[1];

                    for (int i = 1; i <= columnCount; i++)
                    {

                        if (valueSource.ContainsKey(sheet.Cells[1, i].Value.ToString().ToUpper()))
                        {
                            string addressFrom = sheet.Cells[2, i].Address;
                            string addressTo = sheet.Cells[60000, i].Address;

                            // add a validation and set values
                            var validation = sheet.DataValidations.AddListValidation(
                                String.Format("{0}:{1}", addressFrom, addressTo));

                            validation.ShowErrorMessage = true;
                            validation.ErrorStyle = ExcelDataValidationWarningStyle.stop;
                            validation.ErrorTitle = "An invalid value was entered";
                            validation.Error = "Select a value from the list";
                            dropDownCount = 0;

                            foreach (var item in valueSource[sheet.Cells[1, i].Value.ToString().ToUpper()])
                            {
                                dropDownCount++;
                                //下拉项的值
                                if (dropDownCount == 53)
                                    break;
                                validation.Formula.Values.Add(item);
                            }

                        }
                    }
                }
                package.Save();
            }

        }

        /// <summary>
        /// 校验方式
        /// </summary>
        public enum VBACalType
        {
            /// <summary>
            /// 自动计算校验 如 A=B+C
            /// </summary>
            AutoCaculation = 1,

            /// <summary>
            /// 计算公式校验，如 A+B>C 
            /// </summary>
            Validate,

            /// <summary>
            /// 根据表结构设计的校验，如是否可为空，字符长度等
            /// </summary>
            TableStruct
        }

        /// <summary>
        /// 计算结果类型
        /// </summary>
        public enum VBAResultType
        {
            NOLIMIT = 1,
            INTEGER,
            FLOAT,
            STRING
        }

        /// <summary>
        /// 表头语言
        /// </summary>
        public enum TargetHeaderLanguage
        {
            Chinese,
            English
        }

        /// <summary>
        /// 生成VBA代码的参数
        /// </summary>
        public class VBAParam
        {
            /// <summary>
            /// 计算公式 如：A=B+C-D
            /// </summary>
            public string EXP { get; set; }
            /// <summary>
            /// 计算公式两边字段的集合，key为公式左边的字段，value为公式右边的字段
            /// </summary>
            public Dictionary<List<string>, List<string>> Fields { get; set; }
            /// <summary>
            /// 提示消息
            /// </summary>
            public string InvalidMessage { get; set; }
            /// <summary>
            /// 校验方式 
            /// </summary>
            public VBACalType CalType { get; set; }
            /// <summary>
            /// 计算结果的类型
            /// </summary>
            public VBAResultType ResultType { get; set; }
            /// <summary>
            /// 小数点位数
            /// </summary>
            public int DecimalLength { get; set; }
            /// <summary>
            /// 字符串长度
            /// </summary>
            public int StringLength { get; set; }
            /// <summary>
            /// vba循环代码，从xml中获取
            /// </summary>
            public string VBALoopText { get; set; }

            public List<TableStructParam> TableParams { get; set; }

            public class TableStructParam
            {
                public string FIELD_CH_NAME { get; set; }
                public string FIELD_EN_NAME { get; set; }
                public int FIELD_TYPE { get; set; }
                public int FIELD_LENGTH { get; set; }
                public int FIELD_IS_NULL { get; set; }
            }
        }

        /// <summary>
        /// 创建VBA
        /// </summary>
        /// <param name="filename">文件全路径</param>
        /// <param name="columnCount">列数量</param>
        /// <param name="VBAHeadText"></param>
        /// <param name="vbaLoopText"></param>
        /// <param name="VBAFootText"></param>
        /// <param name="headFields"></param>
        public static void CreateVBA(string filename, int columnCount,
            string VBAHeadText,
            List<VBAParam> vbaLoopText,
            string VBAFootText,
            Dictionary<string, string> headFields)
        {
            FileInfo fi = new FileInfo(filename);

            //ExcelPackage pck2;

            using (ExcelPackage pck = new ExcelPackage(fi), pck2 = new ExcelPackage())
            {
                //pck2.Workbook.VbaProject.Protection.SetPassword("neotrident_sjq");
                pck2.Workbook.Worksheets.Add(pck.Workbook.Worksheets[1].Name, pck.Workbook.Worksheets[1]);

                //Create a vba project             
                pck2.Workbook.CreateVBAProject();
                var sheet = pck2.Workbook.Worksheets[1];
                StringBuilder expIsEmpty;
                StringBuilder expIsNumber;
                StringBuilder expAdv;
                StringBuilder invalidStr;
                var vba = new StringBuilder("");
                vba.Append(VBAHeadText);



                foreach (var para in vbaLoopText)
                {
                    //i++;
                    //if (i == 2) continue;
                    expIsEmpty = new StringBuilder("");
                    expIsNumber = new StringBuilder("");
                    expAdv = new StringBuilder("");
                    invalidStr = new StringBuilder("");

                    var expNew = para.EXP;
                    string macro = para.VBALoopText;
                    var dics = para.Fields;
                    var vbaResType = para.ResultType;
                    var decimalLen = para.DecimalLength;

                    #region 根据表格设置验证字段有效性
                    if (para.CalType == VBACalType.TableStruct)
                    {
                        if (para.TableParams != null && para.TableParams.Count > 0)
                        {
                            string emptyValidation = para.EXP.Substring(
                                para.EXP.IndexOf("{---emptyValidationStart---}"),
                                para.EXP.IndexOf("{---emptyValidationEnd---}") - para.EXP.IndexOf("{---emptyValidationStart---}") + "{---emptyValidationStart---}".Length);

                            string lengthValidation = para.EXP.Substring(
                                para.EXP.IndexOf("{---lengthValidationStart---}"),
                                para.EXP.IndexOf("{---lengthValidationEnd---}") - para.EXP.IndexOf("{---lengthValidationStart---}") + "{---lengthValidationStart---}".Length);

                            string emptyValidationWOTag = emptyValidation.Replace("{---emptyValidationStart---}", "").Replace("{---emptyValidationEnd---}", "");
                            string lengthValidationWOTag = lengthValidation.Replace("{---lengthValidationStart---}", "").Replace("{---lengthValidationEnd---}", "");


                            List<VBAParam.TableStructParam> list = para.TableParams;
                            StringBuilder emptyValidationLoop = new StringBuilder("");
                            StringBuilder lengthValidationLoop = new StringBuilder("");
                            foreach (var item in list)
                            {
                                string colName = GetColName(columnCount, sheet, item.FIELD_EN_NAME);
                                if (String.IsNullOrEmpty(colName)) continue; //excel中不包含该字段，跳过当前循环

                                if (item.FIELD_IS_NULL == 0)
                                {
                                    string tmp1 = emptyValidationWOTag.Replace("{---isEmpty---}", String.Format("Range(\"{0}\" & i).Value2=\"\"", colName));
                                    tmp1 = tmp1.Replace("{---isEmptyMsg---}", String.Format("\"{0}\" & i & \"单元格不允许为空\"", colName));
                                    emptyValidationLoop.Append(tmp1);
                                }

                                if (item.FIELD_LENGTH > 0)
                                {
                                    int len = item.FIELD_LENGTH;
                                    string tmp2 = lengthValidationWOTag.Replace("{---lenExceed---}", String.Format("len(Range(\"{0}\" & i).Value2) > {1}", colName, len));
                                    tmp2 = tmp2.Replace("{---lenExceedMsg---}", String.Format("\"{0}\" & i & \"超过最大长度({1})\"", colName, len));
                                    lengthValidationLoop.Append(tmp2);
                                }
                            }
                            if (emptyValidation.Length > 0)
                            {
                                vba.Append(para.EXP.Replace(emptyValidation, emptyValidationLoop.ToString()).Replace(lengthValidation, ""));
                            }
                            if (lengthValidation.Length > 0)
                                vba.Append(para.EXP.Replace(lengthValidation, lengthValidationLoop.ToString()).Replace(emptyValidation, ""));
                        }
                    }
                    #endregion

                    #region 自动计算
                    else if (para.CalType == VBACalType.AutoCaculation)
                    {
                        //右边计算公式字段
                        List<string> expRight = dics.Values.First();
                        expRight.ForEach(
                            fd =>
                            {
                                //单元格的列标识，例如  B1， 则返回B
                                string colName = GetColName(columnCount, sheet, fd);
                                expIsEmpty.Append(String.Format("Range(\"{0}\" & i).Value2 & ", colName));
                                invalidStr.Append(String.Format(" \",\" & Range(\"{0}\" & i).Address & ", colName));
                                expIsNumber.Append(String.Format(" VBA.IsNumeric(Range(\"{0}\" & i).Value2) and ", colName));
                                expNew = expNew.Replace(
                                    "[" + fd + "]",
                                    String.Format("CDbl(Range(\"{0}\" & i).Value2)", colName)
                                    );
                            });

                        expIsNumber.Remove(expIsNumber.ToString().LastIndexOf("and"), 3);
                        macro = macro.Replace("{---isNumber---}", expIsNumber.ToString());

                        //左边被赋值的字段
                        List<string> expLeft = dics.Keys.First();
                        expNew = expNew.Replace(
                            "[" + expLeft[0] + "]",
                            String.Format("Range(\"{0}\" & i).Value2", GetColName(columnCount, sheet, expLeft[0]))
                            );
                        if (VBAResultType.FLOAT == vbaResType || VBAResultType.INTEGER == vbaResType)
                        {
                            if (VBAResultType.INTEGER == vbaResType) decimalLen = 0;
                            expAdv.Append(
                                String.Format("Range(\"{0}\" & i).Value2 = Application.WorksheetFunction.Round(Range(\"{0}\" & i).Value2,{1})",
                                GetColName(columnCount, sheet, expLeft[0]), decimalLen)
                                );
                        }
                        expIsEmpty.Remove(expIsEmpty.ToString().LastIndexOf("&"), 1);
                        expIsEmpty.Append(" <> \"\" ");
                        if (expAdv.Length > 0) expNew += "\r\n" + expAdv.ToString();
                        macro = macro.Replace("{---isEmpty---}", expIsEmpty.ToString());

                        invalidStr.Remove(invalidStr.ToString().LastIndexOf("&"), 1);
                        macro = macro.Replace("{---inValidStr---}", invalidStr.ToString().Substring(1));

                        macro = macro.Replace("{---expression---}", expNew.Replace("[", "").Replace("]", ""));
                        vba.Append(macro);
                    }
                    #endregion

                    #region 校验
                    else if (para.CalType == VBACalType.Validate)
                    {
                        dics.Keys.First().AddRange(dics.Values.First());
                        var expAll = dics.Keys.First();

                        expAll.ForEach(
                            fd =>
                            {
                                //单元格的列标识，例如  B1， 则返回B
                                string colName = GetColName(columnCount, sheet, fd);

                                expIsEmpty.Append(String.Format("Range(\"{0}\" & i).Value2 & ", colName));
                                invalidStr.Append(String.Format(" \",\" & Range(\"{0}\" & i).Address & ", colName));
                                expIsNumber.Append(String.Format(" VBA.IsNumeric(Range(\"{0}\" & i).Value2) and ", colName));
                                expNew = expNew.Replace(
                                    "[" + fd + "]",
                                    String.Format("Range(\"{0}\" & i).Value2", colName)
                                    );
                            });

                        invalidStr.Remove(invalidStr.ToString().LastIndexOf("&"), 1);
                        macro = macro.Replace("{---inValidStr---}", invalidStr.ToString().Substring(1));

                        expIsEmpty.Remove(expIsEmpty.ToString().LastIndexOf("&"), 1);
                        expIsEmpty.Append(" <> \"\" ");
                        macro = macro.Replace("{---isEmpty---}", expIsEmpty.ToString());

                        expIsNumber.Remove(expIsNumber.ToString().LastIndexOf("and"), 3);
                        macro = macro.Replace("{---isNumber---}", expIsNumber.ToString());

                        macro = macro.Replace("{---expression---}", expNew.Replace("[", "").Replace("]", ""));
                        macro = macro.Replace("{---message---}", para.InvalidMessage);
                        vba.Append(macro);
                    }
                    #endregion


                }

                vba.Append(VBAFootText);
                pck2.Workbook.CodeModule.Code = vba.ToString();

                //Optionally, Sign the code with your company certificate.
                /*            
                X509Store store = new X509Store(StoreLocation.CurrentUser);
                store.Open(OpenFlags.ReadOnly);
                pck.Workbook.VbaProject.Signature.Certificate = store.Certificates[0];
                */

                // Save as xlsm
                string newFile = fi.FullName.Replace(fi.Name, fi.Name.Replace(".xlsx", ".xlsm"));
                pck2.File = new FileInfo(newFile);

                ConvertHeaderLanguage(pck2, headFields, TargetHeaderLanguage.Chinese);

                //var fi2 = new FileInfo(newFile);
                //pck.Save();
                //pck2.SaveAs(fi2);
            }
        }

        private static string GetLetters(string str)
        {
            Regex r = new Regex(@"[a-zA-Z]+");
            Match m = r.Match(str);
            return m.Value;
        }

        private static string GetColName(int columnCount, ExcelWorksheet sheet, string col)
        {
            string colName = String.Empty;
            for (int i = 1; i <= columnCount; i++)
            {
                if (col.Equals(sheet.Cells[1, i].Value.ToString(), StringComparison.CurrentCultureIgnoreCase))
                {
                    string address = sheet.Cells[1, i].Address;
                    colName = GetLetters(address);
                    break;
                }
            }
            return colName;
        }

        private static void ConvertHeaderLanguage(ExcelPackage pck, Dictionary<string, string> dics, TargetHeaderLanguage headerLg)
        {
            FileInfo fi;
            var sheet = pck.Workbook.Worksheets[1];
            switch (headerLg)
            {
                case TargetHeaderLanguage.Chinese:
                    for (int i = 1; i <= sheet.Dimension.End.Column; i++)
                    {
                        if (dics.ContainsKey(sheet.Cells[1, i].Value.ToString().ToUpper()))
                        {
                            sheet.Cells[1, i].Value = dics[sheet.Cells[1, i].Value.ToString().ToUpper()];
                            sheet.Cells[1, i].AutoFitColumns();
                        }
                    }
                    break;
                case TargetHeaderLanguage.English:
                    for (int i = 1; i <= sheet.Dimension.End.Column; i++)
                    {
                        if (dics.ContainsValue(sheet.Cells[1, i].Value.ToString()))
                        {
                            sheet.Cells[1, i].Value = dics.FirstOrDefault(d => d.Value == sheet.Cells[1, i].Value.ToString()).Key;
                            sheet.Cells[1, i].AutoFitColumns();
                        }
                    }
                    break;
            }
            //pck.File = new FileInfo(filename);
            //pck.SaveAs(new FileInfo(filename));
            pck.Save();
        }

        /// <summary>
        /// 转换表头语言
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="dics">中英文表头名字键值对，key为英文名，value为中文名</param>
        /// <param name="headerLg"></param>
        public static void ConvertHeaderLanguage(string filename, Dictionary<string, string> dics, TargetHeaderLanguage headerLg)
        {
            FileInfo fi;
            using (ExcelPackage pck = new ExcelPackage(fi = new FileInfo(filename)))
            {
                var sheet = pck.Workbook.Worksheets[1];
                switch (headerLg)
                {
                    case TargetHeaderLanguage.Chinese:
                        for (int i = 1; i <= sheet.Dimension.End.Column; i++)
                        {
                            if (dics.ContainsKey(sheet.Cells[1, i].Value.ToString()))
                            {
                                sheet.Cells[1, i].Value = dics[sheet.Cells[1, i].Value.ToString()];
                                sheet.Cells[1, i].AutoFitColumns();
                            }
                        }
                        break;
                    case TargetHeaderLanguage.English:
                        for (int i = 1; i <= sheet.Dimension.End.Column; i++)
                        {
                            if (dics.ContainsValue(sheet.Cells[1, i].Value.ToString()))
                            {
                                sheet.Cells[1, i].Value = dics.FirstOrDefault(d => d.Value == sheet.Cells[1, i].Value.ToString()).Key;
                                sheet.Cells[1, i].AutoFitColumns();
                            }
                        }
                        break;
                }
                //pck.File = new FileInfo(filename);
                pck.SaveAs(new FileInfo(filename));
            }
        }

        private static object FormatColumeValue(DataColumn column, object cellValue)
        {
            if (column.DataType == typeof(DateTime))
            {
                if (cellValue == null)
                    return cellValue;

                DateTime dt = DateTime.MinValue;
                if (!DateTime.TryParse(cellValue.ToString(), out dt))
                    dt = DateTime.MinValue;

                return dt.ToString("yyyy-MM-dd");
            }
            return cellValue;
        }


        private static void FormatCell(ExcelRangeBase cell, DataColumn column)
        {
            //if (column.DataType == typeof(DateTime))
            //{
            //    cell.Style.Numberformat.Format = System.Globalization.DateTimeFormatInfo.CurrentInfo.LongDatePattern;
            //    return;
            //}
            //if (column.DataType.IsValueType) // == typeof(Decimal) || column.DataType == typeof(Double) || column.DataType == typeof(Single))
            //{
            //    cell.Style.Numberformat.Format = "#,##0.00";
            //}
        }

        #endregion

        #region Methods

        private static ExcelWorksheet CreateSheet(ExcelPackage excel, string sheetName)
        {
            foreach (var sheet in excel.Workbook.Worksheets)
            {
                if (String.Compare(sheet.Name, sheetName, StringComparison.OrdinalIgnoreCase) == 0)
                {
                    return sheet;
                }
            }
            return excel.Workbook.Worksheets.Add(sheetName);
        }


        private static void WriteSheet(DataTable table, ExcelPackage excel, string sheetName, int startRowIndex,
                                       int endRowIndex)
        {
            var sheet = CreateSheet(excel, sheetName);
            var i = 1;
            foreach (DataColumn col in table.Columns)
            {
                FormatCell(sheet.Cells[1, i], col);
                sheet.Cells[1, i].Value = col.ColumnName;
                sheet.Cells[1, i].Style.Font.Bold = true;
                sheet.Cells[1, i].AutoFitColumns();
                i++;
            }
            var columnCount = table.Columns.Count;
            i = 2;
            for (var m = startRowIndex; m <= endRowIndex; m++)
            {
                var row = table.Rows[m];

                //获取行错误信息，并显示到批注中
                string[] cellComments = table.Rows[m].RowError.Split('|');

                //cellComments = null;

                for (var j = 1; j <= columnCount; j++)
                {
                    FormatCell(sheet.Cells[i, j], table.Columns[j - 1]);
                    sheet.Cells[i, j].Value = FormatColumeValue(table.Columns[j - 1], row[j - 1]);

                    if (cellComments != null && cellComments.Length == columnCount)
                    {
                        if (!String.IsNullOrEmpty(cellComments[j - 1]))
                        {
                            //填充批注
                            if (sheet.Cells[i, j].Comment == null)
                                sheet.Cells[i, j].AddComment("", "");
                            sheet.Cells[i, j].Comment.Text = cellComments[j - 1];
                            sheet.Cells[i, j].Comment.Author = "NeoTrident";
                            sheet.Cells[i, j].Comment.AutoFit = true;
                            //设置背景色
                            //sheet.Cells[i, j].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                            //sheet.Cells[i, j].Style.Fill.BackgroundColor.SetColor(Color.Red);

                            //设置字体颜色
                            // sheet.Cells[i, j].Style.Font.Color.SetColor(Color.Red);
                        }
                    }
                }
                i++;
            }
        }

        //private static void WriteSheet(DataTable table, ExcelPackage excel, string sheetName)
        //{
        //    var sheet = CreateSheet(excel, sheetName);
        //    var i = 1;
        //    foreach (DataColumn col in table.Columns)
        //    {
        //        sheet.Cells[1, i].Value = col.ColumnName;
        //        i++;
        //    }
        //    var columnCount = table.Columns.Count;
        //    var rows = table.Rows.Count;
        //    for (i = 2 ; i <= rows; i++)
        //    {
        //        var row = table.Rows[i];
        //        for (var j = 1; j <= columnCount; j++)
        //        {
        //            sheet.Cells[i, j].Value = row[j - 1].ToString();
        //        }
        //    }
        //}

        private static void WriteSheets(DataTable table, ExcelPackage excel, string sheetName)
        {
            const int max = MaxSheetRows2007 - 1;
            var rows = table.Rows.Count;
            var sheetCount = (rows % max == 0) ? rows / max : rows / max + 1;
            for (var sheetNo = 0; sheetNo < sheetCount; sheetNo++)
            {
                WriteSheet(
                    table,
                    excel,
                    (sheetNo == 0) ? sheetName : sheetName + "_" + sheetNo,
                    sheetNo * max,
                    (sheetNo + 1) * max < rows ? (sheetNo + 1) * max - 1 : rows - 1
                    );
            }
            //WriteSheet(table, sheetIndex, ref rowWrited);
        }

        /// <summary>
        /// 将一个List列表转换成DataTable,如果列表为空将返回空的DataTable结构
        /// </summary>
        /// <typeparam name="T">要转换的数据类型</typeparam>
        /// <param name="entityList">List实体对象列表</param> 
        public static DataTable EntityListToDataTable<T>(List<T> entityList)
        {
            DataTable dt = new DataTable();

            //取类型T所有Propertie
            Type entityType = typeof(T);
            PropertyInfo[] entityProperties = entityType.GetProperties();
            Type colType = null;

            foreach (PropertyInfo propInfo in entityProperties)
            {
                if (propInfo.PropertyType.IsGenericType)
                {
                    //返回指定可以为 null 的类型的基础类型参数
                    colType = Nullable.GetUnderlyingType(propInfo.PropertyType);
                }
                else
                {
                    colType = propInfo.PropertyType;
                }

                if (colType.FullName.StartsWith("System"))
                {
                    dt.Columns.Add(propInfo.Name, colType);
                }
            }

            if (entityList != null && entityList.Count > 0)
            {
                foreach (T entity in entityList)
                {
                    DataRow newRow = dt.NewRow();
                    foreach (PropertyInfo propInfo in entityProperties)
                    {
                        if (dt.Columns.Contains(propInfo.Name))
                        {
                            object objValue = propInfo.GetValue(entity, null);
                            newRow[propInfo.Name] = objValue == null ? DBNull.Value : objValue;
                        }
                    }
                    dt.Rows.Add(newRow);
                }

            }

            return dt;
        }

        /// <summary>
        /// 将一个DataTable转换成List列表
        /// </summary>
        /// <typeparam name="T">实体对象的类型</typeparam>
        /// <param name="dt">要转换的DataTable</param>
        /// <returns></returns>
        public static List<T> DataTableToEntityList<T>(DataTable dt)
        {
            List<T> entiyList = new List<T>();

            Type entityType = typeof(T);
            PropertyInfo[] entityProperties = entityType.GetProperties();

            foreach (DataRow row in dt.Rows)
            {
                T entity = Activator.CreateInstance<T>();

                foreach (PropertyInfo propInfo in entityProperties)
                {
                    if (dt.Columns.Contains(propInfo.Name))
                    {
                        if (!row.IsNull(propInfo.Name))
                        {
                            propInfo.SetValue(entity, row[propInfo.Name], null);
                        }
                    }
                }

                entiyList.Add(entity);
            }

            return entiyList;
        }

        #endregion



    }
}
