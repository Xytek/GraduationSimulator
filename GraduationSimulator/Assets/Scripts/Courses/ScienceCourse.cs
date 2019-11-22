using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScienceCourse : Course
{
    private CourseData _courseData;
    public override void Upgrade(CourseData data)
    {
        _courseData = data;
        if (_courseData.UpgradeLevel <= _maxTiers)
        {
            _courseData.UpgradeLevel++;

            EventParams param = new EventParams();
            param.courseType = _courseData.type;
            param.number = _courseData.UpgradeLevel;
            EventManager.TriggerEvent("CourseUpgrade", new EventParams());

            switch (_courseData.UpgradeLevel)
            {
                case 1:                    
                    EventManager.TriggerEvent("CourseUpgrade", new EventParams());
                    Debug.Log("level 1 in science achieved");
                    break;
                case 2:                    
                    EventManager.TriggerEvent("Upgrade", new EventParams());
                    Debug.Log("level 2 in science achieved");
                    break;                
            }
        }            
    }
}
