using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScienceCourse : Course
{
    public DoorTrigger ScienceDoor;

    public override void Activate()
    {
        _upgradeLevel++;
        EventManager.TriggerEvent("ScienceCourseUnlocked", new EventParams());
    }   
    
    public override void FirstUpgrade()
    {
        //ScienceDoor.Unlock();
    }
    public override void SecondUpgrade()
    {

    }
}
