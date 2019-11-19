using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coffee : Collectable, ILookAtHandler
{

    private int _coffeeStrength = 10;
    public void OnLookatEnter()
    {              
    }

    public void OnLookatExit()
    {        
    }

    public void OnLookatInteraction(Vector3 lookAtPosition, Vector3 lookAtDirection)
    {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
        Destroy(this.gameObject);
        _player.ResetLastLookAtObject();
        _player.IncreaseEnergy(_coffeeStrength);
    }

    public void ChangeCoffeeStrength(int strength)
    {
        _coffeeStrength = strength;
    }
}
