using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;

namespace ChaosCore.ModelBase.Plugins
{
    public class SearchModel
    {
        [JsonProperty(PropertyName = "value")]
        public string Value { get; set; }
        [JsonProperty(PropertyName = "regex")]
        public bool Regex { get; set; } = false;
    }
    public enum OrderDir
    {
        asc,desc
    }
    public class OrderModel
    {
        [JsonProperty(PropertyName = "column")]
        public string Column { get; set; }
        [JsonProperty(PropertyName = "dir")]
        public OrderDir Dir { get; set; }
    }
    public class ColumnModel
    {
        public int Data { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
        public bool Searchable { get; set; }
        public bool Orderable { get; set; }
        public SearchModel Search { get; set; }
    }

    public class FilterModel
    {
        [JsonProperty(PropertyName = "column")]
        public string Column { get; set; }
        [JsonProperty(PropertyName = "value")]
        public string Value { get; set; }

    }
    public class QuickQueryModel
    {
        public QuickQueryModel() { }
        
        [JsonProperty(PropertyName = "draw")]
        public int Draw { get; set; } = 1;
        [JsonProperty(PropertyName = "start")]
        public int Start { get; set; } = 0;
        [JsonProperty(PropertyName = "length")]
        public int Length { get; set; } = 10;
        [JsonProperty(PropertyName = "order")]
        public List<OrderModel> Order { get; set; }
        [JsonProperty(PropertyName = "columns")]
        public List<ColumnModel> Columns { get; set; }
        [JsonProperty(PropertyName = "filters")]
        public List<FilterModel> Filters { get; set; }
        [JsonProperty(PropertyName = "search")]
        public SearchModel Search { get; set; }

    }
}