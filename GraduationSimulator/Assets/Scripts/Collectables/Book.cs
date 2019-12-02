using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Book : Interactable
{   
    public Book()
    {
        _useTime = 5;
        _used = false;
        _creditAmount = 6;
        _radius = 1f;
    }

    void Awake()
    {
        EventManager.StartListening("Research2Unlocked", Unlock);
        EventManager.StartListening("Research3Unlocked", ChangeTime);
    }
}
