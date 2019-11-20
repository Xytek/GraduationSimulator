using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Course
{
    protected int _upgradeLevel = 0;   
    
    public abstract void Activate();
    public abstract void FirstUpgrade();
    public abstract void SecondUpgrade();
}
