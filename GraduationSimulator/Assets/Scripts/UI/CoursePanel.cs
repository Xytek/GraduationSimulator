using UnityEngine;
using UnityEngine.UI;

public class CoursePanel : MonoBehaviour
{
    public CourseData courseData;
    private PlayerStats _playerStats;

    [Header("UI Elements")]
    public Text title;
    public Text description;
    public Text priceText;
    public UpgradeButton upgradeButton;
    public Image icon;

    private int _selectedLvl;
    public UpgradeSelectButton[] upgradeSelectButtons;

    private int _upgradeLvl;

    public void Awake()
    {
        _playerStats = GameObject.FindWithTag("Player").GetComponent<PlayerStats>();
        if (_playerStats == null) Debug.LogError("No player stats found");        

        icon.sprite = courseData.icon;
        _upgradeLvl = 0;
        _selectedLvl = _upgradeLvl;
        UpdateUI(_selectedLvl);
        EventManager.StartListening("CourseUpgrade", UpgradePanel);
    }

    public void UpdateUI(int lvl)
    {
        priceText.text = courseData.prices[lvl].ToString();
        title.text = courseData.type.ToString();
        description.text = courseData.UpgradeDescriptions[lvl];
    }

    public int GetUpgradeLevelArray()
    {
        int lvl = _upgradeLvl - 1;
        if (lvl < 0)
            lvl = 0;
        return lvl;
    }

    // checks the affordability in the courseData prices
    public bool CheckIfLvlIsAffordable(int level, int creditCount)
    {
        return creditCount >= courseData.prices[level];
    }

    // sets the upgradeLvl for all LvlButtons of the panel
    public void SetAllUpgradeLvls()
    {
        for (int i = 0; i < _upgradeLvl; i++)
            upgradeSelectButtons[i].LvlAchieved();
    }

    // sets the clicked level selected
    private void SetLvlSelection()
    {
        for (int i = 0; i < upgradeSelectButtons.Length; i++)
            if (i == _selectedLvl)
                upgradeSelectButtons[i].LvlSelected();
            else
                upgradeSelectButtons[i].LvlUnselected();
    }

    // changes to the selected level and checks if the upgrade-button should be active or not
    public void ChangeSelectedLvl(int chosenLvl)
    {        
        if (chosenLvl == 0 && CheckIfLvlIsAffordable(chosenLvl, _playerStats.Credits) && _upgradeLvl < 1)
        {            
            upgradeButton.Activate();
        }            
        else if (chosenLvl == _upgradeLvl && CheckIfLvlIsAffordable(chosenLvl, _playerStats.Credits))
        {         
            upgradeButton.Activate();
        }            
        else if(chosenLvl < _upgradeLvl)
        {            
            upgradeButton.Used();
        }
        else
        {
            upgradeButton.Deactivate();
        }
            
        _selectedLvl = chosenLvl;
        UpdateUI(chosenLvl);
        SetLvlSelection();
    }

    public void UpdatePanel()
    {
        ChangeSelectedLvl(_selectedLvl);
        SetAllUpgradeLvls();
    }

    // upgrades the panel on upgrade event    
    public void UpgradePanel(EventParams param)
    {
        if (param.courseType == courseData.type)
        {
            // select the next level if there is one, otherwise keep the current one
            _upgradeLvl = param.intNr;
            if (_upgradeLvl < 3)
                ChangeSelectedLvl(GetUpgradeLevelArray() + 1);
            else
                ChangeSelectedLvl(GetUpgradeLevelArray());
        }
        SetAllUpgradeLvls();
    }
}
