/*
    GameManager.cs
    20260801  hanaue sho
    ゲームを統括するマネージャー
*/
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    static GameManager _instance;
    private FadeManager _fadeManager;

    private void Awake()
    {
        // シングルトン
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        
        // イベント登録
        SceneManager.sceneLoaded += OnSceneLoaded;

        // FadeManager
        _fadeManager = GetComponentInChildren<FadeManager>();
        if (_fadeManager == null )
        {
            Debug.LogError("[Warning] No FadeManager in GameManager!!!");
        }

    }

    private void OnDisable()
    {
        if (_instance == this)
        {
            // イベント削除
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log($"シーン読み込み完了: {scene.name}");
        Debug.Log($"読み込みモード: {mode}");

        // SceneManager を探す
        SceneManager_Base sm = GameObject.FindAnyObjectByType<SceneManager_Base>();
        if (sm != null)
        {
            sm.Enter();
            sm.NextSceneMoveAction = SceneMove;
        }
        else
        {
            Debug.LogError("[Error] Not Find SceneManager_Base!!!");
        }
    }

    private void SceneMove(int sceneIndex)
    {
        Debug.Log("[Debug] シーン移動します");

        if (_fadeManager != null)
        {
            StartCoroutine(_fadeManager.FadeOutIn(() => SceneManager.LoadScene(sceneIndex)));
        }
        else
        {
            SceneManager.LoadScene(sceneIndex);
        }
    }

}
