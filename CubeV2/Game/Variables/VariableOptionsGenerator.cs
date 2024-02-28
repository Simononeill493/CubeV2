using SAME;
using System;
using System.Collections.Generic;

namespace CubeV2
{
    public static class VariableOptionsGenerator
    {
        //public static List<IVariable> GetAllVariableOptions() => GetVariableOptions(VariableUtils.GetAllVariableTypes());

        public static List<VariableCategory> GetAllVariableCategories() => VariableUtils.GetAllVariableCategories();


        /*public static List<IVariable> GetVariableOptions(IEnumerable<IVariableType> variableTypes)
        {
            var options = new List<IVariable>();

            foreach (IVariableType variableType in variableTypes)
            {
                options.AddRange(GetVariableOptions(variableType));
            }

            return options;
        }*/

        public static List<IVariable> GetVariableOptions(VariableCategory variableCategory)
        {
            var options = new List<IVariable>();

            if (variableCategory.Name == VariableUtils.DirectionVariableName)
            {
                foreach (var i in DirectionUtils.Relatives)
                {
                    options.Add(new RelativeDirectionVariable(i));
                }

            }
            else if (variableCategory.Name == VariableUtils.VariableVariableName)
            {
                for (int i = 0; i < Config.InstructionMaxNumVariables; i++)
                {
                    options.Add(new StoredVariableVariable(i));
                }
            }
            else if (variableCategory.Name == VariableUtils.TemplateVariableName)
            {
                foreach (var template in EntityDatabase.GetAll())
                {
                    options.Add(new EntityTypeVariable(template));
                }
            }
            else if (variableCategory.Name == VariableUtils.RandomVariableName)
            {
                options.Add(new RandomDirectionVariable());
            }



            return options;
        }

    }
}
