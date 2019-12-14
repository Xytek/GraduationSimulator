using UnityEngine;

public class Collectable : MonoBehaviour
{    
    protected PlayerStats _playerStats;
    public void Start()
    {        
        _playerStats = GameObject.FindWithTag("Player").GetComponent<PlayerStats>();     
        if (_playerStats == null) Debug.LogError("No player stats found");
    }
}
