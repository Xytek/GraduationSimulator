using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnlockDoorAbility : Ability, IUnlockAbility
{
    private DoorTrigger _door;
    public UnlockDoorAbility(DoorTrigger door)
    {
        _door = door;
    }

    public void Use()
    {
        _door.Unlock();
    }

}
