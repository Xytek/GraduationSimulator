using UnityEngine;

public class LockedDoor : Door
{
    [SerializeField] private bool _locked;
    [SerializeField] private CourseTypes type;

    public void Awake()
    {
        switch (type)
        {
            case CourseTypes.Science:
                EventManager.StartListening("Science1Unlocked", Unlock);
                break;
            case CourseTypes.Research:
                EventManager.StartListening("Research1Unlocked", Unlock);
                break;
            case CourseTypes.Hacking:
                EventManager.StartListening("Hacking1Unlocked", Unlock);
                break;
            default:
                break;
        }
    }

    public void Unlock(EventParams e)
    {
        _locked = false;
    }

    public override void OpenDoor(string animatorBool)
    {
        // open if unlocked, else trigger locked-event
        if (!_locked)
            base.OpenDoor(animatorBool);
        else
        {
            EventParams eventParams = new EventParams();
            eventParams.text = "You don't have permission to enter this room.";
            EventManager.TriggerEvent("LockedElement", eventParams);
        }
    }

    public override void CloseDoor(string animatorBool)
    {
        if (!_locked)
            base.CloseDoor(animatorBool);
    }
}
