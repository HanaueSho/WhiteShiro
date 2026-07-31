/*
    CommandVisualNode_Base.cs
    20260730  arai eito
    コマンドの見た目の基底クラス
*/
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class CommandVisualNode_Base : UI_Base , IInitializePotentialDragHandler, IBeginDragHandler, IDragHandler , IEndDragHandler
{
    // ==================================================
    // ----- UI Propaty -----
    // ==================================================
    [SerializeField] private Text _nameText;
    [SerializeField] private Image _baseImage;

    [SerializeField] private RectTransform _skinRectTransform;

    // ==================================================
    // ----- Propaty -----
    // ==================================================
    private int _indent = 0;
    private CommandComponent _command = null;
    private CommandVisualNode_Base _rootNode;
    [SerializeField] private CommandVisualNode_Base _beforeNode;
    [SerializeField] private CommandVisualNode_Base _afterNode;

    private float _moveSpeed = 15.0f;

    // ==================================================
    // ----- Drag Propaty -----
    // ==================================================
    private Vector2 _dragBeginSkinPosition;
    private Vector2 _dragOffset;

    // ==================================================
    // ----- Public Propaty -----
    // ==================================================
    public int Indent { get { return _indent; } set { _indent = value; } }
    public CommandComponent Command { get { return _command; } 
        set 
        {
            _command = value;
            if(_nameText != null)
            { 
                _nameText.text = _command?.VisualText ?? "なし";
            }
        } }
    public CommandVisualNode_Base Root { get { return _rootNode; } set { _rootNode = value; } }
    public CommandVisualNode_Base BeforeNode { get { return _beforeNode; } set { _beforeNode = value; } }
    public CommandVisualNode_Base AfterNode { get { return _afterNode; } set { _afterNode = value; } }


    // ==================================================
    // ----- Unity Event -----
    // ==================================================
    private void Update()
    {
        // 動き
        MoveUpdate();
    }

    // ==================================================
    // ----- Update Event -----
    // ==================================================
    private void MoveUpdate()
    {
        // 目的地
        Vector3 position = transform.position;

        if(_rootNode != this && _rootNode != null)
        {
            position.x = _rootNode.transform.position.x + _indent * 30.0f;
        }


        if (_beforeNode != null)
        {
            RectTransform rect = _beforeNode.RectTransform;
            
            position.y = rect.position.y - rect.sizeDelta.y;
        }


        // 移動
        Vector3 dir = position - transform.position;

        if(dir.magnitude > _moveSpeed * Time.deltaTime)
        {
            transform.position += dir * _moveSpeed * Time.deltaTime;
        }
        else
        {
            transform.position = position;
        }
    }

    // ==================================================
    // ----- Public Event -----
    // ==================================================
    public void SetColor(Color color)
    {
        if (_baseImage == null)
        {
            return;
        }

        _baseImage.color = color;
    }


    // ==================================================
    // ----- Drag -----
    // ==================================================

    public void OnInitializePotentialDrag(PointerEventData eventData)
    {
        eventData.useDragThreshold = false;
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        _dragBeginSkinPosition = _skinRectTransform.anchoredPosition;

        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            RectTransform,
            eventData.position,
            null,
            out Vector2 mouseLocalPos);

        _dragOffset = _skinRectTransform.anchoredPosition - mouseLocalPos;
    }

    public void OnDrag(PointerEventData eventData)
    {
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            RectTransform,
            eventData.position,
            null,
            out Vector2 mouseLocalPos);

        _skinRectTransform.anchoredPosition = mouseLocalPos + _dragOffset;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        // 見た目元の場所に戻す
        Vector3 skinPosition = _skinRectTransform.position;
        _skinRectTransform.anchoredPosition = _dragBeginSkinPosition;
        transform.position = skinPosition;
        
        // 入れ替えのCommandVisualNode を取得
        CommandVisualNode_Base anotherNode = null;

        // カーソルにあるUIの一覧を取得
        List<RaycastResult> results = new();
        EventSystem.current.RaycastAll(eventData, results);
        foreach (RaycastResult result in results)
        {
            CommandVisualNode_Base currentNode = result.gameObject.GetComponentInParent<CommandVisualNode_Base>();
            if(currentNode != this)
            {
                anotherNode = currentNode;
                break;
            }
        }

        // 入れ替え無し
        if(anotherNode == null || anotherNode == _rootNode || anotherNode.transform.IsChildOf(transform))
        {
            return;
        }


        // 交換
        // Commandチェイン入れ替え

        // 自分の末端
        // 自分がWhile系統だったらInCommandも移動させたい
        CommandVisualNode_Base lastNode = this;
        if (_command is Command_While)
        {
            // 最終を調べる
            while( 
                lastNode._afterNode != null &&
                lastNode._afterNode._indent > _indent )
            {
                lastNode = lastNode._afterNode;
            }
        }

        // 自分の末端の次
        CommandVisualNode_Base exitNode = lastNode._afterNode;



        // 一時的に自分を除外する
        {
            if(_beforeNode != null)
            {
                _beforeNode._afterNode = exitNode;
            }
            if(exitNode != null)
            {
                exitNode._beforeNode = _beforeNode;
            }
        }


        // 自分を加える
        {
            // 自分
            if (transform.position.y > anotherNode.transform.position.y)
            {
                _beforeNode = anotherNode._beforeNode;
                lastNode._afterNode = anotherNode;
            }
            else
            {
                _beforeNode = anotherNode;                
                lastNode._afterNode = anotherNode._afterNode;
            }
            // 周り
            if (_beforeNode != null)
            {
                _beforeNode._afterNode = this;
            }
            if (lastNode._afterNode != null)
            {
                lastNode._afterNode._beforeNode = lastNode;
            }
        }

        // While設定
        {

            // While 解除
            CommandVisualNode_Base cmdWhileNode = transform.parent.GetComponentInParent<CommandVisualNode_Base>();
            Command_While cmdWhile = cmdWhileNode?.Command as Command_While;
            cmdWhile?.RemoveCommand(_command);

            // While 登録
            cmdWhile = null;

            if (anotherNode != _afterNode)
            {
                cmdWhileNode = anotherNode;
                cmdWhile = cmdWhileNode?.Command as Command_While;
            }

            if (cmdWhile == null)
            {
                cmdWhileNode = anotherNode.transform.parent.GetComponentInParent<CommandVisualNode_Base>();
                cmdWhile = cmdWhileNode?.Command as Command_While;
            }

            cmdWhile?.AddCommand(_beforeNode._command, _command);

            // Transform Parent設定
            transform.SetParent(cmdWhileNode.transform);



            // Whileだったら自分の子どものインデントを変える            
            SetNodeIndent(this);
        }

    }
    private void SetNodeIndent(CommandVisualNode_Base node)
    {
        if(node == null)
        {
            return;
        }
        
        // 親検索
        CommandVisualNode_Base parentNode = node.transform.parent.GetComponentInParent<CommandVisualNode_Base>();
        if(parentNode?.Command is not Command_While)
        {
            return;
        }

        // インデント設定
        node._indent = parentNode?._indent + 1 ?? -1;

        // 子インデント設定
        foreach(Transform child in node.transform)
        {
            var childNode = child.GetComponent<CommandVisualNode_Base>();
            if(childNode != null)
            {
                SetNodeIndent(childNode);
            }
        }
    }

    private void OnDrawGizmos()
    {

        Vector3 position = _skinRectTransform.TransformPoint(Vector3.zero);

        Gizmos.color = Color.red;
        Gizmos.DrawSphere(position, 10f);
    }

}
