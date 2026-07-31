/*
    CommandVisualEditor.cs
    20260730  arai eito
    CommandVisual の Editor
*/
using UnityEngine;
using UnityEditor;
using TMPro.EditorUtilities;


[CustomEditor(typeof(CommandVisual))]
public class CommandVisualEditor : Editor
{
    // ==================================================
    // ----- Private Propaty -----
    // ==================================================
    private CommandVisual _visual;


    // ==================================================
    // ----- Unity Events -----
    // ==================================================
    private void Awake()
    {
        _visual = (CommandVisual)target;
    }

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        GUI.enabled = Application.isPlaying;

        if(GUILayout.Button("VISUAL"))
        {
            _visual.Visual(_visual.Root);
        }

        if (GUILayout.Button("DEBUG HASH CODE"))
        {
            _visual.DebugHashCode();
        }

        GUI.enabled = true;
    }
}
