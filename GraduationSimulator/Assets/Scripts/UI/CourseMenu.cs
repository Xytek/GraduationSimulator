﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CourseMenu : Menu
{
    public static bool CourseMenuIsOpen = false;
    public GameObject CourseMenuUI;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            if (CourseMenuIsOpen)
            {
                Resume();
            } else
            {
                Pause();
            }
        }
    }

    public override void Pause()
    {
        Cursor.lockState = CursorLockMode.Confined;
        // freeze Szene
        CourseMenuIsOpen = true;
        CourseMenuUI.SetActive(true);
    }

    public override void Resume()
    {
        Cursor.lockState = CursorLockMode.Locked;
        CourseMenuIsOpen = false;
        CourseMenuUI.SetActive(false);
    }

    public void ActivateScienceCourse()
    {        
        CourseFactory.GetCourse(CourseFactory.CourseTypes.science).Activate();
        Resume();
    }

    public void ActivatePsychologyCourse()
    {        
        CourseFactory.GetCourse(CourseFactory.CourseTypes.psychology).Activate();
        Resume();
    }

    public void ActivateHackingCourse()
    {
        CourseFactory.GetCourse(CourseFactory.CourseTypes.psychology).Activate();
        Resume();
    }
}