using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCList : MonoBehaviour
{
    [SerializeField] private List<Patrol> patrols;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
            FreezeNPCs();
        if (Input.GetKeyDown(KeyCode.R))
            ResumeNPCs();

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
