﻿using System;
using System.Collections.Generic;

namespace CubeV2
{
    public static class VariableOptionsGenerator
    {
        public static List<IVariable> GetAllVariableOptions() => GetVariableOptions(VariableUtils.GetAllVariableTypes());

        public static List<IVariable> GetVariableOptions(IEnumerable<IVariableType> variableTypes)
        {
            var options = new List<IVariable>();

            foreach (IVariableType variableType in variableTypes)
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
                case IVariableType.RelativeDirection:
                    foreach (var i in DirectionUtils.Relatives)
                    {
                        options.Add(new RelativeDirectionVariable(i));
                    }
                    options.Add(new RandomDirectionVariable());
                    break;
                case IVariableType.StoredVariable:
                    for(int i=0;i<Config.InstructionMaxNumVariables;i++)
                    {
                        options.Add(new StoredVariableVariable(i));
                    }
                    break;
                case IVariableType.EntityType:
                    foreach(var template in EntityDatabase.GetAll())
                    {
                        options.Add(new EntityTypeVariable(template));
                    }
                    break;
                case IVariableType.RotationDirection:
                    options.Add(new RotationDirectionVariable(RotationDirection.Left));
                    options.Add(new RotationDirectionVariable(RotationDirection.Right));
                    break;
                case IVariableType.Integer:
                case IVariableType.Orientation:
                case IVariableType.CardinalDirection:
                case IVariableType.Tile:
                case IVariableType._null:
                    break;
                default:
                    throw new Exception("Make sure all variable types are handled");
            }

            return options;
        }

    }
}