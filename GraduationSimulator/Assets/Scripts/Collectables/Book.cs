using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Book : MonoBehaviour, ILookAtHandler
{
    [SerializeField] private GameObject _creditPrefab;
    private bool _locked;
    private bool _used;
    private int _creditAmount;
    private float _radius;

    private Shader _standardShader;
    private Shader _outlineShader;
    private Renderer _renderer;

    public Book()
    {
        _used = false;
        _creditAmount = 6;
        _radius = 1f;
    }
    void Awake()
    {
        _renderer = GetComponent<Renderer>();
        if (_renderer == null)
            Debug.LogError("Could not find renderer for book");
        _standardShader = Shader.Find("Standard");
        _outlineShader = Shader.Find("Custom/Outline");
    }


    public void OnLookatEnter()
    {
        if (!_locked)
        {
            _renderer.material.shader = _outlineShader;
        }        
    }

    public void OnLookatExit()
    {
        if (!_locked)
        {
            _renderer.material.shader = _standardShader;
        }        
    }

    public void OnLookatInteraction(Vector3 lookAtPosition, Vector3 lookAtDirection)
    {
        // spawn credits in a circle around the laptop if it isn't locked or already used
        if (!_locked && !_used)
        {
            Vector3 center = transform.position;
            for (int i = 0; i < _creditAmount; i++)
            {
                // calculate position in the circle
                float ang = 360 / _creditAmount * i;
                Vector3 pos;
                pos.x = center.x + _radius * Mathf.Sin(ang * Mathf.Deg2Rad);
                pos.y = center.y;
                pos.z = center.z + _radius * Mathf.Cos(ang * Mathf.Deg2Rad);

                // set the rotation of the credit (facing the laptop)
                //Quaternion rot = Quaternion.FromToRotation(Vector3.forward, center - pos);
                Quaternion rot = Quaternion.Euler(0, Random.Range(0, 360), 0);
                // create the credit
                Instantiate(_creditPrefab, pos, rot);
            }
            _used = true;
        }
        else if (_used)
        {
            return;
        }
        else
        {
            // change to more general locked trigger
            EventParams eventParams = new EventParams();
            eventParams.text = "You don't have permission to use this book.";
            EventManager.TriggerEvent("LockedElement", eventParams);
        }
    }
}
