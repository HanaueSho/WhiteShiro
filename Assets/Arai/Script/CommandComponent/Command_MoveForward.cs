/*
    CommandMoveForward.cs
    20260728  arai eito
    コマンド前方移動
*/
using UnityEngine;

[CreateAssetMenu(fileName = "Command_MoveForward", menuName = "Scriptable Objects/Command/MoveForward")]
public class Command_MoveForward : CommandComponent
{
    public override bool Command(CommandPlayer owner)
    {
        base.Command(owner);

        Transform block = owner?.transform;
        if(block != null )
        {
            block.position += block.forward;
        }

        return true;
    }
}
