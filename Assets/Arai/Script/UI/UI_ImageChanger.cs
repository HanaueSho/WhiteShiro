/*
    UI_ImageChanger.cs
    20260802  arai eito
    UIのImageをかえるだけ
*/
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_ImageChanger : UI_Base
{

    // ==================================================
    // ----- Priority -----
    // ==================================================
    [SerializeField] private List<Sprite> _sprites;
    private Image _image;



    // ==================================================
    // ----- Unity Events -----
    // ==================================================
    private void Awake()
    {
        _image = GetComponent<Image>();
        if(_image == null )
        {
            Debug.LogWarning("UI_ImageChanger : Image がありません。");
        }
    }


    // ==================================================
    // ----- Public Events -----
    // ==================================================
    public void ChangeImageSprite(bool value)
    {
        ChangeImageSprite(value ? 1 : 0);
    }

    public void ChangeImageSprite(int index)
    {
        if (_image == null)
        {
            return;
        }

        if (index < 0 || index >= _sprites.Count)
        {
            return;
        }

        _image.sprite = _sprites[index];
    }

    public void ChangeImageSprite(string name)
    {
        if (_image == null)
        {
            return;
        }

        Sprite sprite = _sprites.Find(x => x != null && x.name == name);

        if (sprite != null)
        {
            _image.sprite = sprite;
        }
    }
}
