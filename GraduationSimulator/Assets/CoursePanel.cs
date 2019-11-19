using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class CoursePanel : MonoBehaviour
{    
    [SerializeField]
    public int price;
    public Text priceText;
    public Button updateButton;
    public CourseFactory.CourseTypes type;    

    public void UpdatePriceText()
    {
        priceText.text = price.ToString();
    }

    public void CheckIfAffordable(Player player)
    {        
        if (player.GetCreditCount() >= price)
        {
            updateButton.interactable = true;
            updateButton.onClick.AddListener(() => player.ActivateCourse(type, price));
        } else
        {
            updateButton.interactable = false;            
        }
    }

    public int GetPrice()
    {
        return price;
    }   
}
