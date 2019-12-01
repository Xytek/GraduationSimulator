using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorTrigger : MonoBehaviour
{
    [SerializeField] private bool _locked = false;
    private int _id;
    private static int _doorCounter = 0;

    public void Awake()
    {
        _doorCounter++;
        _id = _doorCounter;
        EventManager.StartListening("FirstScienceCourseUnlocked", Unlock);
    }

    public int GetId()
    {
        return _id;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!_locked)
        {
            EventParams eventParams = new EventParams();
            eventParams.number = _id;
            eventParams.color = Color.red;
            EventManager.TriggerEvent("DoorTriggerEnter", eventParams);
        }
        else
        {
            // Fire LockedDoorTriggered
            EventParams eventParams = new EventParams();
            eventParams.text = "You don't have permission to enter this room.";
            EventManager.TriggerEvent("LockedElement", eventParams);
        }
    }

    // Triggers Event on collision
    private void OnTriggerExit(Collider other)
    {
        EventParams eventParams = new EventParams();
        eventParams.number = _id;
        eventParams.color = Color.yellow;
        EventManager.TriggerEvent("DoorTriggerExit", eventParams);
    }

    public void Unlock(EventParams e)
    {
        _locked = false;
    }

    public void Unlock()
    {
        _locked = false;
    }

}
