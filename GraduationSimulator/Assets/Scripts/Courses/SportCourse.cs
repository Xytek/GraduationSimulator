public class SportCourse : Course
{
    public override void Upgrade()
    {
        base.SendUpgrade();
        EventParams param = new EventParams();
        switch (_upgradeLevel)
        {
            case 1:
                // energy-Factor
                param.floatNr = 0.5f;
                EventManager.TriggerEvent("Sport1Unlocked", param);
                break;
            case 2:
                // speed
                param.floatNr = 10f;
                EventManager.TriggerEvent("Sport2Unlocked", param);
                break;
            case 3:
                // stealth knock-out
                EventManager.TriggerEvent("Sport3Unlocked", new EventParams());
                break;
        }
    }
}
