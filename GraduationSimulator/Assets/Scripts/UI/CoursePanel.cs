using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class CoursePanel : MonoBehaviour
{
    public CourseData courseData;
    public Player _player;

    [Header("UI Elements")]
    public Text title;    
    public Text description;
    public Text priceText;
    public UpgradeButton upgradeButton;
    
    private int _selectedLvl;
    public UpgradeSelectButton[] upgradeSelectButtons;

    public int GetCurrentUpdateLvl()
    {
        return courseData.UpgradeLevel;
    }

    public int GetSelectedLvl()
    {
        return _selectedLvl;
    }
   
    public void Awake()
    {       
        _selectedLvl = courseData.UpgradeLevel;
        priceText.text = courseData.prices[_selectedLvl].ToString();
        title.text = courseData.type.ToString();
        description.text = courseData.UpgradeDescriptions[_selectedLvl];
    }

    

    public bool CheckIfLvlIsAffordable(int level, int creditCount)
    {
        Debug.Log(level + "length: " + courseData.prices.Length);        
        return creditCount >= courseData.prices[level];
    }
    
    // sets upgradeLvl for all upgrades
    public void SetUpgradeLvls()
    {
        for (int i = 0; i < courseData.UpgradeLevel; i++)
        {
            upgradeSelectButtons[i].LvlAchieved();
        }
    }    

    private void SetSelected()
    {
        for(int i=0; i<upgradeSelectButtons.Length; i++)
        {
            if (i == _selectedLvl)
            {
                upgradeSelectButtons[i].LvlSelected();
            } else
            {
                upgradeSelectButtons[i].LvlUnselected();
            }
        }        
    }

    public void ChangeSelectedLvl(int chosenLvl)
    {
        if (chosenLvl == courseData.UpgradeLevel && CheckIfLvlIsAffordable(chosenLvl, _player.GetCreditCount()))
        {
            upgradeButton.Activate();
        }
        else
        {
            upgradeButton.Deactivate();
        }
        _selectedLvl = chosenLvl;
        priceText.text = courseData.prices[chosenLvl].ToString();
        description.text = courseData.UpgradeDescriptions[chosenLvl];
        SetSelected();
    }

    public void UpdatePanel()
    {
        ChangeSelectedLvl(courseData.UpgradeLevel);
        SetUpgradeLvls();
    }
}
