using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laptop : Collectable, ILookAtHandler
{
    public bool _locked;
    public GameObject creditPrefab;
    private bool used;   
    private int creditAmount;    
    private float radius;

    public Laptop()
    {
        used = false;
        creditAmount = 6;
        radius = 1f;
    }

    public void OnLookatEnter()
    {
        
    }

    public void OnLookatExit()
    {

    }

    public void OnLookatInteraction(Vector3 lookAtPosition, Vector3 lookAtDirection)
    {
        // spawn credits in a circle around the laptop if it isn't locked or already used
        if (!_locked && !used)
        {            
            Vector3 center = transform.position;
            for (int i = 0; i < creditAmount; i++)
            {
                // calculate position in the circle
                float ang = 360/creditAmount*i;
                Vector3 pos;
                pos.x = center.x + radius * Mathf.Sin(ang * Mathf.Deg2Rad);
                pos.y = center.y;
                pos.z = center.z + radius * Mathf.Cos(ang * Mathf.Deg2Rad);

                // set the rotation of the credit (facing the laptop)
                //Quaternion rot = Quaternion.FromToRotation(Vector3.forward, center - pos);
                Quaternion rot = Quaternion.Euler(0, Random.Range(0, 360), 0);
                // create the credit
                Instantiate(creditPrefab, pos, rot);
            }
            used = true;
        }
        else if (used)
        {
            return;
        }
        else
        {           
            // change to more general locked trigger
            EventParams eventParams = new EventParams();
            eventParams.text = "You don't have permission to use this laptop.";
            EventManager.TriggerEvent("LockedDoorTriggerEnter", eventParams);
        }
    }    
}
