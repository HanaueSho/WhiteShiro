/*
    SceneManager_Debug.cs
    20260803  hanaue sho
    デバッグシーンを管理するマネージャー
*/
using UnityEngine;

public class SceneManager_Debug : SceneManager_Base
{
    void Start()
    {
        // 速攻で Title シーンへ遷移する
        base.Exit(1);
    }

}
