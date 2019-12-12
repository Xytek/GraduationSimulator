using UnityEngine;

[CreateAssetMenu(menuName = "Abilities/KnockOutAbility")]
public class KnockOutAbility : ScriptableObject
{
    public int coolDownTime;   // Cooldown of the skill
    public GameObject arm;     // Was thinking we could have different kinds of arms to knock out with, but not implemented
    public CourseTypes type;   // Which course it's a part of

    public void Trigger(RaycastHit rayCastHit)
    {
        rayCastHit.transform.gameObject.GetComponent<Teacher>().GetDazed();

        EventParams param = new EventParams();
        param.courseType = type;
        EventManager.TriggerEvent("AbilityUsed", param);
    }
}
