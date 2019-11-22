using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public Player player;
    public CourseData[] courseData;

    public Menu mainMenu;
    public Menu pauseMenu;
    public Menu courseMenu;
    private Menu activeMenu;
    

    // function that starts a menu
    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            if (courseMenu.CheckIfActive())
            {                
                player.Unfreeze();
                courseMenu.Deactivate();
            }
            else
            {
                ((CourseMenu)courseMenu).ActivateAffordableCourses(player);
                ChangeMenu(courseMenu);                
            }
        } else if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (pauseMenu.CheckIfActive())
            {
                player.Unfreeze();
                pauseMenu.Deactivate();
            }
            else
            {
                ChangeMenu(pauseMenu);
            }
        }
    }

    public void StartGame()
    {

    }

    public void ChangeMenu(Menu newMenu)
    {
        if(activeMenu != null)
        {
            activeMenu.Deactivate();            
        }
        activeMenu = newMenu;
        player.Freeze();
        newMenu.Activate();
    }

    public void ResumeGame()
    {
        player.Unfreeze();
        activeMenu.Deactivate();
        activeMenu = null;
    }

    public void Quit()
    {
        foreach (CourseData data in courseData)
        {
            data.ResetUpgradeLvl();
        }
        Debug.Log("Quit");
        Application.Quit(0);
    }  
}
