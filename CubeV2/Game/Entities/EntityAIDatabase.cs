using CubeV2.CubeV2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CubeV2;

internal class EntityAIDatabase
{
    public static void Load()
    {
        EntityDatabase.Get(EntityDatabase.ManualPlayerName).MakeInstructable();
        EntityDatabase.Get(EntityDatabase.AutoPlayerName).MakeInstructable();

        EntityDatabase.Get(EntityDatabase.Ally1Name).MakeInstructable();
        EntityDatabase.Get(EntityDatabase.Ally2Name).MakeInstructable();
        EntityDatabase.Get(EntityDatabase.Ally3Name).MakeInstructable();
        EntityDatabase.Get(EntityDatabase.Ally4Name).MakeInstructable();
        EntityDatabase.Get(EntityDatabase.Ally5Name).MakeInstructable();

        EntityDatabase.Get(EntityDatabase.TurretName).MakeInstructable();
        EntityDatabase.Get(EntityDatabase.MissileName).MakeInstructable();




        //EntityDatabase.Get(EntityDatabase.AutoPlayerName).Instructions[0][0] = new MoveInstruction(RelativeDirection.Forward);

        TurretSetInstructions(EntityDatabase.Get(EntityDatabase.TurretName));
        MissileSetInstructions(EntityDatabase.Get(EntityDatabase.MissileName));


    }

    private static void TurretSetInstructions(EntityTemplate turret)
    {
        var find = new PingRangeInstruction(EntityDatabase.Get(EntityDatabase.ManualPlayerName), 3);
        find.OutputTargetVariables[0] = 0;
        find.IndexFound = 1;
        find.IndexNotFound = 3;

        var turn = new TurnInstruction(new StoredVariableVariable(0));
        var shoot = new CreateInstruction(RelativeDirection.Forward,EntityDatabase.Get(EntityDatabase.MissileName));

        turret.Instructions[0][0] = find;
        turret.Instructions[0][1] = turn;
        turret.Instructions[0][2] = shoot;
    }

    private static void MissileSetInstructions(EntityTemplate missile)
    {
        var scan = new PushScanInstruction(RelativeDirection.Forward);
        scan.OutputTargetVariables[0] = 0;

        var actOnScan = new IfInstruction(new StoredVariableVariable(0), new EntityTypeVariable(EntityDatabase.Get(EntityDatabase.ManualPlayerName)));
        actOnScan.IndexTrue = 5;
        actOnScan.IndexFalse = 2;

        var find = new PingRangeInstruction(EntityDatabase.Get(EntityDatabase.ManualPlayerName), 3);
        find.OutputTargetVariables[0] = 1;
        find.IndexFound = 3;
        find.IndexNotFound = 6;

        var turn = new TurnInstruction(new StoredVariableVariable(1));
        var move = new MoveInstruction(RelativeDirection.Forward);
        move.ControlFlowOutputs[0] = 6;

        var explode = new DestroySelfInstruction();

        missile.Instructions[0][0] = scan;
        missile.Instructions[0][1] = actOnScan;
        missile.Instructions[0][2] = find;
        missile.Instructions[0][3] = turn;
        missile.Instructions[0][4] = move;
        missile.Instructions[0][5] = explode;
    }

}


