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
    [SerializeField] private RectTransform _goalResult;
    [SerializeField] private Text _nodeCountText;
    private bool _isGoal = false;

    // game over 
    [SerializeField] private RectTransform _gameOverResult;
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
        _isGoal = true; _isGameOver = true;


        _player?.OnStop();
        StartCoroutine(OnGoalCoroutine());
    }
    private IEnumerator OnGoalCoroutine()
    {        
        // se
        yield return new WaitForSeconds(0.5f);



        Vector2 start = _goalResult.anchoredPosition;
        Vector2 end = Vector2.zero;
        for(float t = 0.0f; t < 0.7f; t+= Time.deltaTime)
        {
            float value = t / 0.7f;

            _goalResult.anchoredPosition = Vector2.Lerp(start, end, value);

            yield return null;
        }


        yield return new WaitForSeconds(1.0f);


        // 
        int count = 0;
        foreach (var visual in _visuals)
        {
            count += visual.GetNodeCount();
            count --; // ルートノードは除外
        }

        for(float t = 0.0f; t < 1.0f; t+= Time.deltaTime)
        {
            int c = Mathf.FloorToInt( t * count );

            if (_nodeCountText != null)
            {
                _nodeCountText.text = c.ToString() + " コ";
            }

            yield return null;
        }

        if (_nodeCountText != null)
        {
            _nodeCountText.text = count.ToString() + " コ";
        }



        _goalResult.anchoredPosition = end;
    }

    public void OnGameOver()
    {
        if(_isGameOver == true)
        {
            return;
        }
        _isGoal = true; _isGameOver = true;


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

        Vector2 start = _gameOverResult.anchoredPosition;
        Vector2 end = Vector2.zero;
        for (float t = 0.0f; t < 0.7f; t += Time.deltaTime)
        {
            float value = t / 0.7f;

            _gameOverResult.anchoredPosition = Vector2.Lerp(start, end, value);

            yield return null;
        }

        _gameOverResult.anchoredPosition = end;
    }

}
