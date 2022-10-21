﻿using System;
using System.Collections.Generic;
using System.Collections.Specialized;

namespace CubeV2
{
    public class Entity
    {
        public string TemplateID { get; }
        public string EntityID { get; }

        public string Sprite;

        public Orientation Orientation;
        public Vector2Int Location = Vector2Int.MinusOne;

        public List<Instruction> Instructions = new List<Instruction>();
        public int InstructionCounter;

        public IVariable[] Variables = new IVariable[Config.InstructionMaxNumVariables];

        public List<string> Tags = new List<string>();
        public bool HasTag(string tag) => Tags.Contains(tag);

        public Entity(string templateID, string entityID, string sprite)
        {
            TemplateID = templateID;
            EntityID = entityID;
            Sprite = sprite;
        }

        public void Tick(Board currentBoard)
        {
            for (int InstructionCounter = 0; InstructionCounter < Instructions.Count; InstructionCounter++)
            {
                var currentInstruction = Instructions[InstructionCounter];
                currentInstruction.Run(this, currentBoard);
                
                for(int i=0;i<currentInstruction.OutputCount;i++)
                {
                    if (currentInstruction.OutputTargets[i] >= 0)
                    {
                        var output = currentInstruction.Outputs[i];
                        if(output.DefaultType==IVariableType.StoredVariable)
                        {
                            //We can't store variable references in variables yet. causes overflow. too meta lol
                            continue;
                        }

                        Variables[currentInstruction.OutputTargets[i]] = output;
                    }
                }
            }
        }

        public void TryMove(RelativeDirection direction) => TryMove(DirectionUtils.ToCardinal(Orientation, direction));

        public void TryMove(CardinalDirection direction)
        {
            var newLocation = Location + direction.XYOffset();
            GameInterface.TryMoveEntity(this, newLocation);
        }

        public void Rotate(int rotation)
        {
            Orientation = Orientation.Rotate(rotation);
        }

        public void SetOrientation(Orientation orientation)
        {
            Orientation = orientation;
        }


        public virtual bool TryBeCollected(Entity collector) => false;
    }
}
