using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HackingCourse : Course
{
    public HackingCourse()
    {

    }
    public override void Upgrade(CourseData data)
    {
        Debug.Log("Hacking Course activated!");
    }
}
