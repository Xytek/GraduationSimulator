using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : Menu
{
    public static bool PauseMenuIsActive = false;
    public GameObject CourseMenuUI;    

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (PauseMenuIsActive)
            {                
                Resume();
            }
            else
            {
                Pause();
            }
        }
    } 

    public override void Pause()
    {
        player.Freeze();
        PauseMenuIsActive = true;
        CourseMenuUI.SetActive(true);
    }

    public override void Resume()
    {
        player.Unfreeze();
        PauseMenuIsActive = false;
        CourseMenuUI.SetActive(false);
    }

    public void Quit()
    {
        Debug.Log("Quit");
        Application.Quit(0);
    }
}
