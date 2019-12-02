using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SportCourse : Course
{
    public override void Upgrade()
    {
        base.SendUpgrade();
        switch (_upgradeLevel)
        {
            case 1:
                Debug.Log("first sport");
                EventManager.TriggerEvent("Sport1Unlocked", new EventParams());
                break;
            case 2:
                Debug.Log("scnd sport");
                EventManager.TriggerEvent("Sport2Unlocked", new EventParams());
                break;
            case 3:
                Debug.Log("thrd sport");
                EventManager.TriggerEvent("Sport3Unlocked", new EventParams());
                break;
        }
    }
}
