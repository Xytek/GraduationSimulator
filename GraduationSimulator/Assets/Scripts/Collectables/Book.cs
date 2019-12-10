using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Book : Interactable
{      
    void Awake()
    {
        EventManager.StartListening("Research2Unlocked", Unlock);
        EventManager.StartListening("Research3Unlocked", ChangeTime);
    }

    private void OnDestroy()
    {
        EventManager.StopListening("Research2Unlocked", Unlock);
        EventManager.StopListening("Research3Unlocked", ChangeTime);
    }
}
