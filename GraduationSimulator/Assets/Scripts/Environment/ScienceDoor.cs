﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ScienceDoor : Door
{    
    public void Awake()
    {        
        EventManager.StartListening("Science1Unlocked", Unlock);
    }

    public void Unlock(EventParams e)
    {
        _locked = false;
    }
}
