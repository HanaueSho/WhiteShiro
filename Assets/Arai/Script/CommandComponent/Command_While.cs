/*
    CommandWhile.cs
    20260728  arai eito
    コマンドでWhile文実行するためのコマンド
*/
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Command_While", menuName = "Scriptable Objects/Command/While")]
public class Command_While : CommandComponent
{
    // ==================================================
    // ----- Cursor -----
    // ==================================================
    private int _commandCursor = 0;

    // ==================================================
    // ----- Loop Commands -----
    // ==================================================
    [SerializeField] private List<CommandComponent> _inCommands;


    // ==================================================
    // ----- Public Events -----
    // ==================================================
    public override void Initialize()
    {
        base.Initialize();
        _commandCursor = 0;
        foreach (var command in _inCommands)
        {
            command.Initialize();
        }
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override bool Command(CommandPlayer owner)
    {
        base.Command(owner);


        int commandCount = _inCommands.Count;


        // チェック
        if(commandCount <= 0 || _commandCursor >= commandCount)
        {
            return false;
        }

        // コマンドを実行
        // コマンドがnullだったら強制的に次のコマンドに移行する
        bool next = _inCommands[_commandCursor]?.Command(owner) ?? true;


        // カーソルを次に進める
        if (next == true)
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
