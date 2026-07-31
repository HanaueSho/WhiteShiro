/*
    CommandVisualToCommandPlayerEditor.cs
    20260731  arai eito
    CommandVisualToCommandPlayer の Editor
*/
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(CommandVisualToCommandPlayer))]
public class CommandVisualToCommandPlayerEditor : Editor
{
    // ==================================================
    // ----- Private Propaty -----
    // ==================================================
    private CommandVisualToCommandPlayer _visualToPlayer ;


    // ==================================================
    // ----- Unity Events -----
    // ==================================================
    private void OnEnable()
    {
        _visualToPlayer = target as CommandVisualToCommandPlayer;
    }

    // インスペクターに表示されるもの
    public override void OnInspectorGUI()
    {
        // デフォルト
        DrawDefaultInspector();

        GUI.enabled = Application.isPlaying;

        if (GUILayout.Button("VISUAL TO PLAYER"))
        {
            _visualToPlayer.VisualToPlayer();
        }

        GUI.enabled = true;
    }
}
