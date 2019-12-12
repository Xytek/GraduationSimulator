using UnityEngine;

public class Coffee : Collectable, ILookAtHandler
{
    [SerializeField] private int _coffeeStrength;
    [SerializeField] private Shader _standardShader;
    [SerializeField] private Shader _outlineShader;

    public new void Start()
    {
        base.Start();
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
        _player.ResetLastLookAtObject();
        _playerStats.UpdateEnergy(_coffeeStrength);
    }
}
