/*
    ReactionComponent.cs
    20260728  hanaue sho
    ブロックに持たせる機能の基底クラス
    色んなところから Reaction を呼ぶ
*/
using System.Collections;
using UnityEngine;

public class ReactionComponent : MonoBehaviour
{
    public virtual bool Enter(Block influencer, CommandComponent command)
    {
        return false;
    }

    // ==================================================
    // ----- Reaction -----
    // ==================================================
    public virtual IEnumerator Reaction(Block influencer)
    {
        Debug.Log($"{GetType()}");
        yield break;
    }

    public virtual void Exit(Block influencer)
    {

    }

}
