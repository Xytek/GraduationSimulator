using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Course
{    
    protected int _upgradeLevel = 0;   
    protected int _maxTiers = 2;   
    
    public abstract void Upgrade(CourseData data);
}
