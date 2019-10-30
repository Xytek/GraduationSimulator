using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{    
    void Start()
    {        
        EventManager.StartListening("DoorTriggerEnter", ChangeEnemyColor);
        EventManager.StartListening("DoorTriggerExit", ChangeEnemyColor);
    }

    private void OnDestroy()
    {
        EventManager.StopListening("DoorTriggerEnter", ChangeEnemyColor);
        EventManager.StopListening("DoorTriggerExit", ChangeEnemyColor);
    }

    private void ChangeEnemyColor(EventParams e)
    {
        transform.GetComponent<Renderer>().material.color = e.color;
    }   
}
