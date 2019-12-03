using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    [SerializeField] private float _speed = 5f;     // Player movement speed

    [SerializeField] private int _credits = 0;

    private float _startEnergy = 100;
    private float _energy;
    private float _energyFactor = 1;

    private bool _isFrozen;


    private ILookAtHandler _lastLookAtObject = null;
    public float lookDistance = 10f;
    public RaycastHit rayCastHit;

    public ThrowAbility throwVialAbility;
    public ThrowAbility throwAppleAbility;

    private bool _throwVialAvailable = false;
    private bool _throwAppleAvailable = false;

    [Header("UI Elements")]
    public Image energyBar;
    public Text creditText;
    public GameObject noEnergyScreen;
    public SemesterTimer timer;

    private void Awake()
    {
        _energy = _startEnergy;
        creditText.text = _credits.ToString();
        EventManager.StartListening("Science2Unlocked", UnlockVial);
        EventManager.StartListening("Psychology1Unlocked", UnlockApple);
        EventManager.StartListening("Sport1Unlocked", SetEnergyFactor);
        EventManager.StartListening("Sport2Unlocked", SetSpeed);
        EventManager.StartListening("CoolDownOver", EnableAbility);
    }

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;   // Locks the cursor inside the game
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
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
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
            else if (_lastLookAtObject != null)
            {
                _lastLookAtObject.OnLookatExit();
                _lastLookAtObject = null;
            }
        }
        // Logic for placing vials            
        else if (Physics.Raycast(ray, out rayCastHit, lookDistance))
        {
            if (_throwAppleAvailable &&
                Input.GetKeyDown(KeyCode.Alpha1)                // You press 1            
                && rayCastHit.transform.gameObject.tag == "Floor") // You're looking at the floor                                                               
            {
                throwAppleAbility.Trigger(rayCastHit);
                _throwAppleAvailable = false;
            }
            else if (_throwVialAvailable &&           // Skill is unlocked and not currently cooling-Down
                    Input.GetKeyDown(KeyCode.Alpha2)                        // You press 1            
                    && rayCastHit.transform.gameObject.tag == "Floor")     // You're looking at the floor            
            {
                throwVialAbility.Trigger(rayCastHit);
                _throwVialAvailable = false;
            }
        }

        // if the player doesn't look at a valid object right now but has looked at one before
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

        // drain energy if the user is not frozen
        if (!_isFrozen)
        {
            DecreaseEnergy(_energyFactor * Time.deltaTime);
        }

        // end the level if energy is too low
        if (_energy <= 0)
        {
            Die();
        }
    }

    public void ResetLastLookAtObject()
    {
        _lastLookAtObject = null;
    }

    public void SetSpeed(EventParams param)
    {
        _speed = param.floatNr;
    }

    public void SetEnergyFactor(EventParams param)
    {
        _energyFactor = param.floatNr;
    }

    public void UnlockVial(EventParams param)
    {
        _throwVialAvailable = true;
    }
    public void UnlockApple(EventParams param)
    {
        _throwAppleAvailable = true;
    }

    public void EnableAbility(EventParams param)
    {
        if (param.courseType == CourseTypes.Science)
        {
            UnlockVial(param);
        }
        else if (param.courseType == CourseTypes.Psychology)
        {
            UnlockApple(param);
        }
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

    public void Pay(int amount)
    {
        DecreaseCreditCount(amount);
    }
}