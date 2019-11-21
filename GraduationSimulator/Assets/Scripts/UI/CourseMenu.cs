using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CourseMenu : Menu
{    
    public CoursePanel[] coursePanels;

    public new void Activate()
    {
        base.Activate();        
    }

    public void ActivateAffordableCourses(Player player)
    {
        foreach(CoursePanel panel in coursePanels)
        {
            if (panel.CheckIfAffordable(player))
            {                
                panel.ChangeLvl(1);
            }
            
        }
    }
}
