using UnityEngine;
using System.Collections;

public class Credit : Collectable, ILookAtHandler
{    
    public void OnLookatEnter()
    {
        // create outline
    }

    public void OnLookatExit()
    {
        
    }

    public void OnLookatInteraction(Vector3 lookAtPosition, Vector3 lookAtDirection)
    {
        // play collect animation + sound
        Destroy(this.gameObject);
        _player.ResetLastLookAtObject();
        _player.IncreaseCreditCount();        
    }
}
