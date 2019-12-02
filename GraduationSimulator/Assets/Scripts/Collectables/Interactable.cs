using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour, ILookAtHandler
{
    public bool locked;
    public GameObject creditPrefab;
    protected bool _used;
    protected int _useTime;
    protected int _creditAmount;
    protected float _radius;
    protected Shader _standardShader;
    protected Shader _outlineShader;
    protected Renderer _renderer;

    public void Start()
    {
        _renderer = GetComponent<Renderer>();
        _standardShader = Shader.Find("Standard");
        _outlineShader = Shader.Find("Custom/Outline");
    }

    public void OnLookatEnter()
    {
        if (!_used)
        {
            _renderer.material.shader = _outlineShader;
        }
    }

    public void OnLookatExit()
    {
        if (!_used)
        {
            _renderer.material.shader = _standardShader;
        }
    }

    public void OnLookatInteraction(Vector3 lookAtPosition, Vector3 lookAtDirection)
    {
        // spawn credits in a circle around the laptop if it isn't locked or already used
        if (!locked && !_used)
        {
            StartCoroutine(SpawnCoins());

        }
        else if (_used)
        {
            return;
        }
        else
        {
            EventParams eventParams = new EventParams();
            eventParams.text = "You don't have permission to use this laptop.";
            EventManager.TriggerEvent("LockedElement", eventParams);
        }
    }


    public void Unlock(EventParams param)
    {
        locked = false;
    }

    public void ChangeTime(EventParams param)
    {
        _useTime = param.number;
    }

    IEnumerator SpawnCoins()
    {
        //yield on a new YieldInstruction that waits for 5 seconds.
        yield return new WaitForSeconds(_useTime);

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
            Quaternion rot = Quaternion.Euler(0, Random.Range(0, 360), 0);
            // create the credit
            Instantiate(creditPrefab, pos, rot);
        }
        _used = true;
        _renderer.material.shader = _standardShader;
    }

}
