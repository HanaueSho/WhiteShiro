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

    // ==================================================
    // ----- Public Event -----
    // ==================================================
    public void VisualToPlayer()
    {
        if(_visual == null || _player == null)
        {
            return;
        }

        _visual.SetCommandPlayer(_player);
    }
}
