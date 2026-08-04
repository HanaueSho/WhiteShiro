using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using static UnityEngine.Rendering.VolumeComponent;

public class CommandVisualNode_Accessory_Int : UI_Base
{
    // ==================================================
    // ----- Propaty -----
    // ==================================================
    private int _int = 3;
    [SerializeField] private Button _upButton;
    [SerializeField] private Button _downButton;
    [SerializeField] private Text _text;
    [SerializeField] private Vector2 _offset;

    private CommandVisualNode_Base _parentNode;
    private UnityAction<int> _intSetAction;

    // ==================================================
    // ----- Public Propaty -----
    // ==================================================
    private float _moveSpeed = 15.0f;
    public UnityAction<int> IntSetAction {  get { return _intSetAction; } set { _intSetAction = value; } }
    public CommandVisualNode_Base ParentNode { set { _parentNode = value; } }
    public Vector2 Offset { set { _offset = value; } }


    // ==================================================
    // ----- Unity Event -----
    // ==================================================
    private void Awake()
    {
        if (_text != null)
        {
            _text.text = _int.ToString("00");
        }
    }
    private void OnEnable()
    {
        _upButton?.onClick.AddListener(() => ChangeInt(1));
        _downButton?.onClick.AddListener(() => ChangeInt(-1));
    }
    private void OnDisable()
    {
        _upButton?.onClick.RemoveAllListeners();
        _downButton?.onClick.RemoveAllListeners();
    }
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

        if(_parentNode != null)
        {
            position.x =
                _parentNode.transform.position.x +
                _parentNode.RectTransform.sizeDelta.x * 0.5f * _parentNode.RectTransform.lossyScale.x +
                RectTransform.sizeDelta.x * 0.5f * RectTransform.lossyScale.x;


            position.y = _parentNode.transform.position.y + _offset.y;
        }


        // 移動
        Vector3 dir = position - transform.position;

        if (dir.magnitude > _moveSpeed * Time.deltaTime)
        {
            transform.position += dir * _moveSpeed * Time.deltaTime;
        }
        else
        {
            transform.position = position;
        }
    }

    // ==================================================
    // ----- Private Event -----
    // ==================================================

    private void ChangeInt(int i)
    {
        _int += i;
        _int = Mathf.Clamp(_int, 1, 99);

        _intSetAction?.Invoke(_int);
        if(_text != null)
        {
            _text.text = _int.ToString("00");
        }
    }
}
