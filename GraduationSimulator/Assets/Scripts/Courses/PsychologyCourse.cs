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
                EventManager.TriggerEvent("Psychology1Unlocked", new EventParams());
                break;
            case 2:
                // activate ThrowVialAbility         
                EventManager.TriggerEvent("Psychology2Unlocked", new EventParams());
                break;
            case 3:
                // shorten vial coolDownTime                
                EventManager.TriggerEvent("Psychology3Unlocked", new EventParams());
                break;
        }
    }
}
