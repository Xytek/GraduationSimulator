using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Door : MonoBehaviour
{    
    protected bool _fromOutside;
    protected bool _fromInside;
    
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

    public virtual void OpenDoor(string animatorBool)
    {        
            _animator.SetBool(animatorBool, true);              
    }

    public virtual void CloseDoor(string animatorBool)
    {       
            _animator.SetBool(animatorBool, false);     
    }
}
