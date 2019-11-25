using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScienceCourse : Course
{
    private CourseData _courseData;
    public new void Upgrade(CourseData data)
    {
        _courseData = data;
        if (_courseData.UpgradeLevel <= _maxTiers)
        {
            base.Upgrade(data);

            EventParams param = new EventParams();
            param.courseType = _courseData.type;
            param.number = _courseData.UpgradeLevel;
            EventManager.TriggerEvent("CourseUpgrade", new EventParams());

            switch (_courseData.UpgradeLevel)
            {
                case 1:                                    
                    // open science door
                    Debug.Log("level 1 in science achieved");
                    break;
                case 2:                 
                    // enable vial
                    Debug.Log("level 2 in science achieved");
                    break;
                case 3:
                    // shorten vial coolDownTime
                    Debug.Log("level 3 in science achieved");
                    break;
            }
        }            
    }
}
