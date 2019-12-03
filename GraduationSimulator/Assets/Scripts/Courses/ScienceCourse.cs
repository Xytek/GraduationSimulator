using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScienceCourse : Course
{
    public override void Upgrade()
    {
        base.SendUpgrade();
        switch (_upgradeLevel)
        {
            case 1:                
                EventManager.TriggerEvent("Science1Unlocked", new EventParams());
                break;
            case 2:                
                EventManager.TriggerEvent("Science2Unlocked", new EventParams());
                break;
            case 3:
                EventManager.TriggerEvent("Science3Unlocked", new EventParams());
                break;
        }
    }
}
