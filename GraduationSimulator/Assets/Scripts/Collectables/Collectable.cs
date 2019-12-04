using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectable : MonoBehaviour
{
    protected Player _player;
    protected PlayerStats _playerStats;
    public void Start()
    {
        _player = GameObject.FindWithTag("Player").GetComponent<Player>();
        if (_player == null) Debug.LogError("No player found");
        _playerStats = GameObject.FindWithTag("Player").GetComponent<PlayerStats>();
        if (_playerStats == null) Debug.LogError("No player stats found");
    }
}
