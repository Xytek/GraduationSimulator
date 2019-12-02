using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Door : MonoBehaviour
{    
    private bool _fromOutside;
    private bool _fromInside;

    [SerializeField] protected bool _locked;
    [SerializeField] protected Animator _animator;

    public Door()
    {
        _fromOutside = false;
        _fromInside = false;
    }

    public bool FromOutside
    {   
        get { return _fromOutside; }
        set { _fromOutside = value; }
    }

    public bool FromInside
    {
        get { return _fromInside; }
        set { _fromInside = value; }
    }

    public void OpenDoor(string animatorBool)
    {
        if (!_locked)
        {
            _animator.SetBool(animatorBool, true);
        }
        else
        {
            // Fire LockedDoorTriggered
            EventParams eventParams = new EventParams();
            eventParams.text = "You don't have permission to enter this room.";
            EventManager.TriggerEvent("LockedElement", eventParams);
        }
    }

    public void CloseDoor(string animatorBool)
    {
        if (!_locked)
        {
            _animator.SetBool(animatorBool, false);
        }
    }
}
