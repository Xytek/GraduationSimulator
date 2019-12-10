using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laptop : Interactable
{        
    void Awake()
    {                
        EventManager.StartListening("Hacking2Unlocked", Unlock);
        EventManager.StartListening("Hacking3Unlocked", ChangeTime);
    }
    private void OnDestroy()
    {
        EventManager.StopListening("Hacking2Unlocked", Unlock);
        EventManager.StopListening("Hacking3Unlocked", ChangeTime);
    }
}
