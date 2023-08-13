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
        EntityDatabase.Get(EntityDatabase.ManualPlayerName).DefaultUpdateRate = 1;

        EntityDatabase.Get(EntityDatabase.AutoPlayerName).MakeInstructable();

        EntityDatabase.Get(EntityDatabase.Ally1Name).MakeInstructable();
        EntityDatabase.Get(EntityDatabase.Ally2Name).MakeInstructable();
        EntityDatabase.Get(EntityDatabase.Ally3Name).MakeInstructable();
        EntityDatabase.Get(EntityDatabase.Ally4Name).MakeInstructable();
        EntityDatabase.Get(EntityDatabase.Ally5Name).MakeInstructable();

        EntityDatabase.Get(EntityDatabase.TurretName).MakeInstructable();
        EntityDatabase.Get(EntityDatabase.MissileName).MakeInstructable();




        //EntityDatabase.Get(EntityDatabase.AutoPlayerName).Instructions[0][0] = new MoveInstruction(RelativeDirection.Forward);

        TurretSetAI(EntityDatabase.Get(EntityDatabase.TurretName));
        MissileSetAI(EntityDatabase.Get(EntityDatabase.MissileName));


    }

    private static void TurretSetAI(EntityTemplate turret)
    {
        turret.DefaultUpdateRate = 40;

        var find = new PingRangeInstruction(EntityDatabase.Get(EntityDatabase.ManualPlayerName), 8);
        find.OutputTargetVariables[0] = 0;
        find.IndexFound = 1;
        find.IndexNotFound = 3;

        var turn = new TurnInstruction(new StoredVariableVariable(0));
        var shoot = new CreateInstruction(RelativeDirection.Forward,EntityDatabase.Get(EntityDatabase.MissileName));

        turret.Instructions[0][0] = find;
        turret.Instructions[0][1] = turn;
        turret.Instructions[0][2] = shoot;
    }

    private static void MissileSetAI(EntityTemplate missile)
    {
        missile.DefaultUpdateRate = 3;

        var actOnAge = new IfInstruction(new AgeVariable(), new IntegerVariable(25));
        actOnAge.Operator = IOperator.MoreThan;
        actOnAge.IndexTrue = 6;
        actOnAge.IndexFalse = 1;

        var scan = new PushScanInstruction(RelativeDirection.Forward);
        scan.OutputTargetVariables[0] = 0;

        var actOnScan = new IfInstruction(new StoredVariableVariable(0), new DummyVariable());
        actOnScan.Operator = IOperator.NotEmpty;
        actOnScan.IndexTrue = 6;
        actOnScan.IndexFalse = 3;

        var find = new PingRangeInstruction(EntityDatabase.Get(EntityDatabase.ManualPlayerName), 6);
        find.OutputTargetVariables[0] = 1;
        find.IndexFound = 4;
        find.IndexNotFound = 5;

        var turn = new TurnInstruction(new StoredVariableVariable(1));
        var move = new MoveInstruction(RelativeDirection.Forward);
        move.ControlFlowOutputs[0] = 7;

        var explode = new ExplodeInstruction();



        missile.Instructions[0][0] = actOnAge;
        missile.Instructions[0][1] = scan;
        missile.Instructions[0][2] = actOnScan;
        missile.Instructions[0][3] = find;
        missile.Instructions[0][4] = turn;
        missile.Instructions[0][5] = move;
        missile.Instructions[0][6] = explode;
    }

}


