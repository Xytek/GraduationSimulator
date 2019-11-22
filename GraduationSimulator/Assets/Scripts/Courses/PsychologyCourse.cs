using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PsychologyCourse : Course
{    

    public override void Upgrade(CourseData data)
    {         
        Debug.Log("Psychology Course activated!");
    }
}
