/*
    PlayerCharacter_Animator.cs
    20260802  hanaue sho
    プレイヤー専用のアニメーションを再生を管理
    Player, SubPlayer に持たせます。
*/
using UnityEngine;

public class PlayerCharacter_Animator : MonoBehaviour
{
    private Animator _animator;// Animator


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (_animator == null)
        {
            _animator = GetComponentInChildren<Animator>();
        }
        if (_animator == null)
        {
            Debug.LogWarning("[Warning] No Animator.");
        }
    }

    public void OnAnimatorMoving(bool isMoving)
    {
        _animator?.SetBool("IsMoving", isMoving);
    }

    public void OnAnimatorFalling(bool isFalling)
    {
        _animator?.SetBool("IsFalling", isFalling);
    }


}
