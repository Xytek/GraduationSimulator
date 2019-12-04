using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public Player player;

    // just for the reset at quit()
    public CourseData[] courseData;

    [SerializeField] private Menu mainMenu;
    [SerializeField] private Menu pauseMenu;
    [SerializeField] private Menu courseMenu;
    [SerializeField] private InstructionPanel instructionPanel;
    private Menu activeMenu;
    
    public void Awake()
    {
        StartGame();
        EventManager.StartListening("ShowInstructions", ActivateInstructionPanel);
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            if (courseMenu.CheckIfActive())
            {
                ResumeGame();
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
                ResumeGame();
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

    public void ActivateInstructionPanel(EventParams param)
    {
        instructionPanel.Activate();
        instructionPanel.UpdatePanel(param);
        player.Freeze();
        if(activeMenu != null)
        {
            activeMenu.Deactivate();
        }        
        activeMenu = (Menu) instructionPanel;
    }

    public void ResumeGame()
    {
        player.Unfreeze();
        if(activeMenu != null)
        {
            activeMenu.Deactivate();
        }        
        activeMenu = null;
    }

    public void Quit()
    {     
        Debug.Log("Quit");
        Application.Quit(0);
    }  
}
