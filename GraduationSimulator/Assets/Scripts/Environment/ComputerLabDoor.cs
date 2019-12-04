using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ComputerLabDoor : Door
{    
    public void Awake()
    {        
        EventManager.StartListening("Hacking1Unlocked", Unlock);
    }

    public void Unlock(EventParams e)
    {
        _locked = false;
    }
}
