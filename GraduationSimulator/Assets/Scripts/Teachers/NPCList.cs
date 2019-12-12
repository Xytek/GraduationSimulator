using System.Collections.Generic;
using UnityEngine;

public class NPCList : MonoBehaviour
{
    public bool NewTeachers;
   [SerializeField] public List<Teacher> teachers = new List<Teacher>();    // Holds the list of all teachers 

    private void Awake()
    {
        GetActiveTeachers();
    }

    private void GetActiveTeachers()
    {
        // Finds the first grandchild (which is a teacher) and adds its Teacher script to the list
        foreach (Transform child in this.transform)
            if (child.gameObject.activeSelf)
                teachers.Add(child.GetChild(0).GetComponent<Teacher>());
    }

    public void InitializeMoreTeachers()
    {
        // Activate inactive teachers and add them to the teacher list
        foreach (Transform child in this.transform)
            if (child.gameObject.activeSelf == false)
            {
                child.gameObject.SetActive(true);
                teachers.Add(child.GetChild(0).GetComponent<Teacher>());
            }
        // So that functions using the list knows to update it
        NewTeachers = true;
    }
    // Freezes the NPC, used for pausing
    public void FreezeNPCs()
    {
        foreach (Teacher t in teachers)
            t.Freeze();
    }

    // Unfreezes/resumes
    public void ResumeNPCs()
    {
        foreach (Teacher t in teachers)
            t.Resume();
    }

    public void PowerUpNPCs()
    {
        foreach (Teacher t in teachers)
            t.PowerUp(1.33f, 1.33f, 1.33f);
    }
}
