using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{    
    private DoorTrigger _doorTrigger;
    private int _id;
    private bool _locked;

    [SerializeField]
    private Animator _animator;

    // Start is called before the first frame update
    void Start()
    {
        // find matching DoorTrigger
        _doorTrigger = GetComponentInChildren<DoorTrigger>();
        if (_doorTrigger == null)
            Debug.LogError("Could not find door trigger");
        _id = _doorTrigger.GetId();

        // subscribe to DoorTrigger-Events
        EventManager.StartListening("DoorTriggerEnter", OpenDoor);
        EventManager.StartListening("DoorTriggerExit", CloseDoor);
    }

    private void OpenDoor(EventParams e)
    {        
        if (e.number == _id)
        {
            _animator.SetBool("IsOpen", true);
        }
    }

    private void CloseDoor(EventParams e)
    {
        if (e.number == _id)
        {            
            _animator.SetBool("IsOpen", false);
        }
    }

    private void OnDestroy()
    {
        EventManager.StopListening("DoorTriggerEnter", OpenDoor);
        EventManager.StopListening("DoorTriggerExit", CloseDoor);
    }
}
