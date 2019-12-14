using UnityEngine;

public class DoorTriggerOutside : MonoBehaviour
{
    public Door door;

    // opens door if the player is not already inside the room
    private void OnTriggerEnter(Collider other)
    {
        if (!door.FromOutside && other.gameObject.tag == "Player")
            door.OpenDoor("OpeningFromInside");
    }

    // closes door if the player is not already inside the room
    private void OnTriggerExit(Collider other)
    {
        if (!door.FromOutside && other.gameObject.tag == "Player")
            door.CloseDoor("OpeningFromInside");
    }
}
