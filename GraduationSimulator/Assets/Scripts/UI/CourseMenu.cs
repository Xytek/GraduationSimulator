using System.Collections;
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
        player.Freeze();
        CourseMenuIsOpen = true;
        CourseMenuUI.SetActive(true);
    }

    public override void Resume()
    {
        player.Unfreeze();
        CourseMenuIsOpen = false;
        CourseMenuUI.SetActive(false);
    }    
}
