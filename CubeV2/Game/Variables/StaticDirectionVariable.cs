using System.Collections.Generic;
using System.Linq;

namespace CubeV2
{
    public class StaticDirectionVariable : IVariable
    {
        public override IVariableType DefaultType => IVariableType.Direction;
        public override List<IVariableType> ValidTypes { get; } = new List<IVariableType>() { IVariableType.Direction };

        public RelativeDirection Direction { get; }

        public StaticDirectionVariable(RelativeDirection direction)
        {
            Direction = direction;
        }

        public override object Convert(Entity caller,IVariableType variableType)
        {
            switch (variableType)
            {
                case IVariableType.Direction:
                    return Direction;
                default:
                    return null;
            }
        }
    }

    public class StoredVariableVariable : IVariable
    {
        public override IVariableType DefaultType => IVariableType.StoredVariable;
        public override List<IVariableType> ValidTypes { get; } = VariableUtils.GetAllTypes().ToList();

        public int VariableIndex { get; }

        public StoredVariableVariable(int variableIndex)
        {
            VariableIndex = variableIndex;
        }

        public override object Convert(Entity caller,IVariableType variableType)
        {
            var targetVariable = caller.Variables[VariableIndex];
            if(targetVariable!=null)
            {
                return targetVariable.Convert(caller,variableType);
            }

            return null;
        }
    }
}
