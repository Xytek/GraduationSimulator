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
                Debug.Log("level 1 in science achieved");
                EventManager.TriggerEvent("FirstScienceCourseUnlocked", new EventParams());
                break;
            case 2:
                // activate ThrowVialAbility
                Debug.Log("level 2 in science achieved");
                break;
            case 3:
                // shorten vial coolDownTime
                Debug.Log("level 3 in science achieved");
                break;
        }
    }
}
