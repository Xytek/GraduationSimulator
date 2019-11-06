using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coffee : Collectable, ILookAtHandler
{

    private int _coffeeStrength = 10;
    public void OnLookatEnter()
    {
        this.GetComponent<Renderer>().material.color = Color.yellow;
    }

    public void OnLookatExit()
    {
        this.GetComponent<Renderer>().material.color = Color.green;
    }

    public void OnLookatInteraction(Vector3 lookAtPosition, Vector3 lookAtDirection)
    {
        Destroy(this.gameObject);
        _player.ResetLastLookAtObject();
        _player.IncreaseEnergy(_coffeeStrength);
    }
}
