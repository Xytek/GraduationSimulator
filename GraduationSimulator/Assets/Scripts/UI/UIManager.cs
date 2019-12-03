using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public Player player;

    // just for the reset at quit()
    public CourseData[] courseData;

    public Menu mainMenu;
    public Menu pauseMenu;
    public Menu courseMenu;
    private Menu activeMenu;

    public void Awake()
    {
        StartGame();
    }

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
                ((CourseMenu)courseMenu).UpdateCoursePanels();
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
        //mainMenu.Activate();
        // _player.Freeze();
        courseMenu.Deactivate();
        pauseMenu.Deactivate();
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
        Debug.Log("Quit");
        Application.Quit(0);
    }  
}
