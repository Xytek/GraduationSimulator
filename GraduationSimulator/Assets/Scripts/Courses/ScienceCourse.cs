using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScienceCourse : Course
{
    public override void Activate()
    {
        if(_upgradeLevel < _maxTiers)
            _upgradeLevel++;
        switch (_upgradeLevel)
        {
            case 1:
                EventManager.TriggerEvent("FirstScienceCourseUnlocked", new EventParams());
                Debug.Log("level 1 in science achieved");
                break;
            case 2:
                EventManager.TriggerEvent("SecondScienceCourseUnlocked", new EventParams());
                Debug.Log("level 2 in science achieved");
                break;
            default:
                Debug.LogError("There is no setting for upgrade level " + _upgradeLevel);
                break;
        }
    }   
    
    public override void FirstUpgrade()
    {
        //ScienceDoor.Unlock();
    }
    public override void SecondUpgrade()
    {

    }
}
