using UnityEngine;
using System.Collections;

public class Credit : Collectable
{
    public void OnTriggerEnter(Collider other)
    {
        // play collect animation + sound
        Destroy(this.gameObject);
        _player.ResetLastLookAtObject();
        _player.IncreaseCreditCount();
    }
}
