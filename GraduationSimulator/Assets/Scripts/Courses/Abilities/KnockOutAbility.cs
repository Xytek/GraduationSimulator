using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Abilities/KnockOutAbility")]
public class KnockOutAbility : ScriptableObject
{
    public int coolDownTime;
    public GameObject arm;
    public CourseTypes type;

    public void Trigger(RaycastHit rayCastHit)
    {
        rayCastHit.transform.gameObject.GetComponent<Teacher>().GetDazed();

        EventParams param = new EventParams();
        param.courseType = type;
        EventManager.TriggerEvent("AbilityUsed", param);
    }

}
