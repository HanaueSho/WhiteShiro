/*
    CommandWhile.cs
    20260728  arai eito
    コマンドでWhile文実行するためのコマンド
*/
using System.Collections.Generic;
using UnityEngine;

public class Command_While : CommandComponent
{
    private int _commandCursor;
    [SerializeField] private List<CommandComponent> _inCommands;



    public override bool Command()
    {
        base.Command();


        int commandCount = _inCommands.Count;


        // チェック
        if(commandCount <= 0)
        {
            return false;
        }

        // コマンドを実行
        // コマンドがnullだったら強制的に次のコマンドに移行する
        bool next = _inCommands[_commandCursor]?.Command() ?? true;


        // カーソルを次に進める
        if(next == true)
        {
            _commandCursor++;

            // 上限だった場合０に戻す
            if(_commandCursor >= commandCount)
            {
                _commandCursor = 0;
                return OnLoopEnd();
            }
        }

        return false;
    }

    /// <summary>
    /// コマンドの終わりに呼ぶ関数
    /// For文に継承するためだけに存在
    /// </summary>
    protected virtual bool OnLoopEnd()
    {
        Debug.Log("[Debug] While Loop End");
        return false;
    }


}
