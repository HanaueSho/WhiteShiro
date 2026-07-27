/*
    CommandPlayer.cs
    20260728  hanaue sho
    キャラクターが持つコマンド
    ScenePlayer から順に呼ばれる
*/
using UnityEngine;

public class CommandPlayer : MonoBehaviour
{
    [SerializeField] private ScenePlayer.CommandPlayerPriority _priority = ScenePlayer.CommandPlayerPriority.Default;
    public ScenePlayer.CommandPlayerPriority Priority => _priority;

    // コマンドコンポーネントのリストを持つ
    // カーソルを持つ

    // 関数でカーソルが指すリストのアクションを実行する＋カーソルを動かす
    // ↑こいつを ScenePlayer が呼ぶ

    private void Start()
    {
        // CommandComponent を登録
        ScenePlayer.Instance.Register(this);
    }

    public void PlayCommand()
    {
        Debug.Log($"{GetType()} : {_priority}");
    }
}
