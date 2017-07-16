using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;

namespace ChaosCore.CommonLib
{
    public class JsonHelper
    {
        public static T JsonToModel<T>(string jsonString)
        {
            ////将"yyyy-MM-dd HH:mm:ss"格式的字符串转为"\/Date(1294499956278+0800)\/"格式 

            //string p = @"\d{4}-\d{2}-\d{2}\s\d{2}:\d{2}:\d{2}";

            //MatchEvaluator matchEvaluator = new MatchEvaluator(ConvertDateStringToJsonDate);

            //Regex reg = new Regex(p);

            //jsonString = reg.Replace(jsonString, matchEvaluator);

            DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(T));

            MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(jsonString));

            T obj = (T)ser.ReadObject(ms);

            return obj;  
        }

        public static void WriteModelToFile<T>(string filepath,T model) {
            DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(T));

            using (var fs = System.IO.File.Create(filepath)) {
                ser.WriteObject(fs, model);
                fs.Flush();
            }
            //System.IO.File.Encrypt(filepath);
        }

        public static void WriteModelToFile(string filepath,Type type, object model)
        {
            DataContractJsonSerializer ser = new DataContractJsonSerializer(type);
            using (var fs = System.IO.File.Create(filepath)) {
                ser.WriteObject(fs, model);
                fs.Flush();
            }
            //System.IO.File.Encrypt(filepath);
        }
        public static T LoadModelFormFile<T>(string filepath)
        {
            DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(T));
            using (var fs = System.IO.File.Open(filepath,FileMode.Open)) {
                T obj = (T)ser.ReadObject(fs);
                return obj;
            }
        }

        public static object LoadModelFormFile(string filepath,Type type)
        {
            DataContractJsonSerializer ser = new DataContractJsonSerializer(type);
            using (var fs = System.IO.File.Open(filepath, FileMode.Open)) {
                var obj = ser.ReadObject(fs);
                return obj;
            }
        }
    }
}
