/*
    ReactionComponent.cs
    20260728  hanaue sho
    ブロックに持たせる機能の基底クラス
    色んなところから Reaction を呼ぶ
*/
using UnityEngine;

public class ReactionComponent : MonoBehaviour
{
    public virtual bool Enter(Block influencer)
    {
        return false;
    }

    // ==================================================
    // ----- Reaction -----
    // ==================================================
    public virtual void Reaction(Block influencer)
    {
        Debug.Log($"{GetType()}");
    }

    public virtual void Exit(Block influencer)
    {

    }

}
