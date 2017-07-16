using System;

namespace ChaosCore.ModelBase.Attributes
{
    public abstract class AutoValueAttribute: Attribute 
    {
        public BaseEntity Entity { get; set; }

        public abstract object OnUpdate();

        public abstract object OnCreate();

    }
}
