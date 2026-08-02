/*
    Reaction_NoTouch.cs
    20260801  hanaue sho
    触れたら終わりのリアクションを再生する
*/
using UnityEngine;

public class Reaction_NoTouch : ReactionComponent
{
    private SceneManager_LevelScene _smLevel;

    private void Start()
    {
        _smLevel = GameObject.FindAnyObjectByType<SceneManager_LevelScene>();
        if (_smLevel == null)
        {
            Debug.LogWarning("[Warning] Not Find SceneManager_LevelScene!!!");
        }
    }

    public override bool Enter(Block influencer, CommandComponent command)
    {
        // ここで死亡を通知
        _smLevel?.OnGameOver();

        return base.Enter(influencer, command);
    }
}
