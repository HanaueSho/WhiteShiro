/*
    Reaction_Button.cs
    20260801  hanaue sho
    ボタンリアクション
    ターゲットのリアクションを起動する
    それかターゲットの CommandPlayer が持つコマンドを実行する方が良い？
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reaction_Button : ReactionComponent
{
    [Header("ターゲットの CommandPlayer（手動で設定）")]
    [SerializeField] private List<CommandPlayer> _targetCommandPlayers;

    [SerializeField] private bool _isDownButton = false;

    public override bool Enter(Block influencer, CommandComponent command)
    {
        if (!_isDownButton)
        {
            StartCoroutine(Reaction(GetComponent<Block>()));
            _isDownButton = true;
        }
        return true;
    }

    public override IEnumerator Reaction(Block influencer)
    {
        foreach (var reaction in _targetCommandPlayers)
        {
            yield return(reaction.PlayCommand());
        }

        yield break;
    }

    public override void Exit(Block influencer)
    {
        if (_isDownButton)
        {
            StartCoroutine(Reaction(GetComponent<Block>()));
            _isDownButton = false;
        }
    }


}
