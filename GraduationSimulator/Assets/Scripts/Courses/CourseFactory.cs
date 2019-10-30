using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CourseFactory
{    
    public enum CourseTypes {science, psychology, hacking};
    public static Course GetCourse(CourseTypes courseName)
    {
        switch (courseName)
        {
            case CourseTypes.science:
                return new ScienceCourse();
            case CourseTypes.psychology:
                return new PsychologyCourse();
            case CourseTypes.hacking:
                return new HackingCourse();
            default:
                return null;
        }
    }
}

