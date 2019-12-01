using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class HackingCourse : Course
{
    public override void Upgrade()
    {
        base.SendUpgrade();
        switch (_upgradeLevel)
        {
            case 1:
                Debug.Log("level 1 in hacking achieved");
                break;
            case 2:
                // activate ThrowVialAbility
                Debug.Log("level 2 in hacking achieved");
                break;
            case 3:
                // shorten vial coolDownTime
                Debug.Log("level 3 in hacking achieved");
                break;
        }
    }
}
