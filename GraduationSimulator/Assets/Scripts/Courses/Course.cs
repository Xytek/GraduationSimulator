using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public abstract class Course
{
    protected int _upgradeLevel;
    protected int _maxTiers = 3;
    protected CourseTypes _type;

    public void Initialize(CourseTypes type)
    {
        _type = type;
        _upgradeLevel = 0;
    }

    public int UpgradeLevel
    {
        get { return _upgradeLevel; }        
        set { _upgradeLevel = value; }
    }

    public void SendUpgrade()
    {
        if (_upgradeLevel < _maxTiers)
        {
            UpgradeLevel++;
            EventParams param = new EventParams();
            param.number = _upgradeLevel;
            param.courseType = _type;
            EventManager.TriggerEvent("CourseUpgrade", param);
        }
    }
    public abstract void Upgrade();
}
