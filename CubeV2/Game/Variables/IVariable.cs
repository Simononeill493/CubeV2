using System.Collections.Generic;

namespace CubeV2
{
    public abstract class IVariable
    {
        public abstract IVariableType DefaultType { get; }
        public abstract List<IVariableType> ValidTypes { get; }

        public abstract object Convert(Entity caller,IVariableType variableType);
    }
}
