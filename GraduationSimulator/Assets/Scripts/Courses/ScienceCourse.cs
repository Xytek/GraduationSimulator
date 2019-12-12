public class ScienceCourse : Course
{
    public override void Upgrade()
    {
        base.SendUpgrade();
        switch (_upgradeLevel)
        {
            case 1:
                // open doors to science-rooms
                EventManager.TriggerEvent("Science1Unlocked", new EventParams());
                break;
            case 2:
                // activate vials
                EventManager.TriggerEvent("Science2Unlocked", new EventParams());
                break;
            case 3:
                // decrease cooldown-time for vials
                EventParams param = new EventParams();
                param.intNr = 5;
                EventManager.TriggerEvent("Science3Unlocked", param);
                break;
        }
    }
}
