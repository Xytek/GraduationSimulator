using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeSelectButton : MonoBehaviour
{
    public Sprite selectedSprite;
    public Sprite inactiveSprite;
    public Sprite activeSprite;
    private bool _achieved = false;
    private Image _image;

    public void Awake()
    {
        _image = GetComponent<Image>();
    }

    public void LvlUnselected()
    {
        if (_achieved)
        {
            _image.color = Color.white;
        } else
        {
            _image.color = Color.white;
            this.GetComponent<Image>().sprite = inactiveSprite;
        }        
    }

    public void LvlSelected()
    {
        if (_achieved)
        {
            _image.color = Color.gray;
        }
        else
        {
            _image.color = Color.white;
            this.GetComponent<Image>().sprite = selectedSprite;
        }        
    }

    public void LvlAchieved()
    {
        _achieved = true;
        _image.sprite = activeSprite;
    }
}
