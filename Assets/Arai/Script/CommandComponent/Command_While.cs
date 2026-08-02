/*
    CommandWhile.cs
    20260728  arai eito
    コマンドでWhile文実行するためのコマンド
*/
using System;
using System.Collections;
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
    // ----- Public Propaty -----
    // ==================================================
    public List<CommandComponent> InCommand => _inCommands;

    // ==================================================
    // ----- Unity Event -----
    // ==================================================
    private void OnEnable()
    {
        _visualText = "無限にしろ";
    }
    // ==================================================
    // ----- Public Events -----
    // ==================================================
    public void AddCommand(CommandComponent before, CommandComponent cmd)
    {
        // 先頭
        if(before == this)
        {
            _inCommands.Insert(0, cmd);
            return;
        }

        int index = _inCommands.IndexOf(before);

        // beforeがnullなら最後
        if (index == -1)
        {
            _inCommands.Add(cmd);
            return;
        }

        // Index に入れる
        _inCommands.Insert(index + 1, cmd);
    }

    public void RemoveCommand(CommandComponent cmd)
    {
        _inCommands.Remove(cmd);
    }
    public void ClearCommand()
    {
        _inCommands.Clear();
    }

    public override void Initialize()
    {
        base.Initialize();
        _commandCursor = 0;
        foreach (var command in _inCommands)
        {
            command.Initialize();
        }
    }

    public override void Enter(CommandPlayer owner)
    {
        base.Enter(owner);
    }

    public override IEnumerator Command(CommandPlayer owner, Action<bool> result)
    {
        base.Command(owner, result);


        int commandCount = _inCommands.Count;


        // チェック
        if(commandCount <= 0 || _commandCursor >= commandCount)
        {
            result(true);
            yield break;
        }

        // コマンドを実行
        // コマンドがnullだったら強制的に次のコマンドに移行する
        bool next = true;
        _inCommands[_commandCursor]?.Enter(owner);
        yield return _inCommands[_commandCursor]?.Command(owner, b => next = b);
        yield return _inCommands[_commandCursor]?.Exit(owner);


        // カーソルを次に進める
        if (next == true)
        {
            _commandCursor++;

            // 上限だった場合０に戻す
            if(_commandCursor >= commandCount)
            {
                _commandCursor = 0;
                result(OnLoopEnd());
                yield break;
            }
        }

        result(false);
        yield break;
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
