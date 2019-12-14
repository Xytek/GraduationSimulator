using UnityEngine;
using UnityEngine.UI;
using System.Collections;
public class Player : MonoBehaviour
{
    private ILookAtHandler _lastLookAtObject = null;    // For interactable objects
    private FPSCam _fpsCam;                             // The camera that's a child of player
    private PlayerStats _playerStats;                   // Speed/Energy/Credits and UI work
    private NPCList _npcList;
    private RaycastHit rayCastHit;                      // 

    private float _energyFactor = 1f;                   // The speed of which our energy drains
    private float _interactDistance = 3f;               // Distance from player to what he can interact with/place
    private bool _isFrozen;                             // For pausing, either in menus or when caught

    [Header("Abilities")]
    [SerializeField] private ThrowAbility throwVialAbility = default;    // Scriptable object holding information about the apple ability
    [SerializeField] private ThrowAbility throwAppleAbility = default;   // Scriptable object holding information about the vial ability
    [SerializeField] private KnockOutAbility knockOutAbility = default;  // Scriptable object holding information about the knockout ability
    private bool _throwVialAvailable;
    private bool _throwAppleAvailable;
    private bool _knockoutAvailable;

    [Header("UI Elements")]
    [SerializeField] private SemesterTimer timer = default;
    [SerializeField] private Image reticle = default;

    private void Awake()
    {
        StartListening();   // Start event listeners for ability unlocks

        // Get needed components
        _fpsCam = GetComponentInChildren<FPSCam>();
        _playerStats = GetComponent<PlayerStats>();
        _npcList = GameObject.FindGameObjectWithTag("NPCList").GetComponent<NPCList>();

        // Check that you can find them
        if (_fpsCam == null) Debug.LogError("Couldn't find the fps cam");
        if (_playerStats == null) Debug.LogError("No player stats found");
        if (_npcList == null) Debug.LogError("Couldn't find the npc list");        
    }

