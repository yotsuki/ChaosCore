using ChaosCore.ModelBase;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;

namespace ChaosCore.ModelBase.Plugins
{
    public class QueryResultModel
    {
        [JsonProperty(PropertyName = "draw")]
        public int Draw { get; set; }
        /// <summary>
        /// 总记录数
        /// </summary>
        [JsonProperty(PropertyName = "recordsTotal")]
        public int RecordsTotal { get; set; }
        /// <summary>
        /// 筛选后记录数
        /// </summary>
        [JsonProperty(PropertyName = "recordsFiltered")]
        public int RecordsFiltered { get; set; }
        [JsonProperty(PropertyName = "data")]
        public string[][] Data { get; set; }
        [JsonProperty(PropertyName = "error")]
        public string Error { get; set; } = string.Empty;

        public static QueryResultModel FormRequest<T>(QuickQueryModel model, IQueryable<T> items, ResultMapping<T> mapping) where T:BaseEntity
        {
            if (model == null) {
                return null;
            }
            var result = new QueryResultModel();
            result.Draw = model.Draw;
            result.RecordsTotal = items.Select(e=>e.CreateTime).Count();
            var filtereditems = items;
            foreach(var filter in model.Columns.Where(c => c.Searchable && !string.IsNullOrEmpty(c.Search.Value))) {
                filtereditems = mapping.Where(filtereditems, filter.Name, filter.Search.Value);
            }
            if (model.Filters != null) {
                foreach (var filter in model.Filters) {
                    filtereditems = mapping.Where(filtereditems, filter.Column, filter.Value);
                }
            }
            result.RecordsFiltered = filtereditems.Select(e => e.CreateTime).Count();
            foreach(var order in model.Order) {
                var ordername = order.Column;//model.Columns[order.Column].Name;
                filtereditems = (order.Dir == OrderDir.asc) ? filtereditems.OrderBy(mapping.GetTextFunc(ordername)) : filtereditems.OrderByDescending(mapping.GetTextFunc(ordername));
            }
            //select
            mapping.Select(filtereditems);

            var pageditems = (model.Length == -1 ? filtereditems : (model.Start == 0 ? filtereditems : filtereditems.Skip(model.Start)).Take(model.Length)).ToArray();
            //var datas = new List<string[]>();
            //foreach (var data in pageditems) {
            //    var values = new List<string>();
            //    foreach (var column in model.Columns) {
            //        if (string.IsNullOrEmpty(column.Name)) {
            //            values.Add(string.Empty);
            //        } else {
            //            values.Add(mapping.GetHtml(data, column.Name) ?? string.Empty);
            //        }
            //    }
            //    datas.Add(values.ToArray());
            //}
            //result.Data = datas.ToArray();
            return result;
        }
    }
    
}