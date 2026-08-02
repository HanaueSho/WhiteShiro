/*
    SceneManager_LevelScene.cs
    20260802  arai eito
    レベルシーンのマネージャー
*/
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class SceneManager_LevelScene : SceneManager_Base
{
    // ==================================================
    // ----- Priority -----
    // ==================================================

    private BlockManager _blockManager;
    private ScenePlayer _player;

    // command visual 
    [SerializeField] private UI_ImageChanger _visibleButtonImageChanger;
    private CommandVisual[] _visuals;
    private bool _commandVisible = true;


    // ui
    // goal
    [SerializeField] private Transform _goalResult;
    [SerializeField] private Text _nodeCountText;
    private bool _isGoal = false;

    // game over 
    [SerializeField] private Transform _gameOverResult;
    private bool _isGameOver = false;


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



    // ==================================================
    // ----- Public Events -----
    // ==================================================  
    public void OnGoal()
    {
        if(_isGoal == true)
        {
            return;
        }
        _isGoal = true;


        _player?.OnStop();
        StartCoroutine(OnGoalCoroutine());
    }
    private IEnumerator OnGoalCoroutine()
    {        
        // se
        yield return new WaitForSeconds(0.5f);



        int count = 0;
        foreach(var visual in _visuals)
        {
            count += visual.GetNodeCount();
        }

        if(_nodeCountText != null)
        {
            _nodeCountText.text = count.ToString();
        }

        Vector3 start = _goalResult.position;
        Vector3 end = Vector3.zero;
        for(float t = 0.0f; t < 0.7f; t+= Time.deltaTime)
        {
            float value = t / 0.7f;

            _goalResult.position = Vector3.Lerp(start, end, value);

            yield return null;
        }

        _goalResult.position = end;
    }

    public void OnGameOver()
    {
        if(_isGameOver == true)
        {
            return;
        }
        _isGameOver = true;


        _player?.OnStop();
        StartCoroutine(OnGameOverCoroutine());
    }
    private IEnumerator OnGameOverCoroutine()
    {
        // se
        yield return new WaitForSeconds(0.5f);



        int count = 0;
        foreach (var visual in _visuals)
        {
            count += visual.GetNodeCount();
        }

        if (_nodeCountText != null)
        {
            _nodeCountText.text = count.ToString();
        }

        Vector3 start = _gameOverResult.position;
        Vector3 end = Vector3.zero;
        for (float t = 0.0f; t < 0.7f; t += Time.deltaTime)
        {
            float value = t / 0.7f;

            _gameOverResult.position = Vector3.Lerp(start, end, value);

            yield return null;
        }

        _gameOverResult.position = end;
    }

}
