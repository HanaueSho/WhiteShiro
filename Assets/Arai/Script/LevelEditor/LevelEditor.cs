/*
    LevelEditor.cs
    20260802  arai eito
    レベルエディター
    プレイボタンを押していない状態でも動くようにする
 */

#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using UnityEngine.InputSystem;


[ExecuteAlways]
public class LevelEditor : MonoBehaviour
{
#if UNITY_EDITOR
    // ==================================================
    // ----- Priority -----
    // ==================================================

    private LevelEditorSelectBlock _selectBlock;

    // ==================================================
    // ----- Unity Events -----
    // ==================================================

    private void OnEnable()
    {
        _selectBlock = GetComponentInChildren<LevelEditorSelectBlock>();
        
        SceneView.duringSceneGui += OnSceneClick;
    }
    private void OnDisable()
    {
        SceneView.duringSceneGui -= OnSceneClick;
    }



    private void Update()
    {
        if(Application.isPlaying)
        {
            OnGameClick();
        }
    }

    private void OnSceneClick(SceneView sceneView)
    {
        if (_selectBlock != null)
        {
            return;
        }

        Event e = Event.current;
        if (e == null)
        {
            return;
        }

        Vector2 mousePos = e.mousePosition;
        Ray ray = HandleUtility.GUIPointToWorldRay(mousePos);

        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            _selectBlock.SelectBlock = hit.collider.gameObject?.GetComponentInParent<Block>();
        }
    }
    private void OnGameClick()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            Vector2 mousePos = Mouse.current.position.ReadValue();

            Ray ray = Camera.main.ScreenPointToRay(mousePos);

            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                _selectBlock.SelectBlock = hit.collider.gameObject?.GetComponentInParent<Block>();
            }
        }
    }
#endif
}
