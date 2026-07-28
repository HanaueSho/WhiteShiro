/*
    CommandComponent.cs
    20260728  hanaue sho
    コマンドの基底クラス
    CommandPlayer から順に呼ばれる
*/
using UnityEngine;

public class CommandComponent : MonoBehaviour
{
    public virtual bool Command()
    {
        Debug.Log($"{GetType()}");
        return true;
    }
}
