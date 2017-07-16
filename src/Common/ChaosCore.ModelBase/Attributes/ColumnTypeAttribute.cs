using Labmem.EntityFrameworkCorePlus.Attributes;

namespace ChaosCore.ModelBase.Attributes
{
    public class ColumnTypeAttribute : PropertyAttribute
    {
        public string ColumnType { get; set; }
        public ColumnTypeAttribute(string columntype)
        {
            ColumnType = columntype;
        }

        //public override PrimitivePropertyConfiguration OnPropertyConfiguration(PrimitivePropertyConfiguration configuration)
        //{
        //    return configuration.HasColumnType(ColumnType);
        //}
    }
}