    private void Update()
    {
        // If the game is paused then just exit this function
        if (_isFrozen)
            return;

        // First person movement
        float vertical = Input.GetAxis("Vertical");
        float horizontal = Input.GetAxis("Horizontal");
        Vector3 moveDirection = new Vector3(horizontal, 0f, vertical) * _playerStats.Speed * Time.deltaTime;
        transform.Translate(moveDirection);

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out rayCastHit, _interactDistance))
        {
            // Check if you're looking at any interactable objects
            LookAtObject();
            // Check if you're attempting to use any skills           
            UseSkill();
        }

        // call the interaction method if the user presses the left mouse button
        if (Input.GetMouseButtonDown(0) && _lastLookAtObject != null)
            _lastLookAtObject.OnLookatInteraction(rayCastHit.point, transform.forward);

        // drain energy if the user is not frozen
        if (!_isFrozen)
            _playerStats.UpdateEnergy(-_energyFactor * Time.deltaTime);

        // end the level if energy is too low
        if (_playerStats.Energy <= 0)
        {
            RegainEnergy();
        }

        // Check if you've met the criteria for a new semester
        float timeLeft = timer.CurrentTime;
        if (timeLeft <= 0f || _playerStats.NewSem)
        {
            NewSemester(timer.StartTime - timeLeft);
        }

        // Press C to get 100 credits. Just helps when testing
        Cheat();
    }

    private void Cheat()
    {
        if (Input.GetKeyDown(KeyCode.C))
            _playerStats.UpdateCredits(-100);
    }

    private void UseSkill()
    {
        GameObject rayO = rayCastHit.transform.gameObject;      // What you're looking at

        if ((rayO.tag == "Floor" && (_throwAppleAvailable || _throwVialAvailable)) || (rayO.tag == "Teacher" && _knockoutAvailable))
            reticle.color = Color.green;
        else
            reticle.color = Color.red;

        switch (Input.inputString)                              // Checks any input from the player
        {
            case "1":
                if (rayO.tag == "Floor" && _throwAppleAvailable)
                {
                    throwAppleAbility.Trigger(rayCastHit);
                    _throwAppleAvailable = false;
                }
                break;
            case "2":
                if (rayO.tag == "Floor" && _throwVialAvailable)
                {
                    throwVialAbility.Trigger(rayCastHit);
                    _throwVialAvailable = false;
                }
                break;
            case "3":
                if (rayO.tag == "Teacher" && _knockoutAvailable)
                {
                    knockOutAbility.Trigger(rayCastHit);
                    _knockoutAvailable = false;
                }
                break;
            default:
                break;
        }
    }
    private void LookAtObject()
    {
        // check if an object that carries an ILookAtHandler component has been hit
        ILookAtHandler currentLookAtObject = rayCastHit.collider.GetComponent<ILookAtHandler>();

        // if the player starts looking at a valid object, call its "start" method
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

    private void StartListening()
    {
        EventManager.StartListening("Science2Unlocked", UnlockVial);
        EventManager.StartListening("Psychology1Unlocked", UnlockApple);
        EventManager.StartListening("Sport1Unlocked", SetEnergyFactor);
        EventManager.StartListening("Sport2Unlocked", SetSpeed);
        EventManager.StartListening("Sport3Unlocked", UnlockKnockOut);
        EventManager.StartListening("CoolDownOver", EnableAbility);
        EventManager.StartListening("LookAtObjDestroyed", ResetLastLookAtObject);
    }

    #region Event listeners
    private void SetSpeed(EventParams param)
    {
        _playerStats.Speed = param.floatNr;
    }

    private void SetEnergyFactor(EventParams param)
    {
        _energyFactor = param.floatNr;
    }

    private void UnlockVial(EventParams param)
    {
        _throwVialAvailable = true;
    }
    private void UnlockApple(EventParams param)
    {
        _throwAppleAvailable = true;
    }
    private void UnlockKnockOut(EventParams param)
    {
        _knockoutAvailable = true;
    }
    private void EnableAbility(EventParams param)
    {
        switch (param.courseType)
        {
            case CourseTypes.Science:
                UnlockVial(param);
                break;
            case CourseTypes.Psychology:
                UnlockApple(param);
                break;
            case CourseTypes.Sports:
                UnlockKnockOut(param);
                break;
            default:
                Debug.Log("No course found");
                break;
        }
    }
    #endregion

    #region Public functions
    public void ResetLastLookAtObject(EventParams param)
    {
        _lastLookAtObject = null;
    }

    public void Freeze()
    {
        timer.Deactivate();
        _isFrozen = true;
        if (_fpsCam == null)
            _fpsCam = GetComponentInChildren<FPSCam>();
        _fpsCam.enabled = false;
        Cursor.lockState = CursorLockMode.None;
    }

    public void Unfreeze()
    {
        timer.Activate();
        _isFrozen = false;
        _fpsCam.enabled = true;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void StopAndLookAt(Transform target)
    {
        _isFrozen = true;
        transform.LookAt(target);
        transform.Rotate(-20, 0, 0);
        _fpsCam.enabled = false;
    }

    public void Detention()
    {
        this.gameObject.transform.position = new Vector3(-6.5f, 1f, 4f);
        this.gameObject.transform.rotation = new Quaternion(0f, 0f, 0f, 0f);
        _fpsCam.enabled = true;
        _isFrozen = false;

        // reduce time
        timer.CurrentTime = timer.CurrentTime - 30;

        // trigger detention event for UI
        EventParams param = new EventParams();
        param.text = "You have been caught by your teacher, stay in detention for a while.";
        EventManager.TriggerEvent("ShowInstructions", param);
    }

    public void RegainEnergy()
    {
        // trigger information event for UI
        EventParams param = new EventParams();
        param.text = "Seems like university has drained all your energy. You spent the entire day in the cafeteria to regain energy. That costed you thirty seconds of time.";
        EventManager.TriggerEvent("ShowInstructions", param);

        // teleport to canteen
        this.gameObject.transform.position = new Vector3(37f, 1f, -10.5f);
        this.gameObject.transform.rotation = new Quaternion(0f, 0f, 0f, 0f);

        // reduce time
        timer.CurrentTime = timer.CurrentTime - 30;

        // refill energy
        _playerStats.ResetEnergy();
    }

    private void NewSemester(float timeSpent)
    {
        this.gameObject.transform.position = new Vector3(-6.5f, 1f, 4f);
        this.gameObject.transform.rotation = new Quaternion(0f, 0f, 0f, 0f);
        _playerStats.TotalTime += timeSpent;
        _fpsCam.enabled = true;
        _isFrozen = false;
        _playerStats.ResetEnergy();
        timer.NewSemester();
        _npcList.InitializeMoreTeachers();
        _npcList.PowerUpNPCs();

        // trigger semester event for UI
        EventParams param = new EventParams();
        param.text = "You have finished the level. Keep working hard in the next semester! Your time: " + timeSpent.ToString();
        EventManager.TriggerEvent("SemesterOver", param);
    }
    #endregion
}