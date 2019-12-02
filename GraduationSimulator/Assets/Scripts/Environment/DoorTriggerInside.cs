using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorTriggerOutside : MonoBehaviour
{
    public Door door;
    private void OnTriggerEnter(Collider other)
    {
        if (!door.FromOutside)
        {
            door.OpenDoor("OpeningFromInside");
        }
    }

    // Triggers Event on collision
    private void OnTriggerExit(Collider other)
    {
        if (!door.FromOutside)
        {
            door.CloseDoor("OpeningFromInside");
        }
    }
}
