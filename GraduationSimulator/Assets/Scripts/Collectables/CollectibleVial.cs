using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectibleVial : MonoBehaviour, ILookAtHandler
{
    private Shader _standardShader;
    private Shader _outlineShader;
    private Player _player;
    private Renderer _renderer;

    private bool _unlocked = false;

    private void Awake()
    {
        _player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        if (_player == null)
            Debug.Log("Couldn't find any player");

        _renderer = GetComponent<Renderer>();
        if (_renderer == null)
            Debug.Log("Couldn't find a renderer");
        _standardShader = Shader.Find("Standard");
        _outlineShader = Shader.Find("Custom/Outline");

        EventManager.StartListening("FirstScienceCourseUnlocked", Unlock); // CHANGE the tier later
    }

    private void Unlock(EventParams e)
    {
        _unlocked = true;
    }

    public void OnLookatEnter()
    {
        if (_unlocked)
            _renderer.material.shader = _outlineShader;
    }

    public void OnLookatExit()
    {
        if (_unlocked)
            _renderer.material.shader = _standardShader;
    }

    public void OnLookatInteraction(Vector3 lookAtPosition, Vector3 lookAtDirection)
    {
        if (_unlocked)        
        {
            _player.IncreaseVials();
            this.gameObject.SetActive(false);
        }
    }


}