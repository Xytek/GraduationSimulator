using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCList : MonoBehaviour
{
    public List<Teacher> teachers = new List<Teacher>();

    private void Awake()
    {
        // Finds the first grandchild (which is a teacher) and adds its Patrol script to the list
        foreach (Transform child in this.transform)
        {
            if (child.gameObject.activeSelf)
            {
            Transform grandChild = child.GetChild(0);
            teachers.Add(grandChild.GetComponent<Teacher>());
            }
        }
    }

    public void FreezeNPCs()
    {
        foreach (Teacher p in teachers)
            p.Freeze();
    }

    public void ResumeNPCs()
    {
        foreach (Teacher p in teachers)
            p.Resume();
    }
}
