using UnityEngine;

public class Coffee : Collectable, ILookAtHandler
{
    [SerializeField] private int _coffeeStrength = default;
    [SerializeField] private Shader _standardShader = default;
    [SerializeField] private Shader _outlineShader = default;

    protected Player _player;
                    
    public new void Start()
    {
        base.Start();
        _player = GameObject.FindWithTag("Player").GetComponent<Player>();
        if (_player == null) Debug.LogError("No player found");
    }

    public void OnLookatEnter()
    {
        foreach (Transform child in transform)
            child.gameObject.GetComponent<Renderer>().material.shader = _outlineShader;
    }

    public void OnLookatExit()
    {
        foreach (Transform child in transform)
            child.gameObject.GetComponent<Renderer>().material.shader = _standardShader;
    }

    public void OnLookatInteraction(Vector3 lookAtPosition, Vector3 lookAtDirection)
    {        
        foreach (Transform child in transform)
            Destroy(child.gameObject);

        Destroy(this.gameObject);
        EventManager.TriggerEvent("LookAtObjDestroyed", new EventParams());
        _playerStats.UpdateEnergy(_coffeeStrength);
    }
}
