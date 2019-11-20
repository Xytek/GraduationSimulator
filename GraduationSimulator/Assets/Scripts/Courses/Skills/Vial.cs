using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vial : MonoBehaviour, ILookAtHandler
{
    private Player _player;
    [SerializeField] private GameObject _fire;
    [SerializeField] private GameObject _explosion;
    [SerializeField] private float _detonationTime = 3f;
    [SerializeField] private Shader _standardShader;
    [SerializeField] private Shader _outlineShader;

    private bool _unlocked = false; 
    public bool used = false;
    private Renderer _renderer;

    private void Awake()
    {
        StartCoroutine(PlantVial());
        _player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        if (_player == null)
            Debug.Log("Couldn't find any player");

        _renderer = GetComponent<Renderer>();
        if (_renderer == null)
            Debug.Log("Couldn't find a renderer");
        EventManager.StartListening("FirstScienceCourseUnlocked", Unlock); // CHANGE the tier later
    }

    private void Unlock(EventParams e)
    {
        _unlocked = true;
    }

      public void OnLookatEnter()
    {
        if (_unlocked)
        {
            _renderer.material.shader = _outlineShader;
        }
    }

    public void OnLookatExit()
    {
        if (_unlocked)
        {
            _renderer.material.shader = _standardShader;
        }
    }

    public void OnLookatInteraction(Vector3 lookAtPosition, Vector3 lookAtDirection)
    {
        if (_unlocked && !used)         // Used is there so you can't pick up vials you've set ablaze
        {
            _player.IncreaseVials();
            this.gameObject.SetActive(false);
        }
    }

    public IEnumerator PlantVial()
    {
        // These two are just to make sure they're not initially enabled
        _fire.SetActive(false);
        _explosion.SetActive(false);
        yield return new WaitForEndOfFrame();
        if (used == true)                // This should only be called when the player spawns the vial, and is set in the player script
        {
            // Once you plant a vial it takes a few seconds before it explodes
            yield return new WaitForSeconds(_detonationTime);
            _fire.SetActive(true);
            _explosion.SetActive(true);
            yield return new WaitForSeconds(1.5f);
            _explosion.SetActive(false);        // Ensures the explosion only plays one time
            yield return new WaitForSeconds(30);
            Destroy(this.gameObject);           // Ensures the object doesn't stay forever
        }
    }

    public void DestroyVial()
    {
        Destroy(this.gameObject);          // So the teachers have a way to put it out
    }

}