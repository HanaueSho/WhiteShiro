/*
    CommandMoveForward.cs
    20260728  arai eito
    コマンド前方移動
*/
using UnityEngine;

public class Command_MoveForward : CommandComponent
{
    public override bool Command()
    {
        base.Command();

        Transform block = GetComponentInParent<Block>()?.transform;
        if(block != null )
        {
            block.position += transform.forward;
        }

        return true;
    }
}
