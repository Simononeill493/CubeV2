using CubeV2.CubeV2;
using SAME;
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
        EntityDatabase.Get(EntityDatabase.ManualPlayerName).DefaultUpdateRate = 2;

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
        turret.DefaultUpdateRate = 15;
        turret.AddDefaultVariable(0, IVariableType.Integer, 8);

        var find = new PingRangeInstruction(new EntityTypeVariable(EntityDatabase.Get(EntityDatabase.ManualPlayerName)), new StoredVariableVariable(0));
        find.OutputTargetVariables[0] = 1;
        find.IndexFound = 1;
        find.IndexNotFound = 3;

        var turn = new TurnInstruction(new StoredVariableVariable(1));
        var shoot = new CreateInstruction(RelativeDirection.Forward, EntityDatabase.Get(EntityDatabase.MissileName));

        turret.Instructions[0][0] = find;
        turret.Instructions[0][1] = turn;
        turret.Instructions[0][2] = shoot;
    }

    private static void MissileSetAI(EntityTemplate missile)
    {
        missile.DefaultUpdateRate = 3;

        var actOnAge = new IfInstruction(new AgeVariable(), new IntegerVariable(25));
        actOnAge.Operator = IOperator.MoreThan;
        actOnAge.IndexTrue = 8;
        actOnAge.IndexFalse = 1;

        var scan1 = new PushScanInstruction(RelativeDirection.Forward);
        scan1.OutputTargetVariables[0] = 0;

        var actOnScan1 = new IfInstruction(new StoredVariableVariable(0), new DummyVariable());
        actOnScan1.Operator = IOperator.NotEmpty;
        actOnScan1.IndexTrue = 8;
        actOnScan1.IndexFalse = 3;

        var find = new PingRangeInstruction(EntityDatabase.Get(EntityDatabase.ManualPlayerName), 6);
        find.OutputTargetVariables[0] = 1;
        find.IndexFound = 4;
        find.IndexNotFound = 5;

        var turn = new TurnInstruction(new StoredVariableVariable(1));

        var scan2 = new PushScanInstruction(RelativeDirection.Forward);
        scan2.OutputTargetVariables[0] = 2;

        var actOnScan2 = new IfInstruction(new StoredVariableVariable(2), new DummyVariable());
        actOnScan2.Operator = IOperator.NotEmpty;
        actOnScan2.IndexTrue = 8;
        actOnScan2.IndexFalse = 7;

        var move = new MoveInstruction(RelativeDirection.Forward);
        move.ControlFlowOutputs[0] = 9;

        var explode = new ExplodeInstruction();

        missile.Instructions[0][0] = actOnAge;
        missile.Instructions[0][1] = scan1;
        missile.Instructions[0][2] = actOnScan1;
        missile.Instructions[0][3] = find;
        missile.Instructions[0][4] = turn;
        missile.Instructions[0][5] = scan2;
        missile.Instructions[0][6] = actOnScan2;
        missile.Instructions[0][7] = move;
        missile.Instructions[0][8] = explode;
    }

}


