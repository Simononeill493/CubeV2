using System.Collections.Generic;
using System.Linq;

namespace CubeV2
{
    public static class VariableUtils
    {
        public const string DirectionVariableName = "Direction";
        public const string VariableVariableName = "Variable";
        public const string TemplateVariableName = "Template";
        public const string IntegerVariableName = "Integer";
        public const string RandomVariableName = "Random";


        public static IEnumerable<IVariableType> GetAllVariableTypes() => typeof(IVariableType).GetEnumValues().Cast<IVariableType>();

        public static List<VariableCategory> GetAllVariableCategories() => _variableCategories;
        private static List<VariableCategory> _variableCategories;

        public static void Init()
        {
            _variableCategories = new List<VariableCategory>();

            _variableCategories.Add(new VariableCategory() { Name = DirectionVariableName}); 
            _variableCategories.Add(new VariableCategory() { Name = VariableVariableName });
            _variableCategories.Add(new VariableCategory() { Name = TemplateVariableName });
            _variableCategories.Add(new VariableCategory() { Name = IntegerVariableName });
            _variableCategories.Add(new VariableCategory() { Name = RandomVariableName });
        }
    }
}
