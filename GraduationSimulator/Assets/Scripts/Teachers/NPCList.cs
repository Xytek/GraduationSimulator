using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCList : MonoBehaviour
{
    public List<Patrol> patrols = new List<Patrol>();

    private void Awake()
    {
        // Finds the first grandchild (which is a teacher) and adds its Patrol script to the list
        foreach (Transform child in this.transform)
        {
            if (child.gameObject.activeSelf)
            {
            Transform grandChild = child.GetChild(0);
            patrols.Add(grandChild.GetComponent<Patrol>());
            }
        }
    }

    public void FreezeNPCs()
    {
        foreach (Patrol p in patrols)
            p.Freeze();
    }

    public void ResumeNPCs()
    {
        foreach (Patrol p in patrols)
            p.Resume();
    }
}
