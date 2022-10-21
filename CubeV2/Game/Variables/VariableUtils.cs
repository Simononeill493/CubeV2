using System.Collections.Generic;
using System.Linq;

namespace CubeV2
{
    public static class VariableUtils
    {
        public static IEnumerable<IVariableType> GetAllVariableTypes() => typeof(IVariableType).GetEnumValues().Cast<IVariableType>();
    }
}
