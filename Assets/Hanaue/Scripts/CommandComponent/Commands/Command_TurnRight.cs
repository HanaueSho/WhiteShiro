/*
    Command_TurnRight.cs
    20260728  hanaue sho
    コマンド右旋回
*/
using UnityEngine;


[CreateAssetMenu(fileName = "Command_TurnRight", menuName = "Scriptable Objects/Command/TurnRight")]
public class Command_TurnRight : CommandComponent 
{
    public override bool Command(CommandPlayer owner)
    {
        base.Command(owner);

        Transform block = owner?.transform;
        if (block != null)
        {
            block.RotateAround(block.position, Vector3.up, 90.0f);
        }

        return true;
    }
}
