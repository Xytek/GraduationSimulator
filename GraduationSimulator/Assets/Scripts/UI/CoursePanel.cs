﻿using System.Collections;
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

    private int _upgradeLvl;    

    public void Awake()
    {
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
        {
            lvl = 0;
        }
        return lvl;
    }

    public bool CheckIfLvlIsAffordable(int level, int creditCount)
    {
        return creditCount >= courseData.prices[level];
    }

    // sets upgradeLvl for all upgradedLvls
    public void SetAllUpgradeLvls()
    {
        for (int i = 0; i < _upgradeLvl; i++)
        {
            upgradeSelectButtons[i].LvlAchieved();
        }
    }

    private void SetLvlSelection()
    {
        for (int i = 0; i < upgradeSelectButtons.Length; i++)
        {
            if (i == _selectedLvl)
            {
                upgradeSelectButtons[i].LvlSelected();
            }
            else
            {
                upgradeSelectButtons[i].LvlUnselected();
            }
        }
    }

    public void ChangeSelectedLvl(int chosenLvl)
    {
        if(chosenLvl == 0 && CheckIfLvlIsAffordable(chosenLvl, _player.GetCreditCount()) && GetUpgradeLevelArray() < 1)
        {
            upgradeButton.Activate();
        }
        else if (chosenLvl == GetUpgradeLevelArray()+1 && CheckIfLvlIsAffordable(chosenLvl, _player.GetCreditCount()))
        {
            upgradeButton.Activate();
        }
        //else if (chosenLvl < GetArrayLvl())
        //{
        //    upgradeButton.Used();
        //}
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

    public void UpgradePanel(EventParams param)
    {        
        if (param.courseType == courseData.type)
        {
            _upgradeLvl = param.number;
            if (_upgradeLvl < 3)
            {
                ChangeSelectedLvl(GetUpgradeLevelArray() + 1);
            } else
            {
                ChangeSelectedLvl(GetUpgradeLevelArray());
            }                        
        }
        SetAllUpgradeLvls();
    }
}
