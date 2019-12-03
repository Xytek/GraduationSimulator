using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggeredApple : MonoBehaviour
{    
    public IEnumerator Start()
    {                      
        yield return new WaitForSeconds(30);
        Destroy(this.gameObject);           // Ensures the object doesn't stay forever
    }
}