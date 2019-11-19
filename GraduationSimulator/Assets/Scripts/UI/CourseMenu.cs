using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CourseMenu : Menu
{
    public static bool CourseMenuIsOpen = false;
    public GameObject CourseMenuUI;

    private CoursePanel[] coursePanels;

    public void Awake()
    {
        // get all the panels for the different courses, also the ones that are currently diabled
        coursePanels = GetComponentsInChildren<CoursePanel>(true);
    }

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
        player.Freeze();      
        CourseMenuIsOpen = true;
        CourseMenuUI.SetActive(true);

        // check if the player can afford a course        
        foreach (CoursePanel panel in coursePanels)
        {
            panel.UpdatePriceText();
            panel.CheckIfAffordable(player);
        }
    }

    public override void Resume()
    {
        player.Unfreeze();
        CourseMenuIsOpen = false;
        CourseMenuUI.SetActive(false);
    }       
}
