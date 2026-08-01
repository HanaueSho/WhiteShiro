/*
    FadeManager.cs
    20260801  hanaue sho
    フェードマネージャー
*/
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class FadeManager : MonoBehaviour
{
    // フェードイン、アウト
    // フェード実行
    [SerializeField] private bool _isStartFadeIn = true;
    [SerializeField] private float _fadeDuratino = 0.5f;
    [SerializeField] private Canvas _fadeCanvas;
    [SerializeField] private Image _fadeImage;

    private void Awake()
    {
        if (_fadeCanvas == null)
        {
            _fadeCanvas = GetComponentInChildren<Canvas>(includeInactive: true);
        }
        _fadeCanvas.gameObject.SetActive(true); // 有効化

        if (_fadeImage == null)
        {
            _fadeImage = GetComponentInChildren<Image>();
        }

        // ----- Start FadeIn -----
        if (_isStartFadeIn)
        {
            Color color = _fadeImage.color;
            color.a = 1.0f;
            _fadeImage.color = color;
            StartCoroutine(FadeIn());
        }
        else
        {
            Color color = _fadeImage.color;
            color.a = 0.0f;
            _fadeImage.color = color;
        }
    }

    public IEnumerator FadeIn()
    {
        Debug.Log("[Debug] FadeIn");
        yield return Fade(true);
    }

    public IEnumerator FadeOut()
    {
        Debug.Log("[Debug] FadeOut");
        yield return Fade(false);
    }

    private IEnumerator Fade(bool isFadeIn)
    {
        if (_fadeImage == null)
        {
            yield break;
        }

        float elapsedTime = 0.0f;
        Color color = _fadeImage.color;
        while (elapsedTime < _fadeDuratino)
        {
            elapsedTime += Time.deltaTime;

            float t = Mathf.Clamp01(elapsedTime / _fadeDuratino);
            if (isFadeIn)
            {
                color.a = 1.0f - t;
            }
            else
            {
                color.a = t;
            }
            _fadeImage.color = color;

            yield return null;
        }

        if (isFadeIn)
        {
            color.a = 0.0f;
        }
        else
        {
            color.a = 1.0f;
        }

        yield break;
    }


    public IEnumerator FadeOutIn(UnityAction _action)
    {
        // FadeOut
        yield return FadeOut();
        yield return new WaitForSeconds(1.0f);

        // MoveScece
        _action?.Invoke();


        yield return new WaitForSeconds(1.0f);
        // FadeIn
        yield return FadeIn();

    }

}
