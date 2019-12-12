using System.Collections;
using UnityEngine;

public class TriggeredVial : MonoBehaviour
{
    [SerializeField] private GameObject _fire = default;        // Fire effect
    [SerializeField] private GameObject _explosion = default;   // Explosion effect
    [SerializeField] private float _detonationTime = 3f;        // Time from placed to explosion occurs
    private bool _detonated = false;                            // Has the vial detonated yet?
    public IEnumerator Start()
    {
        // These two are just to make sure they're not initially enabled
        _fire.SetActive(false);
        _explosion.SetActive(false);

        yield return new WaitForSeconds(_detonationTime);   // Once you plant a vial it takes a few seconds before it explodes
        _detonated = true;
        _fire.SetActive(true);
        _explosion.SetActive(true);
        yield return new WaitForSeconds(1.5f);
        _explosion.SetActive(false);        // Ensures the explosion only plays one time
        yield return new WaitForSeconds(10);
        Destroy(this.gameObject);           // Ensures the object doesn't stay forever
    }

    public void TurnOffFire()
    {
        _fire.SetActive(false);
        _explosion.SetActive(false);
    }

    // Once it detonates nearby teachers can hear it, and not just see it
    public bool HasDetonated()
    {
        return _detonated;
    }
}