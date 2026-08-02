/*
    CommandVisualTrashCan.cs
    20260731  arai eito
    コマンドビジュアルのゴミ箱
*/
using UnityEngine;

public class CommandVisualTrashCan : UI_Base
{
    // ==================================================
    // ----- Public Events -----
    // ==================================================
    public void DeleteNode(CommandVisualNode_Base node)
    {
        if(node == null || node.Root == node)
        {
            return;
        }


        // 自分の末端
        // 自分がWhile系統だったらInCommandも移動させたい
        CommandVisualNode_Base lastNode = node;
        if (node.Command is Command_While)
        {
            // 最終を調べる
            while (
                lastNode.AfterNode != null &&
                lastNode.AfterNode.Indent > node.Indent)
            {
                lastNode = lastNode.AfterNode;
            }
        }

        // 自分の末端の次
        CommandVisualNode_Base exitNode = lastNode.AfterNode;


        // 一時的に自分を除外する
        {
            if (node.BeforeNode != null)
            {
                node.BeforeNode.AfterNode = exitNode;
            }
            if (exitNode != null)
            {
                exitNode.BeforeNode = node.BeforeNode;
            }
        }

        // While 解除
        CommandVisualNode_Base cmdWhileNode = node.transform.parent.GetComponentInParent<CommandVisualNode_Base>();
        Command_While cmdWhile = cmdWhileNode?.Command as Command_While;
        cmdWhile?.RemoveCommand(node.Command);
        Debug.Log(cmdWhile);        

        // ゲームオブジェクト削除
        Destroy(node.gameObject);
    }
}
