/*
    SceneManager_LevelScene.cs
    20260802  arai eito
    レベルシーンのマネージャー
*/
using System.Collections;
using UnityEngine;

public class SceneManager_LevelScene : SceneManager_Base
{
    // ==================================================
    // ----- Priority -----
    // ==================================================

    private BlockManager _blockManager;
    private ScenePlayer _player;

    [SerializeField] private UI_ImageChanger _visibleButtonImageChanger;
    private CommandVisual[] _visuals;
    private bool _commandVisible = true;


    private bool _isCanPlay = true;

    // ==================================================
    // ----- Unity Events -----
    // ================================================== 
    private void Awake()
    {
        _blockManager = GetComponentInChildren<BlockManager>();
        _player = GetComponentInChildren<ScenePlayer>();
        _visuals = GetComponentsInChildren<CommandVisual>();

        SetCommandVisible(true);
    }

    // ==================================================
    // ----- UI Events -----
    // ==================================================    
    // Stop 同義
    public void OnResetButtonClick()
    {
        StartCoroutine(OnResetButtonClickCoroutine());
    }
    private IEnumerator OnResetButtonClickCoroutine()
    {
        _isCanPlay = false;

        _player?.OnStop();

        if(_blockManager != null)
        {
            yield return _blockManager.ResetBlock();
        }

        _isCanPlay = true;
    }
    public void OnPlayButtonClick()
    {
        if(!_isCanPlay)
        {
            return;
        }

        StartCoroutine(OnPlayButtonClickCoroutine());
    }
    private IEnumerator OnPlayButtonClickCoroutine()
    {
        yield return OnResetButtonClickCoroutine();

        yield return new WaitForSeconds(0.5f);

        _player?.OnRun();
    }
    public void OnCommandVisibleButtonClick()
    {
        _commandVisible = !_commandVisible;
        SetCommandVisible(_commandVisible);
    }
    private void SetCommandVisible(bool visible)
    {
        _commandVisible = visible;

        _visibleButtonImageChanger?.ChangeImageSprite(visible);
        foreach(var visual in _visuals)
        {
            visual.Visible = visible;
        }
    }

}
