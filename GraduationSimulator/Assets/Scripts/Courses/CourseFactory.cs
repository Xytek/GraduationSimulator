using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CourseFactory
{        
    public static Course GetCourse(CourseTypes courseName)
    {
        switch (courseName)
        {
            case CourseTypes.Science:
                return new ScienceCourse();
            case CourseTypes.Psychology:
                return new PsychologyCourse();
            case CourseTypes.Hacking:
                return new HackingCourse();
            case CourseTypes.Research:
                return new HackingCourse();
            case CourseTypes.Sports:
                return new HackingCourse();
            default:
                return null;
        }
    }
}

