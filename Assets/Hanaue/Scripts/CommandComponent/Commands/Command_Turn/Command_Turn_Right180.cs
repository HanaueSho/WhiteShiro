/*
    Command_Turn_Right180.cs
    20260801  hanaue sho
    コマンド右旋回１８０度
*/
using UnityEngine;


[CreateAssetMenu(fileName = "Command_Turn_Right180", menuName = "Scriptable Objects/Command/Turn_Right180")]
public class Command_Turn_Right180 : Command_Turn
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
        _turnAngle = 180.0f;

        base.Enter(owner);
    }
}
