using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectable : MonoBehaviour
{
    protected Player _player;
    public void Start()
    {
        _player = GameObject.FindWithTag("Player").GetComponent<Player>();
    }
}
