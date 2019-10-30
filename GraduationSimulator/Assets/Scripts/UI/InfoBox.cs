using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InfoBox : MonoBehaviour
{
    public Text txt;
    public GameObject blackboard;
    void Start()
    {
        EventManager.StartListening("LockedDoorTriggerEnter", LockedDoorEnter);
        EventManager.StartListening("LockedDoorTriggerExit", LockedDoorExit);
    }

    void LockedDoorEnter(EventParams e)
    {
        blackboard.SetActive(true);
        UpdateInfotext(e.text);
    }

    void LockedDoorExit(EventParams e)
    {
        blackboard.SetActive(false);
        UpdateInfotext(e.text);
    }

    private void UpdateInfotext(string text)
    {
        txt.text = text;     
    }
}
