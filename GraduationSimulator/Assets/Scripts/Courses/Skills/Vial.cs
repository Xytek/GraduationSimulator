using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vial : MonoBehaviour
{
    [SerializeField] private GameObject _fire;
    [SerializeField] private GameObject _explosion;
    [SerializeField] private float _detonationTime = 3f;
    // Start is called before the first frame update
    private IEnumerator Start()
    {
        // These two are just to make sure they're not initially enabled
        _fire.SetActive(false);
        _explosion.SetActive(false);
        // Once you plant a vial it takes a few seconds before it explodes
        yield return new WaitForSeconds(_detonationTime);
        _fire.SetActive(true);
        _explosion.SetActive(true);
        yield return new WaitForSeconds(1.5f);
        _explosion.SetActive(false);        // Ensures the explosion only plays one time
        yield return new WaitForSeconds(30);
        Destroy(this.gameObject);           // Ensures the object doesn't stay forever
    }

    public void DestroyVial()
    {
        Destroy(this.gameObject);          // So the teachers have a way to put it out
    }
}