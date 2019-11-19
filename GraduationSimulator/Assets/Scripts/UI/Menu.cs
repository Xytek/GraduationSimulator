using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Menu: MonoBehaviour
{
    protected Player player;

    public void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    }

    public abstract void Pause();
    public abstract void Resume();
}
