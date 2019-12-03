using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menu : MonoBehaviour
{
    protected bool IsActive;
    private NPCList _npcList;

    public void Start()
    {
        if (_npcList == null)
            GetNPCList();
    }

    public void Activate()
    {
        if (_npcList == null)
            GetNPCList();
        _npcList.FreezeNPCs();
        IsActive = true;
        this.gameObject.SetActive(true);
    }

    private void GetNPCList()
    {
        _npcList = GameObject.FindGameObjectWithTag("NPCList").GetComponent<NPCList>();
        if (_npcList == null) Debug.LogError("Couldn't find the npc list");
    }
    public void Deactivate()
    {
        if (_npcList == null)
            GetNPCList();
        _npcList.ResumeNPCs();
        IsActive = false;
        this.gameObject.SetActive(false);
    }

    public bool CheckIfActive()
    {
        return IsActive;
    }
}
