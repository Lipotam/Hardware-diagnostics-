using System.Collections.Generic;

namespace HardwareControl
{
    class ModelingSetsComparer : IEqualityComparer<ModelingSet>
    {
        public bool Equals(ModelingSet x, ModelingSet y)
        {
            return x.ToString().Equals(y.ToString());
        }

        public int GetHashCode(ModelingSet obj)
        {
            return obj.ToString().GetHashCode();
        }
    }
}
