/*
    CommandVisual.cs
    20260730  arai eito
    コマンド一覧を入れると自動で見た目を作ってくれる
*/
using System.Collections.Generic;
using UnityEngine;


public class CommandVisual : MonoBehaviour
{
    // ==================================================
    // ----- Node -----
    // ==================================================
    [SerializeField] private CommandComponent _root;
    [SerializeField] private GameObject _nodePrefab;
    private CommandVisualNode_Base _rootVisualNode;
    List<CommandVisualNode_Base> _nodes = new();

    // 関数で使う用
    private CommandVisualNode_Base _beforeNode;

    // ==================================================
    // ----- Public Propaty -----
    // ==================================================
    public CommandComponent Root => _root;

    // ==================================================
    // ----- Public Events -----
    // ==================================================
    public void Visual(CommandComponent root)
    {
        if(root == null)
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
        CreateNode(root, transform , null, null);

    }

    public void DebugHashCode()
    {
        foreach(var node in _nodes)
        {
            Debug.Log(node.GetHashCode());
        }
    }

    // ==================================================
    // ----- Private Events -----
    // ==================================================
    /// <summary>
    /// 見た目のノードを作る関数
    /// </summary>    
    private void CreateNode(CommandComponent cmd ,Transform parent ,CommandVisualNode_Base rootNode, CommandVisualNode_Base parentNode)
    {
        // ノードオブジェクト生成
        GameObject obj = Instantiate(_nodePrefab);
        obj.name = cmd.name;

        // ノードクラス生成
        CommandVisualNode_Base node = obj.GetComponent<CommandVisualNode_Base>();
        if(node == null)
        {
            return;
        }
        if (rootNode == null)
        {
            _rootVisualNode = node;    
        }
        node.Indent = parentNode?.Indent + 1 ?? -1;

        // コマンド複製
        CommandComponent newCmd = Instantiate(cmd);
        node.Command = newCmd;
        if(parentNode?.Command is Command_While parentCmdWhile)
        {
            parentCmdWhile.AddCommand(null, newCmd);
        }

        // 色
        int nameHash = obj.name.GetHashCode();
        node.SetColor(Color.HSVToRGB(((nameHash & 0xFFFF) / 65535f), 0.8f, 1.0f));

        // 親
        obj.transform.SetParent(parent);

        // 場所
        obj.transform.position = transform.position;

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
                CreateNode(child, node.transform, _rootVisualNode, node);
            }
        }
    }
}
