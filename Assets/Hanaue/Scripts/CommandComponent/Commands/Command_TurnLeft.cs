/*
    Command_TurnLeft.cs
    20260728  hanaue sho
    コマンド左旋回
*/
using UnityEngine;


[CreateAssetMenu(fileName = "Command_TurnLeft", menuName = "Scriptable Objects/Command/TurnLeft")]
public class Command_TurnLeft : CommandComponent 
{
    public override bool Command(CommandPlayer owner)
    {
        base.Command(owner);

        Transform block = owner?.transform;
        if (block != null)
        {
            block.RotateAround(block.position, Vector3.up, -90.0f);
        }

        return true;
    }
}
