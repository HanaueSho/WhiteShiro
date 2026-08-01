/*
    SceneManager_BaseEditor.cs
    20260801  hanaue sho
    シーンを管理するマネージャーの基底クラスのエディター
*/
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(SceneManager_Base))]
public class SceneManager_BaseEditor : Editor
{
    // ==================================================
    // ----- Private Propaty -----
    // ==================================================
    private SceneManager_Base _sceneManager;


    // ==================================================
    // ----- Unity Events -----
    // ==================================================
    private void Awake()
    {
        _sceneManager = (SceneManager_Base)target;
    }

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        GUI.enabled = Application.isPlaying;

        if (GUILayout.Button("Exit"))
        {
            _sceneManager.Exit(0); 
        }

        GUI.enabled = true;
    }
}
