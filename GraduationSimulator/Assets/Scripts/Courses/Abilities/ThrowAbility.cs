using UnityEngine;

[CreateAssetMenu(menuName = "Abilities/ThrowAbility")]
public class ThrowAbility : ScriptableObject
{    
    public int coolDownTime;
    public GameObject throwObject;
    public CourseTypes type;

    public void Trigger(RaycastHit rayCastHit)
    {        
        Instantiate(throwObject, rayCastHit.point, Quaternion.identity);
        
        EventParams param = new EventParams();
        param.courseType = type;
        EventManager.TriggerEvent("AbilityUsed", param);     
    }
}
