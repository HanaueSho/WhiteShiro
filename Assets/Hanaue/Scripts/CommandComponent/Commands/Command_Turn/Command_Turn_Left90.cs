/*
    Command_Turn_Left90.cs
    20260801  hanaue sho
    コマンド左旋回９０度
*/
using UnityEngine;


[CreateAssetMenu(fileName = "Command_Turn_Left90", menuName = "Scriptable Objects/Command/Turn_Left90")]
public class Command_Turn_Left90 : Command_Turn
{
    // ==================================================
    // ----- Unity Event -----
    // ==================================================
    private void OnEnable()
    {
        _visualText = "左回りしろ";
    }


    public override void Enter(CommandPlayer owner)
    {
        _turnAngle = -90.0f;

        base.Enter(owner);
    }
}
