/*
    CommandFor.cs
    20260728  arai eito
    コマンドFor文実行するためのコマンド
*/
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "Command_For", menuName = "Scriptable Objects/Command/For")]
public class Command_For : Command_While
{
    // ==================================================
    // ----- Propaty -----
    // ==================================================
    private int _forCursor;
    private int _forMax = 3;

    // ==================================================
    // ----- Public Propaty -----
    // ==================================================
    public void SetForMax(int value) { _forMax = value; }

    // ==================================================
    // ----- Unity Event -----
    // ==================================================
    private void OnEnable()
    {
        _visualText = "1 回繰り返す";
    }

    // ==================================================
    // ----- Public Event -----
    // ==================================================
    protected override bool OnLoopEnd()
    {
        _forCursor++;

        bool next = (_forCursor >= _forMax);
        if (next == true) _forCursor = 0;

        return next;
    }


}
