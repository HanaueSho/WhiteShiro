/*
    ScenePlayer.cs
    20260728  hanaue sho
    
*/
using System.Collections;
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
        Default,
        NoPlay
    }

    // ==================================================
    // ----- Commands -----
    // ==================================================
    private Dictionary<CommandPlayerPriority, List<CommandPlayer>> _commandPlayers;

    private List<CommandPlayer> _defaults;

    // ==================================================
    // ----- Priority -----
    // ==================================================
    private Coroutine _coroutine = null;

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
        // Start
        foreach (var commandPlayerList in _commandPlayers.Values)
        {
            foreach (var commandPlayer in commandPlayerList)
            {
                commandPlayer.StartCommand();
            }
        }

        _coroutine = StartCoroutine(Play());
    }

    public void OnRun()
    {
        if(_coroutine != null)
        {
            return;
        }

        // Start
        foreach(var commandPlayerList in _commandPlayers.Values)
        {
            foreach (var commandPlayer in commandPlayerList)
            {
                commandPlayer.StartCommand();
            }
        }

        // Run
        _coroutine = StartCoroutine(Run());
    }

    public void OnStop()
    {
        if(_coroutine != null)
        {
            StopCoroutine(_coroutine);
            _coroutine = null;
        }
    }

    // ==================================================
    // ----- Play Turn -----
    // ==================================================
    private IEnumerator Play()
    {
        for (int i = 0; i < (int)CommandPlayerPriority.Gimmic; i++)
        {
            CommandPlayerPriority priority = (CommandPlayerPriority)i;

            if (_commandPlayers.TryGetValue(priority, out var list))
            {

                // コマンド実行
                foreach(CommandPlayer player in list)
                {
                    if (player == null)
                    {
                        // 消去処理いれたいね
                        continue;
                    }
                    // player.PlayCommand();
                    yield return player.PlayCommand();
                }

                // 0.5 seconds
                yield return new WaitForSeconds(0.5f);
            }
        }
        
        {
            CommandPlayerPriority priority = (CommandPlayerPriority.Gimmic);

            if (_commandPlayers.TryGetValue(priority, out var list))
            {

                // コマンド実行
                foreach (CommandPlayer player in list)
                {
                    if (player == null)
                    {
                        // 消去処理いれたいね
                        continue;
                    }
                    // player.PlayCommand();
                    StartCoroutine(player.PlayCommand());
                }
            }
        }

        // 0.5 seconds
        yield return new WaitForSeconds(0.5f);

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

    private IEnumerator Run()
    {
        bool isPlaying = false;
        do
        {
            // コマンド実行
            yield return Play();


            isPlaying = false;
            // メインプレイヤーの終了チェック
            CommandPlayerPriority priority = CommandPlayerPriority.Player;
            if (_commandPlayers.TryGetValue(priority, out var playerList))
            {
                foreach (CommandPlayer player in playerList)
                {
                    isPlaying = isPlaying || player.IsPlaying;
                }
            }
            // サブプレイヤーの終了チェック
            priority = CommandPlayerPriority.SubPlayer;
            if (_commandPlayers.TryGetValue(priority, out var subList))
            {
                foreach (CommandPlayer player in subList)
                {
                    isPlaying = isPlaying || player.IsPlaying;
                }
            }
        } while (isPlaying);


        _coroutine = null;
    }

}
