/*
    CommandVisualToCommandPlayer.cs
    CommandPlayer.cs
    20260731 arai eito 
    PlayerとVisualをつなぐクラス
*/
using UnityEngine;

public class CommandVisualToCommandPlayer : MonoBehaviour
{
    // ==================================================
    // ----- Priority -----
    // ==================================================
    [SerializeField] private CommandVisual _visual;
    [SerializeField] private CommandPlayer _player;
    [SerializeField] private bool _playOnAwake;

    // ==================================================
    // ----- Public Event -----
    // ==================================================
    private void Start()
    {
        if(_playOnAwake)
        {
            VisualToPlayer();
        }
    }
    public void VisualToPlayer()
    {
        if(_visual == null || _player == null)
        {
            return;
        }

        _visual.SetCommandPlayer(_player);
    }
}
