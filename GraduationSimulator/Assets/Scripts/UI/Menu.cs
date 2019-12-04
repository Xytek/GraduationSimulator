using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menu : MonoBehaviour
{
    protected bool IsActive;
    
    public void Activate()
    {        
        IsActive = true;
        this.gameObject.SetActive(true);
    }
   
    public void Deactivate()
    {        
        IsActive = false;
        this.gameObject.SetActive(false);
    }

    public bool CheckIfActive()
    {
        return IsActive;
    }
}
