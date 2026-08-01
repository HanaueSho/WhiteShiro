/*
    Command_Turn_Right90.cs
    20260801  hanaue sho
    コマンド右旋回９０度
*/
using UnityEngine;


[CreateAssetMenu(fileName = "Command_Turn_Right90", menuName = "Scriptable Objects/Command/Turn_Right90")]
public class Command_Turn_Right90 : Command_Turn
{
    // ==================================================
    // ----- Unity Event -----
    // ==================================================
    private void OnEnable()
    {
        _visualText = "右回りしろ";
    }


    public override void Enter(CommandPlayer owner)
    {
        _turnAngle = 90.0f;

        base.Enter(owner);
    }
}
