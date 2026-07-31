/*
    UI_Base.cs
    20260730  arai eito
    UIの基底クラス

    自身のRectTransformと所属Canvasを持っています。
*/
using UnityEngine;

public class UI_Base : MonoBehaviour
{
    // ==================================================
    // ----- UI Propaty -----
    // ==================================================
    private RectTransform _rectTransform;
    private Canvas _canvas;

    // ==================================================
    // ----- Public Propaty -----
    // ==================================================
    public RectTransform RectTransform {  get 
        {
            _rectTransform ??= transform as RectTransform;

            if (_rectTransform == null)
            {
                Debug.LogError("[Error] UI_Base はUIオブジェクトに設置してください。");
            }

            return _rectTransform; 
        } }
    public Canvas Canvas { get
        {
            _canvas ??= GetComponentInParent<Canvas>();

            if(_canvas == null)
            {
                Debug.LogError("[Error] UI_Base が Canvas に入っていません。");
            }

            return _canvas;
        } }

}
