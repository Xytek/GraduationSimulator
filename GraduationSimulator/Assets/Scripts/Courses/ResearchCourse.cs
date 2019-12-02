using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResearchCourse : Course
{
    public override void Upgrade()
    {
        base.SendUpgrade();
        switch (_upgradeLevel)
        {
            case 1:
                EventManager.TriggerEvent("Research1Unlocked", new EventParams());
                break;
            case 2:
                EventManager.TriggerEvent("Research2Unlocked", new EventParams());
                break;
            case 3:
                // decrease usage-time of laptops
                EventParams param = new EventParams();
                param.number = 2;
                EventManager.TriggerEvent("Research3Unlocked", param);
                break;
        }
    }
}
