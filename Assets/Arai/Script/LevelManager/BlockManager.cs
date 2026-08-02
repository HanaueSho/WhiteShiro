/*
    BlockManager.cs
    20260802  arai eito
    ブロックの一括管理
*/
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class BlockManager : MonoBehaviour
{
    // ==================================================
    // ----- Priority -----
    // ==================================================
    private struct BlockInfo
    {
        public Block _block;
        public Vector3 _start;
        public Vector3 _position;
        public Quaternion _rotation;
        public Transform _parent;
    }
    private List<BlockInfo> _blockInfos = new List<BlockInfo>();



    // ==================================================
    // ----- Unity Events -----
    // ==================================================
    private void Awake()
    {
        _blockInfos.Clear();

        var blocks = GetComponentsInChildren<Block>();        
        
        Array.Sort(blocks, (a, b) =>
            a.transform.position.y.CompareTo(b.transform.position.y));

        foreach (var block in blocks)
        {
            BlockInfo info;
            info._block = block;
            info._start = Vector3.zero;
            info._position = block.transform.position;
            info._rotation = block.transform.rotation;
            info._parent = block.transform.parent;

            _blockInfos.Add(info);
        }


        StartCoroutine(StartAnimation());
    }


    // ==================================================
    // ----- Public Events -----
    // ==================================================
    public IEnumerator StartAnimation()
    {
        float oneBlockAnimationTime = 1.0f;
        float gapTime = 0.07f;

        float totalTime = oneBlockAnimationTime + gapTime * _blockInfos.Count;



        // ブロックごとにtimeを設定
        foreach(var info in _blockInfos)
        {
            Block b = info._block;
            if (b == null) continue;

            b.transform.position = info._position + Vector3.up * 50.0f;
        }



        for (float t = 0.0f; t < totalTime + 1f; t += Time.deltaTime)
        {
            // ブロックごとにtimeを設定
            for (int i = 0; i < _blockInfos.Count; i++)
            {
                float time = t - i * gapTime;
                time = Mathf.Clamp(time, 0.0f, oneBlockAnimationTime);
                time /= oneBlockAnimationTime;


                if(_blockInfos[i]._block is Block b )
                {
                    // 位置
                    b.transform.position = Vector3.Lerp(
                        _blockInfos[i]._position + Vector3.up * 50,
                        _blockInfos[i]._position,
                        time);
                }
                
            }

            yield return null;
        }

        yield return new WaitForSeconds(1.0f);

        yield break;
    }
    
    public IEnumerator ResetBlock()
    {
        for(int i = 0; i < _blockInfos.Count;i++)
        {
            BlockInfo info = _blockInfos[i];

            Transform trans = _blockInfos[i]._block?.transform;
            if (trans == null) continue;

            info._start = trans.position;
            _blockInfos[i] = info;


            trans.SetParent(info._parent, false);
        }

        float time = 1.5f;
        for(float t = 0.0f; t < time; t += Time.deltaTime)
        {
            float value = t / time;

            foreach(var info in _blockInfos)
            { 
                Transform trans = info._block?.transform;
                if (trans == null) continue;

                Vector3 start = info._start;
                Vector3 end = info._position;
                Vector3 current = start + (end - start) * value;

                trans.position = current;
            }

            yield return null;
        }

        foreach (var info in _blockInfos)
        {
            Transform trans = info._block?.transform;
            if (trans == null) continue;

            trans.position = info._position;
            trans.rotation = info._rotation;
        }


        yield break;
    }


}
