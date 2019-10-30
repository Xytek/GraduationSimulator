using UnityEngine;
using System.Collections;

public class Credit : MonoBehaviour, ILookAtHandler
{
    private Player _player;

    public void Start()
    {
        _player = GameObject.FindWithTag("Player").GetComponent<Player>();
    }
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
        // play collect animation + sound
        Destroy(this.gameObject);        
        _player.ResetLastLookAtObject();
        _player.IncreaseCreditCount();        
    }
}
