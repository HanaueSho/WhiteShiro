/*
    LevelEditorBlockButton.cs
    20260802  arai eito
    gameObjectを返すイベント駆動型のUI
 */
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class LevelEditorBlockButton : MonoBehaviour
{

    private Block _block;
    private Button _button;

    private UnityAction<Block> _action;

    private void Awake()
    {
        _block = GetComponentInChildren<Block>();
        _button = GetComponentInChildren<Button>();    
    }


}
