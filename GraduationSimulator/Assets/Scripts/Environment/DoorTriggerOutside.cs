using UnityEngine;

public class DoorTriggerInside : MonoBehaviour
{
    public Door door;
    private void OnTriggerEnter(Collider other)
    {        
        if (!door.FromInside && other.gameObject.tag != "Teacher")
            door.OpenDoor("OpeningFromOutside");
    }

    // Triggers Event on collision
    private void OnTriggerExit(Collider other)
    {
        if (!door.FromInside && other.gameObject.tag != "Teacher")
            door.CloseDoor("OpeningFromOutside");
    }
}
