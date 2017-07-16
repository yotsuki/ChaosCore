using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChaosCore.ModelBase.Extensions
{
    public class HashGroup<TKey,TModel>: Dictionary<TKey, List<TModel>>
    {
        public void AddModel(TKey key,TModel model)
        {
            if (base.ContainsKey(key)) {
                base[key].Add(model);
            } else {
                var list = new List<TModel>();
                list.Add(model);
                base.Add(key, list);
            }
        }
    }
}
