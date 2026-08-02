/*
    Reaction_Goal.cs
    20260801  hanaue sho
    ゴールのリアクションを再生する
    Goal に持たせます。
*/
using UnityEngine;

public class Reaction_Goal : ReactionComponent
{
    [SerializeField]
    private SceneManager_LevelScene _smLevel;

    private void Start()
    {
        _smLevel = GameObject.FindAnyObjectByType<SceneManager_LevelScene>();
        if (_smLevel == null )
        {
            Debug.LogWarning("[Warning] Not Find SceneManager_LevelScene!!!");
        }
    }

    public override bool Enter(Block influencer, CommandComponent command)
    {
        // ここでクリアを通知
        _smLevel?.OnGoal();

        return base.Enter(influencer, command);
    }

}
