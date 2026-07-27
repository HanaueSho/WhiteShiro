/*
    ScenePlayer.cs
    20260728  hanaue sho
    
*/
using System.Collections.Generic;
using UnityEngine;

public class ScenePlayer : MonoBehaviour
{
    // ==================================================
    // ----- Singleton -----
    // ==================================================
    static private ScenePlayer _instance;
    static public ScenePlayer Instance => _instance;

    // ==================================================
    // ----- Command Priority -----
    // ==================================================
    public enum CommandPlayerPriority
    {
        Player = 0,
        SubPlayer,
        Gimmic,
        Default
    }

    // ==================================================
    // ----- Abilities -----
    // ==================================================
    private Dictionary<CommandPlayerPriority, List<CommandPlayer>> _commandPlayers;
    private List<CommandPlayer> _defaults;


    // ==================================================
    // ----- Unity Events -----
    // ==================================================
    private void Awake()
    {
        _instance = this;
        _commandPlayers = new();
        _defaults = new();
    }

    // ==================================================
    // ----- Public Events -----
    // ==================================================
    public void Register(CommandPlayer player)
    {
        // null チェック
        if (player == null)
        {
            return;
        }

        // Default チェック
        CommandPlayerPriority priority = player.Priority;
        if (priority == CommandPlayerPriority.Default)
        {
            _defaults.Add(player);
            return;
        }

        // 格納
        if (_commandPlayers.TryGetValue(priority, out var list))
        {
            list.Add(player);
        }
        else
        {
            List<CommandPlayer> newList = new();
            _commandPlayers.Add(priority, newList);
            newList.Add(player);
        }
    }

    public void OnPlay()
    {
        Play();
    }

    // ==================================================
    // ----- Play Turn -----
    // ==================================================
    private void Play()
    {
        for (int i = 0; i < (int)CommandPlayerPriority.Default; i++)
        {
            CommandPlayerPriority priority = (CommandPlayerPriority)i;

            if (_commandPlayers.TryGetValue(priority, out var list))
            {

                // リアクション実行
                foreach(CommandPlayer player in list)
                {
                    if (player == null)
                    {
                        // 消去処理いれたいね
                        continue;
                    }
                    player.PlayCommand();
                }

            }
        }
        foreach (CommandPlayer player in _defaults)
        {
            if (player == null)
            {
                // 消去処理いれたいね
                continue;
            }
            player.PlayCommand();
        }
    }
}
