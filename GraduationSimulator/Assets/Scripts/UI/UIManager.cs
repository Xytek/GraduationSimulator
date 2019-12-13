using UnityEngine;
using System.Collections.Generic;


public class UIManager : MonoBehaviour
{
    public Player player;
    private NPCList _npcList;

    [SerializeField] private Menu _pauseMenu = default;
    [SerializeField] private Menu _courseMenu = default;
    [SerializeField] private InstructionPanel _instructionPanel = default;
    [SerializeField] private SemesterOverUI _semesterOverPanel = default;
    private Menu _activeMenu;

    public void Start()
    {        
        _instructionPanel.SetPanelText("Collect as much credits as possible in a semester. Avoid the teachers! Press F to get into the Course-Shop and Escape to pause the game!");
        ChangeMenu((Menu)_instructionPanel);
        Cursor.lockState = CursorLockMode.None;

        _pauseMenu.Deactivate();
        _courseMenu.Deactivate();

        if (_npcList == null)
            GetNPCList();

        EventManager.StartListening("ShowInstructions", ActivateInstructionPanel);
        EventManager.StartListening("SemesterOver", ActivateSemesterOverPanel);
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            if (_courseMenu.CheckIfActive())
                ResumeGame();
            else
            {
                ((CourseMenu)_courseMenu).UpdateCoursePanels();
                ChangeMenu(_courseMenu);
            }
        }
        else if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (_pauseMenu.CheckIfActive())
                ResumeGame();
            else
                ChangeMenu(_pauseMenu);
        }
    }

    public void ChangeMenu(Menu newMenu)
    {
        if (_activeMenu != null)
            _activeMenu.Deactivate();

        _activeMenu = newMenu;
        FreezeScene();
        newMenu.Activate();
    }

    public void ResumeGame()
    {
        UnfreezeScene();
        if (_activeMenu != null)
            _activeMenu.Deactivate();

        _activeMenu = null;
    }

    public void Quit()
    {
        Debug.Log("Quit");
        Application.Quit(0);
    }

    public void ActivateInstructionPanel(EventParams param)
    {
        _instructionPanel.UpdatePanel(param);
        ChangeMenu((Menu)_instructionPanel);
    }

    public void ActivateSemesterOverPanel(EventParams param)
    {
        _semesterOverPanel.UpdatePanel(param);
        ChangeMenu((Menu)_semesterOverPanel);
    }

    private void FreezeScene()
    {
        if (_npcList == null || _npcList.NewTeachers)
        {
            GetNPCList();
            _npcList.NewTeachers = false;
        }

        _npcList.FreezeNPCs();
        player.Freeze();
    }

    private void UnfreezeScene()
    {
        if (_npcList == null || _npcList.NewTeachers)
        {
            GetNPCList();
            _npcList.NewTeachers = false;
        }
        _npcList.ResumeNPCs();
        player.Unfreeze();
    }

    private void GetNPCList()
    {
        _npcList = GameObject.FindGameObjectWithTag("NPCList").GetComponent<NPCList>();
        if (_npcList == null) Debug.LogError("Couldn't find the npc list");
    }
}
