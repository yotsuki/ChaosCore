using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChaosCore.ModelBase.Extensions
{
    /// <summary>
    /// 类型转换器
    /// </summary>
    public static class TypeConvertor
    {
        public static object BoolConvertToBit(object target)
        {
            if (target == null || String.IsNullOrEmpty(target.ToString()))
                return 0;
            string t = target.ToString();
            try
            {
                return t.Equals("1") || t.ToLower().Equals("true") || t.ToLower().Equals("yes") || t.ToLower().Equals("on")
                   ? 1 : 0;
            }
            catch (Exception ex)
            {
                throw new ArgumentException("类型不匹配", ex);
            }
        }
        public static object Convert(object target, Type targetType)
        {
            if (target == null || targetType == null || String.IsNullOrEmpty(target.ToString()))
                return GetDefault(targetType);
            string t = target.ToString();
            try
            {
                //TODO: Mono not suppot type.Equals
                var targetTypeName = targetType.FullName;
                if (targetTypeName.Equals(typeof(String).FullName))
                    return target.ToString();
                else if (targetTypeName.Equals(typeof(int).FullName))
                    return Int32.Parse(t);
                else if (targetTypeName.Equals(typeof(DateTime).FullName))
                    return DateTime.Parse(t);
                else if (targetTypeName.Equals(typeof(bool).FullName))
                    return t.Equals("1") || t.ToLower().Equals("true") || t.ToLower().Equals("yes") || t.ToLower().Equals("on")
                        ? true : false;
                else if (targetTypeName.Equals(typeof(byte[]).FullName))
                    return (byte[])target;
                else if (targetTypeName.Equals(typeof(decimal).FullName))
                    return Decimal.Parse(t);
                else if (targetTypeName.Equals(typeof(double).FullName))
                    return Double.Parse(t);
                else
                    throw new ArgumentException("类型不匹配");
            }
            catch (Exception ex)
            {
                throw new ArgumentException("类型不匹配", ex);
            }
        }

        /// <summary>
        /// 根据类型定义返回默认值
        /// </summary>
        /// <param name="targetType"></param>
        /// <returns></returns>
        public static object GetDefault(Type targetType)
        {
            try
            {
                if (targetType.Equals(typeof(String)))
                    return string.Empty;
                else if (targetType.Equals(typeof(byte)))
                    return 0;
                else if (targetType.Equals(typeof(byte?)))
                    return new byte?();
                else if (targetType.Equals(typeof(short)))
                    return 0;
                else if (targetType.Equals(typeof(short?)))
                    return new short?();
                else if (targetType.Equals(typeof(int)))
                    return 0;
                else if (targetType.Equals(typeof(int?)))
                    return new int?();
                else if (targetType.Equals(typeof(long)) || targetType.Equals(typeof(long?)))
                    return 0L;
                else if (targetType.Equals(typeof(DateTime)))
                    return new DateTime(2001, 9, 11);
                else if (targetType.Equals(typeof(DateTime?)))
                    return new DateTime?();
                else if (targetType.Equals(typeof(bool)))
                    return false;
                else if (targetType.Equals(typeof(bool?)))
                    return new bool?();
                else if (targetType.Equals(typeof(byte[])))
                    return new byte[] { };
                else if (targetType.Equals(typeof(decimal)))
                    return 0.0M;
                else if (targetType.Equals(typeof(decimal?)))
                    return new decimal?();
                else if (targetType.Equals(typeof(double)))
                    return 0.0D;
                else if (targetType.Equals(typeof(double?)))
                    return new double?();
                else if (targetType.Equals(typeof(float)))
                    return 0.0f;
                else if (targetType.Equals(typeof(float?)))
                    return new float?();
                else
                    throw new ArgumentException("类型不匹配");
            }
            catch (Exception ex)
            {
                throw new ArgumentException("类型不匹配", ex);
            }
        }

        /// <summary>
        /// 根据类型字符串返回默认值
        /// </summary>
        /// <param name="typeStirng"></param>
        /// <returns></returns>
        public static object GetDefault(string typeStirng)
        {
            if (String.IsNullOrEmpty(typeStirng))
                throw new ArgumentException("类型参数错误.");
            Type type = Type.GetType(typeStirng);
            return GetDefault(type);
        }
    }
}
