using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class CoursePanel : MonoBehaviour
{    
    public CourseData courseData;

    [Header("UI Elements")]
    public Text title;
    public Button[] upgradeStatusButtons;
    public Text description;    
    public Text priceText;
    public Button upgradeButton;
    public Sprite activeSprite;
    public Image lockImage;

    private int _selectedLvl;

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
        EventManager.StartListening("Upgrade", SetUpgradeLvlEvent);
        _selectedLvl = courseData.UpgradeLevel;
        priceText.text = courseData.prices[_selectedLvl].ToString();
        title.text = courseData.type.ToString();
        description.text = courseData.UpgradeDescriptions[_selectedLvl];        
    }  

    public void ActivateUpgrade()
    {
        lockImage.enabled = false;
        upgradeButton.interactable = true;
    }

    public void DeactivateUpgrade()
    {
        lockImage.enabled = true;
        upgradeButton.interactable = false;
    }    

    public bool CheckIfAffordable(Player player)
    {
        return player.GetCreditCount() >= courseData.prices[courseData.UpgradeLevel];        
    }

    public void SetUpgradeLvl()
    {
        for (int i = 0; i < courseData.UpgradeLevel; i++)
        {
            upgradeStatusButtons[i].GetComponent<Image>().sprite = activeSprite;
        }
    }
    public void SetUpgradeLvlEvent(EventParams param)
    {
        SetUpgradeLvl();
    }

    public void ChangeLvl(int chosenLvl)
    {
        if (chosenLvl != courseData.UpgradeLevel)
        {
            DeactivateUpgrade();
        }
        else if (!upgradeButton.interactable)
        {
            ActivateUpgrade();
        }
        priceText.text = courseData.prices[chosenLvl].ToString();
        description.text = courseData.UpgradeDescriptions[chosenLvl];
    }
}
