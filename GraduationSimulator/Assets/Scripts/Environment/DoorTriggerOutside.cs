using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorTriggerInside : MonoBehaviour
{
    public Door door;
    private void OnTriggerEnter(Collider other)
    {
        if (!door.FromInside)
        {
            door.OpenDoor("OpeningFromOutside");
        }
    }

    // Triggers Event on collision
    private void OnTriggerExit(Collider other)
    {
        if (!door.FromInside)
        {
            door.CloseDoor("OpeningFromOutside");
        }
    }
}
