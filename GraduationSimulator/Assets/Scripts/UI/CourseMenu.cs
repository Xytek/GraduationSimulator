using UnityEngine;
public class CourseMenu : Menu
{
    public CoursePanel[] coursePanels;

    // updates affordability and level-selection of all courses
    public void UpdateCoursePanels()
    {
        foreach (CoursePanel panel in coursePanels)
            panel.UpdatePanel();
    }
}
