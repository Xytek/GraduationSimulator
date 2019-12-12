using UnityEngine;

public class DoorTriggerOutside : MonoBehaviour
{
    public Door door;

    // opens door if the player is not already inside the room
    private void OnTriggerEnter(Collider other)
    {
        if (!door.FromOutside)
            door.OpenDoor("OpeningFromInside");
    }

    // closes door if the player is not already inside the room
    private void OnTriggerExit(Collider other)
    {
        if (!door.FromOutside)
            door.CloseDoor("OpeningFromInside");
    }
}
