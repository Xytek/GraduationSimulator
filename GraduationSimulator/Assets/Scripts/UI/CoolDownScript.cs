using UnityEngine.UI;
using UnityEngine;

public class CoolDownScript : MonoBehaviour
{
    [SerializeField] private float _coolDownTime;
    private bool _isCoolingDown;

    public Image coolDownImage;
    public CourseTypes type;
    public void Awake()
    {
        _isCoolingDown = false;
        coolDownImage.fillAmount = 0;
        EventManager.StartListening("AbilityUsed", StartCoolDown);
        switch (type)
        {
            case CourseTypes.Science:
                EventManager.StartListening("Science2Unlocked", Display);
                EventManager.StartListening("Science3Unlocked", ChangeCoolDownTime);
                break;
            case CourseTypes.Psychology:
                EventManager.StartListening("Psychology1Unlocked", Display);
                EventManager.StartListening("Psychology2Unlocked", ChangeCoolDownTime);
                break;
            case CourseTypes.Sports:
                EventManager.StartListening("Sport3Unlocked", Display);
                break;
            default:
                Debug.LogError("Change the course-type to one that has a cooldown-ability!");
                break;
        }
        if (type == CourseTypes.Science)
        {
            EventManager.StartListening("Science2Unlocked", Display);
            EventManager.StartListening("Science3Unlocked", ChangeCoolDownTime);
        }
        else if (type == CourseTypes.Psychology)
        {
            EventManager.StartListening("Psychology1Unlocked", Display);
            EventManager.StartListening("Psychology2Unlocked", ChangeCoolDownTime);
        }
        else if (type == CourseTypes.Sports)
        {
            EventManager.StartListening("Sport3Unlocked", Display);
        }
        this.gameObject.SetActive(false);
    }
    public void Update()
    {
        if (_isCoolingDown)
            coolDownImage.fillAmount -= 1 / _coolDownTime * Time.deltaTime;

        // stop coolDown and trigger event that it is over
        if (_isCoolingDown && coolDownImage.fillAmount == 0)
        {
            _isCoolingDown = false;
            EventParams param = new EventParams();
            param.courseType = type;
            EventManager.TriggerEvent("CoolDownOver", param);
        }
    }
    public void StartCoolDown(EventParams param)
    {
        if (param.courseType == type)
        {
            coolDownImage.fillAmount = 1;
            _isCoolingDown = true;
        }
    }
    public void Display(EventParams param)
    {
        this.gameObject.SetActive(true);
    }

    public void ChangeCoolDownTime(EventParams param)
    {
        _coolDownTime = param.intNr;
    }
}
