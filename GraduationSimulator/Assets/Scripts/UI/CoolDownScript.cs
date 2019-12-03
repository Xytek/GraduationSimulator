using System.Collections;
using System.Collections.Generic;
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
        if (type == CourseTypes.Science)
        {            
            EventManager.StartListening("Science2Unlocked", Display);
        } else if(type == CourseTypes.Psychology)
        {
            EventManager.StartListening("Psychology1Unlocked", Display);
        }
        this.gameObject.SetActive(false);
    }

    public void Update()
    {
        if (_isCoolingDown)
        {
            coolDownImage.fillAmount -= 1 / _coolDownTime * Time.deltaTime;            
        }

        // stop coolDown and trigger event that it is over
        if(_isCoolingDown && coolDownImage.fillAmount == 0)
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
        Debug.Log("activated");
        this.gameObject.SetActive(true);
    }
}
