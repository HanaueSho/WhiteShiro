/*
    SceneManager_Base.cs
    20260801  hanaue sho
    シーンを管理するマネージャーの基底クラス
*/
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class SceneManager_Base : MonoBehaviour
{
    // ==================================================
    // ----- Propaty -----
    // ==================================================
    private UnityAction<int> _nextSceneMoveAction;

    // ==================================================
    // ----- Public Propaty -----
    // ==================================================
    public UnityAction<int> NextSceneMoveAction { set { _nextSceneMoveAction = value; } }


    // ==================================================
    // ----- Public Events -----
    // ==================================================
    public void Enter()
    {
        Debug.Log("[Debug] SceneManager Enter");
    }

    public void Exit(int sceneIndex)
    {
        // 実行
        _nextSceneMoveAction?.Invoke(sceneIndex);


    }

    public void OnExitButtonClick(int sceneIndex)
    {
        Exit(sceneIndex);
    }

}
