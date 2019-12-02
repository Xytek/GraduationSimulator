using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laptop : Interactable
{    
    public Laptop()
    {
        _useTime = 5;
        _used = false;
        _creditAmount = 6;
        _radius = 1f;
    }
    void Awake()
    {                
        EventManager.StartListening("Hacking2Unlocked", Unlock);
        EventManager.StartListening("Hacking3Unlocked", ChangeTime);
    }
}
