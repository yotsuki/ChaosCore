using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace ChaosCore.CommonLib
{
    public class CsvHelper
    {
        public static bool DebugOutputLine { get; set; } = false;
        public static IEnumerable<DoubleStringDictionary> ReadCsvFile(string csvfile, string encodingname = "GBK")
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            Encoding encoding = Encoding.GetEncoding(encodingname);
            using (var fs = System.IO.File.Open(csvfile, FileMode.Open,FileAccess.Read, FileShare.Read)) {
                var sr = new StreamReader(fs, encoding);
                var header = sr.ReadLine().TrimEnd();
                var headers = SplitCell(header);

                while (!sr.EndOfStream) {
                    var line = sr.ReadLine().TrimEnd(' ', ',');
                    if (DebugOutputLine) {
                        Debug.WriteLine(line);
                    }
                    var cells = SplitCell(line);
                    if(cells == null) {
                        continue;
                    }
                    var dict = new DoubleStringDictionary();
                    for (int i = 0; i < (headers.Count); i++) {
                        if (i >= cells.Count) {
                            break;
                        }
                        dict[headers[i]] = cells[i].Trim();
                    }
                    yield return dict;
                }
            }
        }
        public static IEnumerable<string> ReadCsvFileHeaders(string csvfile, string encodingname = "gbk")
        {
            Encoding encoding = Encoding.GetEncoding(encodingname);
            using (var fs = System.IO.File.Open(csvfile, FileMode.Open, FileAccess.Read, FileShare.ReadWrite)) {
                var sr = new StreamReader(fs, encoding);
                var header = sr.ReadLine().TrimEnd(' ',',');
                var headers = SplitCell(header);
                return headers;
            }
        }

        public static int WriteCsvFile(string csvfilename, IEnumerable<DoubleStringDictionary> lstDict, IEnumerable<string> headers, string encodingname = "gbk")
        {
            int count = 0;
            Encoding encoding = Encoding.GetEncoding(encodingname);
            using (var fs = System.IO.File.Open(csvfilename, FileMode.Create, FileAccess.Write)) {
                var sw = new StreamWriter(fs, encoding);
                StringBuilder sb = new StringBuilder();
                foreach (var h in headers) {
                    if (sb.Length > 0) {
                        sb.Append(",");
                    }
                    sb.Append(h);
                }
                sw.WriteLine(sb.ToString());
                sb.Clear();
                foreach(var dict in lstDict) {
                    foreach (var h in headers) {
                        if (sb.Length > 0) {
                            sb.Append(",");
                        }
                        sb.Append(Format(dict[h]));
                    }
                    sw.WriteLine(sb.ToString());
                    count++;
                    sb.Clear();
                }
                sw.Flush();
            }
            return count;
        }

        public static int AppendWriteCsvFile(string csvfilename, IEnumerable<DoubleStringDictionary> lstDict, IEnumerable<string> headers, string encodingname = "gbk")
        {
            int count = 0;
            Encoding encoding = Encoding.GetEncoding(encodingname);
            if (!System.IO.File.Exists(csvfilename)) {
                return -1;
            }
            using (var fs = System.IO.File.Open(csvfilename, FileMode.Append, FileAccess.Write)) {
                var sw = new StreamWriter(fs, encoding);
                StringBuilder sb = new StringBuilder();
                foreach (var dict in lstDict) {
                    foreach (var h in headers) {
                        if (sb.Length > 0) {
                            sb.Append(",");
                        }
                        sb.Append(Format(dict[h]));
                    }
                    sw.WriteLine(sb.ToString());
                    count++;
                    sb.Clear();
                }
                sw.Flush();
            }
            return count;
        }
        public static int WriteCsvFile(string csvfilename, IEnumerable<Dictionary<string,string>> lstDict, IEnumerable<string> headers, string encodingname = "gbk")
        {
            int count = 0;
            Encoding encoding = Encoding.GetEncoding(encodingname);
            using (var fs = System.IO.File.Open(csvfilename, FileMode.Create, FileAccess.Write)) {
                var sw = new StreamWriter(fs, encoding);
                StringBuilder sb = new StringBuilder();
                bool bWritedHeaders = false;
                foreach (var dict in lstDict) {
                    if(headers == null) {
                        headers = dict.Keys.ToArray();
                    }
                    if (!bWritedHeaders) {
                        foreach (var h in headers) {
                            if (sb.Length > 0) {
                                sb.Append(",");
                            }
                            sb.Append(h);
                        }
                        sw.WriteLine(sb.ToString());
                        sb.Clear();
                        bWritedHeaders = true;
                    }
                    foreach (var h in headers) {
                        if (sb.Length > 0) {
                            sb.Append(",");
                        }
                        if (dict.ContainsKey(h)) {
                            sb.Append(Format(dict[h]));
                        }
                    }
                    sw.WriteLine(sb.ToString());
                    count++;
                    sb.Clear();
                }
                sw.Flush();
            }
            return count;
        }
        //public static void WriteCsvFile(string csvfilename,DataTable dataTable, string encodingname = "gbk")
        //{
        //    Encoding encoding = Encoding.GetEncoding(encodingname);
        //    using (var fs = System.IO.File.Open(csvfilename, FileMode.Create, FileAccess.Write)) {
        //        var sw = new StreamWriter(fs, encoding);
        //        StringBuilder sb = new StringBuilder();
        //        var headers = new List<string>();
        //        foreach (DataColumn h in dataTable.Columns) {
        //            if (sb.Length > 0) {
        //                sb.Append(",");
        //            }
        //            sb.Append(h.ColumnName);
        //            headers.Add(h.ColumnName);
        //        }
        //        sw.WriteLine(sb.ToString());
        //        sb.Clear();
        //        foreach (DataRow row in dataTable.Rows) {
        //            foreach (var h in headers) {
        //                if (sb.Length > 0) {
        //                    sb.Append(",");
        //                }
        //                sb.Append(Format(row[h]));
        //            }
        //            sw.WriteLine(sb.ToString());
        //            sb.Clear();
        //        }
        //        sw.Flush();
        //    }
        //}
        private static List<string> SplitCell(string str, char splitChar = ',', char quotaChar = '\"')
        {
            List<string> lstResult = new List<string>();
            StringBuilder sb = new StringBuilder();
            bool quotaMode = false;
            bool bEnd = false;
            for (int i = 0; i < str.Length; i++) {
                char c = str[i];
                if (c == quotaChar) {
                    if (quotaMode) {
                        if (i < str.Length - 1) {
                            if (str[i + 1] == quotaChar) {
                                i += 1;
                                sb.Append(quotaChar);
                                continue;
                            }
                            if (str[i + 1] != splitChar) {
                                return null;
                            }
                            quotaMode = !quotaMode;
                            continue;
                        } else {
                            bEnd = true;
                        }
                    } else {
                        if (i > 0) {
                            if (str[i - 1] != splitChar) {
                                if (str[i - 1] != '=') {
                                    return null;
                                } else {
                                    sb.Clear();
                                }
                            }
                            quotaMode = !quotaMode;
                            continue;
                        } else {
                            quotaMode = !quotaMode;
                            continue;
                        }
                        quotaMode = !quotaMode;
                    }
                }

                if (c == splitChar && !quotaMode || bEnd ) {
                    lstResult.Add(sb.ToString().Trim());
                    sb.Clear();
                    continue;
                }
                sb.Append(c);
            }
            if (sb.Length != 0) {
                lstResult.Add(sb.ToString());
            }
            return lstResult;
        }
        private static string Format(object obj)
        {
            if(obj is bool) {
                return Format((bool)obj);
            } else if (obj is long?) {
                return Format((long?)obj);
            } else if (obj is decimal?) {
                return Format((decimal?)obj);
            } else {
                return Format(obj.ToString());
            }
        }
        private static string Format(string str)
        {
            if(str == null) {
                return "\"\"";
            }
            Regex regex = new Regex(@"^[\d\.,]+[%]?$");
            Regex regex2 = new Regex(@"^\d+[/]\d+$");
            if (regex.Match(str).Success || regex2.Match(str).Success) {
                return string.Format("=\"{0}\"", str.Replace("\"", "\"\""));
            }
            return string.Format("\"{0}\"", str.Replace("\"", "\"\""));
        }
        private static string Format(long? value)
        {
            if (value.HasValue) {
                return Format(value.Value.ToString());
            } else {
                return string.Empty;
            }
        }
        private static string Format(bool value)
        {
            return value ? "True" : "False";
        }
        private static string Format(decimal? value)
        {
            if (value.HasValue) {
                return Format(string.Format("{0:0.####}",value.Value));
            } else {
                return string.Empty;
            }
        }
    }
}
