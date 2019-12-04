using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public Player player;
    private NPCList _npcList;

    // just for the reset at quit()
    public CourseData[] courseData;
    
    [SerializeField] private Menu pauseMenu;
    [SerializeField] private Menu courseMenu;
    [SerializeField] private InstructionPanel instructionPanel;
    private Menu activeMenu;


    public void Start()
    {
        pauseMenu.Deactivate();
        courseMenu.Deactivate();
        activeMenu = null;
        if (_npcList == null)
            GetNPCList();            
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

    public void ChangeMenu(Menu newMenu)
    {
        if(activeMenu != null)
        {
            activeMenu.Deactivate();            
        }
        activeMenu = newMenu;
        FreezeScene();
        newMenu.Activate();
    }

    public void ActivateInstructionPanel(EventParams param)
    {
        instructionPanel.Activate();
        instructionPanel.UpdatePanel(param);
        FreezeScene();
        if(activeMenu != null)
        {
            activeMenu.Deactivate();
        }        
        activeMenu = (Menu) instructionPanel;
    }

    public void ResumeGame()
    {
        UnfreezeScene();
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

    private void FreezeScene()
    {
        if (_npcList == null)
            GetNPCList();
        _npcList.FreezeNPCs();
        player.Freeze();
    }

    private void UnfreezeScene()
    {
        if (_npcList == null)
            GetNPCList();
        _npcList.ResumeNPCs();
        player.Unfreeze();
    }

    private void GetNPCList()
    {
        _npcList = GameObject.FindGameObjectWithTag("NPCList").GetComponent<NPCList>();
        if (_npcList == null) Debug.LogError("Couldn't find the npc list");
    }
}
