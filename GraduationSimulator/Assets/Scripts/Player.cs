using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    [SerializeField]
    private float _speed = 5f;      // Player movement speed
    private int _credits = 0;
    private float _startEnergy = 100;
    private float _energy;
    private bool _isFrozen;
    private ILookAtHandler _lastLookAtObject = null;
    public float lookDistance = 10f;

    [Header("UI Elements")]
    public Image energyBar;
    public Text creditText;
    public GameObject noEnergyScreen;
    public SemesterTimer timer;

    private List<CourseFactory.CourseTypes> courses;

    private void Awake()
    {
        _energy = _startEnergy;
        creditText.text = _credits.ToString();
        courses = new List<CourseFactory.CourseTypes>();
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
                if (_lastLookAtObject == null)
                {
                    currentLookAtObject.OnLookatEnter();
                    _lastLookAtObject = currentLookAtObject;
                }
                // if it's not the first time and the player is looking at a different object
                else if (currentLookAtObject != _lastLookAtObject)
                {
                    _lastLookAtObject.OnLookatExit();
                    currentLookAtObject.OnLookatEnter();
                    _lastLookAtObject = currentLookAtObject;
                }
            }
            // if the player doesn't look at a valid object right now but has looked at one before
            else if (_lastLookAtObject != null)
            {
                _lastLookAtObject.OnLookatExit();
                _lastLookAtObject = null;
            }
        }
        else if (_lastLookAtObject != null)
        {
            _lastLookAtObject.OnLookatExit();
            _lastLookAtObject = null;
        }

        // call the interaction method if the user presses the left mouse button
        if (Input.GetMouseButtonDown(0) && _lastLookAtObject != null)
        {
            _lastLookAtObject.OnLookatInteraction(rayCastHit.point, rayDirection);
        }

        if (!_isFrozen)
        {
            DecreaseEnergy(1 * Time.deltaTime);
        }

        if (_energy <= 0)
        {
            Die();
        }

    }

    public void ResetLastLookAtObject()
    {
        _lastLookAtObject = null;
    }

    public int GetCreditCount()
    {
        return _credits;
    }

    public void IncreaseCreditCount()
    {
        _credits++;
        creditText.text = _credits.ToString();
    }
    public void DecreaseCreditCount()
    {
        if (_credits > 0)
        {
            _credits--;
            creditText.text = _credits.ToString();
        }
    }
    public void DecreaseCreditCount(int amount)
    {
        if (_credits - amount > 0)
        {
            _credits -= amount;
            creditText.text = _credits.ToString();
        }
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

    public void Freeze()
    {
        timer.Deactivate();
        _isFrozen = true;
        GetComponentInChildren<FPSCam>().enabled = false;
        Cursor.lockState = CursorLockMode.None;
    }

    public void Unfreeze()
    {
        timer.Activate();
        _isFrozen = false;
        GetComponentInChildren<FPSCam>().enabled = true;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void Die()
    {
        noEnergyScreen.SetActive(true);
    }

    public void ActivateCourse(CourseFactory.CourseTypes type, int price)
    {        
        bool alreadyExists = false;
        if (courses != null)
        {
            foreach (CourseFactory.CourseTypes course in courses)
            {
                if (course == type)
                {
                    alreadyExists = true;
                    return;
                }                
            }
        }
        if(!alreadyExists)
        {            
            courses.Add(type);
            CourseFactory.GetCourse(type).Activate();
            DecreaseCreditCount(price);
        }
    }
}