/*
    CommandVisualSpawner.cs
    20260731  arai eito
    コマンドビジュアルのスポナー
*/
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CommandVisualSpawner : UI_Base, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    // ==================================================
    // ----- Propaty -----
    // ==================================================
    [SerializeField] private CommandVisual _visual;
    [SerializeField] private CommandComponent _command;
    private CommandVisualNode_Base _dragNode;

    // ==================================================
    // ----- Drag -----
    // ==================================================
    public void OnBeginDrag(PointerEventData eventData)
    {
        // 前のがあった場合削除
        if (_dragNode != null)
        {
            Destroy(_dragNode.gameObject);
            _dragNode = null;
        }

        // ノード生成
        _dragNode = _visual?.CreateNode(_command);
        if(_dragNode == null)
        {
            return;
        }

        // 親
        _dragNode.transform.SetParent(transform,false);

        RectTransformUtility.ScreenPointToWorldPointInRectangle(
            _dragNode.RectTransform,
            eventData.position,
            null,
            out Vector3 position);

        _dragNode.RectTransform.position = position;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if(_dragNode == null)
        {
            return;
        }

        RectTransformUtility.ScreenPointToWorldPointInRectangle(
            _dragNode.RectTransform,
            eventData.position,
            null,
            out Vector3 position);

        _dragNode.RectTransform.position = position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (_dragNode == null)
        {
            return;
        }

        _dragNode.OnEndDrag(eventData);

        // エラーノード
        if(_dragNode.BeforeNode == null)
        {
            Debug.Log("DESTROY");
            Destroy(_dragNode.gameObject);
        }
        else
        {
            _dragNode = null;
        }
    }
}
