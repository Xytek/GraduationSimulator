using System.Collections;
using UnityEngine;

public class Interactable : MonoBehaviour, ILookAtHandler
{
    public bool locked;
    public GameObject creditPrefab;

    [SerializeField] protected int _useTime;
    [SerializeField] protected int _creditAmount;
    [SerializeField] protected float _radius;

    [SerializeField] protected Shader _standardShader;
    [SerializeField] protected Shader _outlineShader;
    [SerializeField] protected Renderer _renderer;

    public void OnLookatEnter()
    {
        _renderer.material.shader = _outlineShader;
    }

    public void OnLookatExit()
    {
        _renderer.material.shader = _standardShader;
    }

    public void OnLookatInteraction(Vector3 lookAtPosition, Vector3 lookAtDirection)
    {
        // spawn credits in a circle around the laptop if it isn't locked
        if (!locked)
            StartCoroutine(UseObject());
        else
        {
            EventParams eventParams = new EventParams();
            eventParams.text = "You don't have permission to use this.";
            EventManager.TriggerEvent("LockedElement", eventParams);
        }
    }

    public void Unlock(EventParams param)
    {
        locked = false;
    }

    public void ChangeTime(EventParams param)
    {
        _useTime = param.intNr;
    }

    IEnumerator UseObject()
    {        
        // start animation

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
        Destroy(this.gameObject);
        EventManager.TriggerEvent("LookAtObjDestroyed", new EventParams());
    }
}
