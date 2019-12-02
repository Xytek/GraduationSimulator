using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coffee : Collectable, ILookAtHandler
{
    private bool _used;
    private int _coffeeStrength = 10;

    protected Shader _standardShader;
    protected Shader _outlineShader;
    protected Renderer _renderer;

    public new void Start()
    {
        base.Start();
        _used = false;
        _standardShader = Shader.Find("Standard");
        _outlineShader = Shader.Find("Custom/Outline");
    }

    public void OnLookatEnter()
    {
        if (!_used)
        {
            foreach (Transform child in transform)
            {
                child.gameObject.GetComponent<Renderer>().material.shader = _outlineShader;
            }
        }
    }

    public void OnLookatExit()
    {
        if (!_used)
        {
            foreach (Transform child in transform)
            {
                child.gameObject.GetComponent<Renderer>().material.shader = _standardShader;
            }
        }
    }

    public void OnLookatInteraction(Vector3 lookAtPosition, Vector3 lookAtDirection)
    {
        // delete all parts of the coffee-mug
        _used = true;
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
