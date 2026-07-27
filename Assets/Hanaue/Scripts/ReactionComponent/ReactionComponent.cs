/*
    ReactionComponent.cs
    20260728  hanaue sho
    ブロックに持たせる機能の基底クラス
    色んなところから Reaction を呼ぶ
*/
using UnityEngine;

public class ReactionComponent : MonoBehaviour
{
    // ==================================================
    // ----- Reaction -----
    // ==================================================
    public virtual void Reaction()
    {
        Debug.Log($"{GetType()}");
    }
}
