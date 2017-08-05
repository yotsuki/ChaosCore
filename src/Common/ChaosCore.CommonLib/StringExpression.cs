using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Text.RegularExpressions;

namespace ChaosCore.CommonLib
{
    public class StringExpression
    {
        private int groupid = 0;
        private Dictionary<string, string> mapSubString = new Dictionary<string, string>();
        private static readonly Regex s_regex = new Regex(@"(?<k>\w+)(?<e>[!]?[\=])(?<v>(""[\s\S]+""|\d+))");
        public Expression<Func<T, bool>> GetBooleanExpression<T>(string exprString, string paraname = "_")
        {
            var expPara = Expression.Parameter(typeof(T), paraname);
            
            var binary = GetBooleanExpression<T>(exprString, expPara);
            var lambda = Expression.Lambda<Func<T, bool>>(binary, expPara);
            return lambda;
        }

        private BinaryExpression GetBooleanExpression<T>(string exprString, ParameterExpression paraExpr)
        {
            BinaryExpression expr = null;
            if (exprString.Contains("(")) {
                var tmp = exprString.Trim();
                var sb = new StringBuilder(tmp);
                
                var left_offset = tmp.IndexOf('('); ;
                var right_offset = left_offset;
                var right_start = left_offset;
                while (left_offset != -1) {
                    right_offset = tmp.IndexOf(')', right_start);
                    var otherleft_offset = tmp.Substring(left_offset + 1, right_offset - left_offset).IndexOf('(');
                    if (otherleft_offset == -1) {
                        var substr = tmp.Substring(left_offset + 1, right_offset - left_offset - 1);
                        var key = $"$g{groupid++}";
                        mapSubString.Add(key, substr);
                        sb.Replace(tmp.Substring(left_offset, right_offset - left_offset+1), key);
                        left_offset = tmp.IndexOf('(', right_offset + 1);
                        right_start = left_offset;
                    } else {
                        right_start = right_offset + 1;
                    }
                }
                return GetBooleanExpression<T>(sb.ToString(), paraExpr);
            } else if(exprString.Contains("|")) {
                expr = OrExpression<T>(exprString.Split('|'), paraExpr);
            } else if (exprString.Contains("&")) {
                expr = AndExpression<T>(exprString.Split('&'), paraExpr);
            } else if (exprString[0]=='!') {
                var substr = exprString.Substring(1, exprString.Length - 1);
                var subExpr = GetBooleanExpression<T>(substr, paraExpr);
                Expression.Not(subExpr);
            } else {
                if (mapSubString.ContainsKey(exprString)) {
                    return GetBooleanExpression<T>(mapSubString[exprString], paraExpr);
                } else {
                    var match = s_regex.Match(exprString);
                    if (match.Success) {
                        var key = match.Groups["k"].Value;
                        var strValue = match.Groups["v"].Value;
                        object value = null;
                        if (strValue[0] == '\"' && strValue[strValue.Length - 1] == '\"') {
                            value = strValue.Length == 2 ? string.Empty : strValue.Substring(1, strValue.Length - 2);
                        } else {
                            value = long.Parse(strValue);
                        }
                        var equalValue = match.Groups["e"].Value;
                        var expProperty = Expression.Property(paraExpr, key);
                        var equal = equalValue == "=" ? Expression.Equal(expProperty, Expression.Constant(value))
                                                    : Expression.NotEqual(expProperty, Expression.Constant(value));
                        return equal;
                    }
                }
            }

            return expr;
        }

        private BinaryExpression OrExpression<T>(string[] subs,ParameterExpression paraExpr)
        {
            BinaryExpression expr = null;
            foreach (var sub in subs) {
                if (expr == null) {
                    expr = GetBooleanExpression<T>(sub, paraExpr);
                } else {
                    var subExpr = GetBooleanExpression<T>(sub, paraExpr);
                    expr = Expression.Or(expr, subExpr);
                }
            }
            return expr;
        }

        private BinaryExpression AndExpression<T>(string[] array, ParameterExpression paraExpr)
        {
            BinaryExpression expr = null;
            foreach (var sub in array) {
                if (expr == null) {
                    expr = GetBooleanExpression<T>(sub, paraExpr);
                } else {
                    var subExpr = GetBooleanExpression<T>(sub, paraExpr);
                    expr = Expression.And(expr, subExpr);
                }
            }
            return expr;
        }
    }
}
