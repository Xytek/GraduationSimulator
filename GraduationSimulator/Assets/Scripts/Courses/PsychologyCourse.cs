using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PsychologyCourse : Course
{
    public override void Upgrade()
    {
        base.SendUpgrade();
        switch (_upgradeLevel)
        {
            case 1:
                // unlock apples
                EventManager.TriggerEvent("Psychology1Unlocked", new EventParams());
                break;
            case 2:
                // decrease cooldown-times of apples
                EventParams param = new EventParams();
                param.intNr = 2;
                EventManager.TriggerEvent("Psychology2Unlocked", param);
                break;
            case 3:
                // shorten vial coolDownTime                
                EventManager.TriggerEvent("Psychology3Unlocked", new EventParams());
                break;
        }
    }
}
