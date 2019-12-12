using UnityEngine;

public class UIManager : MonoBehaviour
{
    public Player player;
    private NPCList _npcList;

    // just for the reset at quit()
    public CourseData[] courseData;

    [SerializeField] private Menu _pauseMenu = default;
    [SerializeField] private Menu _courseMenu = default;
    [SerializeField] private InstructionPanel _instructionPanel = default;
    private Menu _activeMenu;

    public void Start()
    {
        _pauseMenu.Deactivate();
        _courseMenu.Deactivate();
        _activeMenu = null;
        if (_npcList == null)
            GetNPCList();
        EventManager.StartListening("ShowInstructions", ActivateInstructionPanel);
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

    public void ActivateInstructionPanel(EventParams param)
    {
        _instructionPanel.Activate();
        _instructionPanel.UpdatePanel(param);
        FreezeScene();
        if (_activeMenu != null)
            _activeMenu.Deactivate();

        _activeMenu = (Menu)_instructionPanel;
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
