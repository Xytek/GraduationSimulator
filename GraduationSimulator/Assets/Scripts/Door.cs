using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{    
    private DoorTrigger _doorCollider;
    private int _id;
    [SerializeField]
    private Animator _animator;

    // Start is called before the first frame update
    void Start()
    {
        // find matching DoorTrigger
        _doorCollider = GetComponentInChildren<DoorTrigger>();
        _id = _doorCollider.GetId();

        // subscribe to DoorTrigger-Events
        EventManager.StartListening("DoorTriggerEnter", OpenDoor);
        EventManager.StartListening("DoorTriggerExit", CloseDoor);
    }

    private void OpenDoor(EventParams e)
    {        
        if (e.id == _id)
        {
            _animator.SetBool("IsOpen", true);
        }
    }

    private void CloseDoor(EventParams e)
    {
        if (e.id == _id)
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
