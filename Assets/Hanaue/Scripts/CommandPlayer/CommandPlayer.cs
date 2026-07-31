/*
    CommandPlayer.cs
    20260728  hanaue sho
    キャラクターが持つコマンド
    ScenePlayer から順に呼ばれる

    20260728  arai eito
    コマンドリストを追加
*/
using UnityEngine;

public class CommandPlayer : MonoBehaviour
{
    // ==================================================
    // ----- Priority -----
    // ==================================================
    [SerializeField] private ScenePlayer.CommandPlayerPriority _priority = ScenePlayer.CommandPlayerPriority.Default;
    public ScenePlayer.CommandPlayerPriority Priority => _priority;

    // ==================================================
    // ----- Base Command -----
    // ==================================================
    [SerializeField] private CommandComponent _baseCommand;

    // ==================================================
    // ----- Public Propaty -----
    // ==================================================
    public bool IsPlaying { get; private set; }
    public CommandComponent BaseCommand { get { return _baseCommand; } set { _baseCommand = value; } }


    // ==================================================
    // ----- Unity Events -----
    // ==================================================
    private void Start()
    {
        // CommandComponent を登録
        ScenePlayer.Instance.Register(this);
    }

    // ==================================================
    // ----- Public Events -----
    // ==================================================
    public void PlayCommand()
    {
        if (!IsPlaying)
        {
            return;
        }

        // Command 実行
        Debug.Log($"{GetType()} : {_priority}");

        // Enter
        _baseCommand?.Enter(this);
        // Commnad
        if (_baseCommand?.Command(this) ?? true)
        {
            // Command が true を返したら実行を止める
            IsPlaying = false;
        }
        // Exit 
        _baseCommand?.Exit(this);
        
    }

    public void StartCommand()
    {
        IsPlaying = true;
        _baseCommand.Initialize();
    }
}
