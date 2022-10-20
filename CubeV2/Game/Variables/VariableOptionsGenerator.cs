using System;
using System.Collections.Generic;

namespace CubeV2
{
    public static class VariableOptionsGenerator
    {
        public static List<IVariable> GetAllVariableOptions()
        {
            var options = new List<IVariable>();

            foreach (IVariableType variableType in VariableUtils.GetAllTypes())
            {
                options.AddRange(GetVariableOptions(variableType));
            }

            return options;
        }

        public static List<IVariable> GetVariableOptions(IVariableType variableType)
        {
            var options = new List<IVariable>();

            switch (variableType)
            {
                case IVariableType.Direction:
                    foreach (var i in DirectionUtils.Relatives)
                    {
                        options.Add(new StaticDirectionVariable(i));
                    }
                    break;
                case IVariableType.StoredVariable:
                    for(int i=0;i<Config.InstructionMaxNumVariables;i++)
                    {
                        options.Add(new StoredVariableVariable(i));
                    }
                    break;
                case IVariableType.EntityType:
                    break;
                default:
                    throw new Exception("Make sure all variable types are handled");
            }

            return options;
        }

    }
}
