public class HackingCourse : Course
{
    public override void Upgrade()
    {
        base.SendUpgrade();
        switch (_upgradeLevel)
        {
            case 1:
                EventManager.TriggerEvent("Hacking1Unlocked", new EventParams());
                break;
            case 2:
                EventManager.TriggerEvent("Hacking2Unlocked", new EventParams());
                break;
            case 3:
                // decrease usage-time of laptops
                EventParams param = new EventParams();
                param.intNr = 2;
                EventManager.TriggerEvent("Hacking3Unlocked", param);
                break;
        }
    }
}
