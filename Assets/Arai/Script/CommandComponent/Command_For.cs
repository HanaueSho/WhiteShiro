/*
    CommandFor.cs
    20260728  arai eito
    コマンドFor文実行するためのコマンド
*/
using UnityEngine.Events;

public class Command_For : Command_While
{
    private int _forCursor;
    private int _forMax;



    protected override bool OnLoopEnd()
    {
        _forCursor++;

        bool next = (_forCursor >= _forMax);
        if (next == true) _forCursor = 0;

        return next;
    }


}
