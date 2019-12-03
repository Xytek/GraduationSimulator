using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAbility
{
    void Initialize();
    void Trigger(RaycastHit rayCastHit);
}
