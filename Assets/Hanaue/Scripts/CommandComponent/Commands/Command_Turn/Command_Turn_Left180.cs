/*
    Command_Turn_Left180.cs
    20260801  hanaue sho
    コマンド左旋回１８０度
*/
using UnityEngine;


[CreateAssetMenu(fileName = "Command_Turn_Left180", menuName = "Scriptable Objects/Command/Turn_Left180")]
public class Command_Turn_Left180 : Command_Turn
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
        _turnAngle = -180.0f;

        base.Enter(owner);
    }
}
