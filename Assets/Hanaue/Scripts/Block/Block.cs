/*
    Block.cs
    20260728  hanaue sho

*/
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{

    // ==================================================
    // ----- Public Events -----
    // ==================================================
    // 上下左右前後に隣接するブロックを取得
    public List<Block> GetAroundBlocks()
    {
        // ----- オブジェクトの位置同期（処理重かったらごめんね） -----
        Physics.SyncTransforms();

        // リストの作成
        List<Block> result = new List<Block>();

        // 前後左右上下の６方向
        Vector3[] Directions =
        {
            Vector3.right,
            Vector3.left,
            Vector3.up,
            Vector3.down,
            Vector3.forward,
            Vector3.back
        };
        foreach (Vector3 localDirection in Directions)
        {
            Block b = GetBlock(localDirection);
            if (b != null)
            {
                result.Add(b);
            }
        }
        return result;
    }
    // 正面のブロックを取得
    public Block GetForwardBlock()
    {
        // ----- オブジェクトの位置同期（処理重かったらごめんね） -----
        Physics.SyncTransforms();
        return GetBlock(Vector3.forward);
    }
    // 上部のブロックを取得
    public Block GetUpBlock()
    {
        // ----- オブジェクトの位置同期（処理重かったらごめんね） -----
        Physics.SyncTransforms();
        return GetBlock(Vector3.up);
    }
    // 真下のブロックを取得
    public Block GetDownBlock()
    {
        // ----- オブジェクトの位置同期（処理重かったらごめんね） -----
        Physics.SyncTransforms();
        return GetBlock(Vector3.down);
    }
    // ２マス以上下のブロックを取得
    public Block GetLowerBlock()
    {
        // ----- オブジェクトの位置同期（処理重かったらごめんね） -----
        Physics.SyncTransforms();
        Vector3 worldDirection = transform.TransformDirection(Vector3.down);
        if (Physics.Raycast(transform.position + new Vector3(0.0f, -1.0f, 0.0f), worldDirection, out RaycastHit hit, 100.0f)) // 100マス下まで検索
        {
            if (hit.transform.GetComponent<Block>())
            {
                return hit.transform.GetComponent<Block>();
            }
        }
        return null;
    }

    // ==================================================
    // ----- Get Block -----
    // 引数の方向のブロックを取得
    // ==================================================
    private Block GetBlock(Vector3 localDirection)
    {
        Vector3 worldDirection = transform.TransformDirection(localDirection);
        if (Physics.Raycast(transform.position, worldDirection, out RaycastHit hit, 1.0f))
        {
            if (hit.transform.GetComponent<Block>())
            {
                return hit.transform.GetComponent<Block>();
            }
        }
        return null;
    }
}
