using System.Collections.Generic;
using UnityEngine;

public class NPCList : MonoBehaviour
{
    public List<Teacher> teachers = new List<Teacher>();    // Holds the list of all teachers 

    private void Awake()
    {
        // Finds the first grandchild (which is a teacher) and adds its Teacher script to the list
        foreach (Transform child in this.transform)
            if (child.gameObject.activeSelf)
                teachers.Add(child.GetChild(0).GetComponent<Teacher>());
    }

    // Freezes the NPC, used for pausing
    public void FreezeNPCs()
    {
        foreach (Teacher p in teachers)
            p.Freeze();
    }

    // Unfreezes/resumes
    public void ResumeNPCs()
    {
        foreach (Teacher p in teachers)
            p.Resume();
    }
}
