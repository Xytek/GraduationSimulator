using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    [SerializeField]
    private float _speed = 5f;      // Player movement speed
    private int _credits = 0;
    private float _startEnergy = 100;
    private float _energy;

    [Header("UI Elements")]
    public Image energyBar;
    public Text creditText;
    public GameObject noEnergyScreen;

    public float lookDistance = 10f;
    [HideInInspector]
    public ILookAtHandler lastLookAtObject = null;

    private void Awake()
    {
        _energy = _startEnergy;
        creditText.text = _credits.ToString();
    }

    void Start()
    {
        // Locks the cursor inside the game
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        // first person movement
        float vertical = Input.GetAxis("Vertical");
        float horizontal = Input.GetAxis("Horizontal");
        Vector3 moveDirection = new Vector3(horizontal, 0f, vertical) * _speed * Time.deltaTime;
        transform.Translate(moveDirection);

        Vector3 rayOrigin = transform.position;
        Vector3 rayDirection = transform.forward;
        RaycastHit rayCastHit;

        // send raycast
        if (Physics.Raycast(rayOrigin, rayDirection, out rayCastHit, lookDistance))
        {
            // check if an object that carries an ILookAtHandler component has been hit
            ILookAtHandler currentLookAtObject = rayCastHit.collider.GetComponent<ILookAtHandler>();

            // if the player starts looking at a valid object, call its "start" mehtod
            // if the player stops looking at a valid object, call its "end" method
            if (currentLookAtObject != null)
            {
                // if this is the first time the player looks at a valid object
                if (lastLookAtObject == null)
                {
                    currentLookAtObject.OnLookatEnter();
                    lastLookAtObject = currentLookAtObject;
                }
                // if it's not the first time and the player is looking at a different object
                else if (currentLookAtObject != lastLookAtObject)
                {
                    lastLookAtObject.OnLookatExit();
                    currentLookAtObject.OnLookatEnter();
                    lastLookAtObject = currentLookAtObject;
                }
            }
            // if the player doesn't look at a valid object right now but has looked at one before
            else if (lastLookAtObject != null)
            {
                lastLookAtObject.OnLookatExit();
                lastLookAtObject = null;
            }
        }
        else if (lastLookAtObject != null)
        {
            lastLookAtObject.OnLookatExit();
            lastLookAtObject = null;
        }

        // call the interaction method if the user presses the left mouse button
        if (Input.GetMouseButtonDown(0) && lastLookAtObject != null)
        {
            lastLookAtObject.OnLookatInteraction(rayCastHit.point, rayDirection);
        }

        DecreaseEnergy(1 * Time.deltaTime);

        if(_energy <= 0)
        {
            Die();
        }

    }

    public void ResetLastLookAtObject()
    {
        lastLookAtObject = null;
    }

    public void IncreaseCreditCount()
    {
        _credits++;
        creditText.text = _credits.ToString();
        Debug.Log("You have " + _credits + " credits, congratulations!");
    }

    public void DecreaseEnergy(float decrease)
    {        
        _energy -= decrease;
        energyBar.fillAmount = _energy / _startEnergy;        
    }

    public void IncreaseEnergy(float increase)
    {
        _energy += increase;
        energyBar.fillAmount = _energy / _startEnergy;
    }

    public void Die()
    {
        noEnergyScreen.SetActive(true);
    }

}