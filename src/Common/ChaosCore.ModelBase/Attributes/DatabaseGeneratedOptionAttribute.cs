using Labmem.EntityFrameworkCorePlus.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace ChaosCore.ModelBase.Attributes
{
    public class DatabaseGeneratedOptionAttribute: PropertyAttribute
    {
        public DatabaseGeneratedOption? DatabaseGeneratedOption { get; set; }
        public DatabaseGeneratedOptionAttribute(DatabaseGeneratedOption databaseGeneratedOption)
        {
            DatabaseGeneratedOption = databaseGeneratedOption;
        }

        //public override PrimitivePropertyConfiguration OnPropertyConfiguration(PrimitivePropertyConfiguration configuration)
        //{
        //    return configuration.HasDatabaseGeneratedOption(DatabaseGeneratedOption);
        //}
    }
}
