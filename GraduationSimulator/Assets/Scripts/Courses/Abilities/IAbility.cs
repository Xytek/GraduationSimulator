using UnityEngine;

public interface IAbility
{
    void Initialize();
    void Trigger(RaycastHit rayCastHit);
}
