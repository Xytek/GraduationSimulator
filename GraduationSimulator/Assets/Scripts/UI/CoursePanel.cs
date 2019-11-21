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
    public Image lockImage;

    public void Start()
    {
        int uLvl = courseData.UpgradeLevel;
        priceText.text = courseData.prices[uLvl].ToString();
        title.text = courseData.type.ToString();
        description.text = courseData.UpgradeDescriptions[uLvl];
    }

    public void ChangeLvl(int chosenLvl)
    {
        if(chosenLvl != courseData.UpgradeLevel)
        {
            DeactivateUpgrade();
        } else if(!upgradeButton.interactable)
        {
            ActivateUpgrade();
        }
        priceText.text = courseData.prices[chosenLvl].ToString();
        description.text = courseData.UpgradeDescriptions[chosenLvl];
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
}
