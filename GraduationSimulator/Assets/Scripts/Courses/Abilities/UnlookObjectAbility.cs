using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnlockInteractablesAbility : Ability, IUnlockAbility
{
    private Collectable _object;
    public UnlockInteractablesAbility(Collectable obj)
    {
        _object = obj;
    }

    public void Use()
    {                
        EventManager.TriggerEvent("BooksUnlocked", new EventParams());
    }

}
