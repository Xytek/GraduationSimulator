using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coffee : Collectable
{
    private int _coffeeStrength = 10;
    public void OnTriggerEnter(Collider other)
    {
        // delete all parts of the coffee-mug
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
