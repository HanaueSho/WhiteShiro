/*
    CommandVisual.cs
    20260730  arai eito
    コマンド一覧を入れると自動で見た目を作ってくれる
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CommandVisual : MonoBehaviour
{

    // ==================================================
    // ----- Propaty -----
    // ==================================================
    private bool _visible;
    // canvas group
    private CanvasGroup _canvasGroup;

    // ==================================================
    // ----- Node -----
    // ==================================================
    [SerializeField] private CommandComponent _root;
    [SerializeField] private GameObject _nodePrefab;
    [SerializeField] private GameObject _nodeAccessoryIntPrefab;
    // ゲーム上での可変データ
    private CommandVisualNode_Base _rootVisualNode;
    private List<CommandVisualNode_Base> _nodes = new();
    // プレイヤー
    private CommandPlayer _commandPlayer;


    // 関数で使う用
    private CommandVisualNode_Base _beforeNode;

    // ==================================================
    // ----- Public Propaty -----
    // ==================================================
    public CommandComponent Root => _root;
    public CommandVisualNode_Base RootVisualNode => _rootVisualNode;
    public bool Visible { 
        set 
        {
            _visible = value;

            if(_canvasGroup != null)
            {
                StartCoroutine(VisibleCoroutine());
            }
        } }


    // ==================================================
    // ----- Unity Events -----
    // ==================================================
    private void Awake()
    {
        _canvasGroup = gameObject.AddComponent<CanvasGroup>();
    }

    // ==================================================
    // ----- Public Events -----
    // ==================================================
    public void Visual(CommandComponent root)
    {
        if (root == null)
        {
            Debug.LogWarning("[Warning] CommandVisual Visual() : root not found.");
            return;
        }

        // コマンドビジュアルノード削除
        if(_rootVisualNode != null)
        {
            if (Application.isPlaying)
            {
                Destroy(_rootVisualNode.gameObject);
            }
            else
            {
                DestroyImmediate(_rootVisualNode.gameObject);
            }
        }

        // ルート設定
        _root = root;

        // コマンドノード
        CreateVisual(root, transform , null, null);

    }

    public void DebugHashCode()
    {
        foreach(var node in _nodes)
        {
            Debug.Log(node.GetHashCode());
        }
    }

    /// <summary>
    /// コマンド工場
    /// </summary>
    private void CreateVisual(CommandComponent cmd ,Transform parent ,CommandVisualNode_Base rootNode, CommandVisualNode_Base parentNode)
    {
        // ノード生成
        CommandVisualNode_Base node = CreateNode(cmd);
        if (node == null)
        {
            return;
        }
        if(rootNode == null)
        {
            _rootVisualNode = node;
        }

        // 親
        node.transform.SetParent(parent);

        // インデント調整
        node.Indent = parentNode?.Indent + 1 ?? -1;

        // コマンド調整
        if (parentNode?.Command is Command_While parentCmdWhile)
        {
            parentCmdWhile.AddCommand(null, node.Command);
        }

        // リストに追加
        node.Root = _rootVisualNode;
        node.BeforeNode = _beforeNode;
        if(_beforeNode != null)
        {
            _beforeNode.AfterNode = node;
        }

        _nodes.Add(node);
        _beforeNode = node;

        // while を継承しているコマンドならinCommandを追加する
        if (cmd is Command_While cmdWhile)
        {
            // InCommand もAddする
            List<CommandComponent> list = cmdWhile.InCommand;
            foreach (CommandComponent child in list)
            {
                // 新しいノード
                CreateVisual(child, node.transform, _rootVisualNode, node);
            }
        }
    }
    public CommandVisualNode_Base CreateNode(CommandComponent cmd)
    {
        if (cmd == null)
        {
            return null;
        }

        // ノードオブジェクト生成
        GameObject obj = Instantiate(_nodePrefab);
        obj.name = cmd.name;

        // ノードクラス生成
        CommandVisualNode_Base node = obj.GetComponent<CommandVisualNode_Base>();
        if (node == null)
        {
            Destroy(obj);
            return null;
        }
        node.Indent = -1;

        // コマンド複製
        {
            CommandComponent newCmd = Instantiate(cmd);
            // while
            if (newCmd is Command_While w)
            {
                w.ClearCommand();
            }

            node.Command = newCmd;
        }

        // 色

        // 親
        obj.transform.SetParent(null);

        // 場所
        obj.transform.position = transform.position;



        // ノード付属品を作る関数 ( for文など )
        CreateNodeAccessory(node);

        return node;
    }
    private void CreateNodeAccessory(CommandVisualNode_Base node)
    {
        CommandComponent cmd = node.Command;

        if(cmd as Command_For)
        {
            CreateNodeAccessoryFor(node);
        }
    }
    private void CreateNodeAccessoryFor(CommandVisualNode_Base parentNode)
    {
        Command_For cmdFor = parentNode?.Command as Command_For;
        if(cmdFor == null)
        {
            return;
        }

        // ノードオブジェクト生成
        GameObject obj = Instantiate(_nodeAccessoryIntPrefab);
        obj.name = parentNode.name;
        obj.transform.SetParent(parentNode.transform,false);
        obj.transform.position = parentNode.transform.position;

        // ノードクラス生成
        CommandVisualNode_Accessory_Int node = obj.GetComponent<CommandVisualNode_Accessory_Int>();
        if (node == null)
        {
            Destroy(obj);
            return;
        }

        // コマンド
        node.IntSetAction = cmdFor.SetForMax;
        node.ParentNode = parentNode;       
    }


    public int GetNodeCount()
    {
        var cmds = GetComponentsInChildren<CommandComponent>();

        return cmds.Length;
    }

    // ==================================================
    // ----- Visible Events -----
    // ==================================================    
    private IEnumerator VisibleCoroutine()
    {
        float time = 0.5f;
        float start = _visible ? 0.0f : 1.0f;
        float end = 1.0f - start;


        foreach(Transform c in transform)
        {
            c.gameObject.SetActive(true);
        }

        for(float t = 0.0f; t < time; t+= Time.deltaTime)
        {
            _canvasGroup.alpha = Mathf.Lerp(start, end, t / time);

            yield return null;
        }
        _canvasGroup.alpha = end;

        foreach (Transform c in transform)
        {
            c.gameObject.SetActive(_visible);
        }
    }


// ==================================================
// ----- Command Player Events -----
// ==================================================
public void SetCommandPlayer(CommandPlayer commandPlayer)
    {
        if (commandPlayer == null)
        {
            return;
        }

        // CommandPlayer をセット
        _commandPlayer = commandPlayer;
        // CommandPlayer のCommandComponentをセット
        // Visual() を自動的に呼ぶ
        // CommandVisualNode の変更した時にCommandVisualのイベントを呼ばせて
        Visual(commandPlayer.BaseCommand);
        _commandPlayer.BaseCommand = _rootVisualNode?.Command;
    }

}
