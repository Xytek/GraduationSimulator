using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScienceCourse : Course
{  
    public override void Activate()
    {                      
        EventManager.TriggerEvent("ScienceCourseUnlocked", new EventParams());
    }    
}
