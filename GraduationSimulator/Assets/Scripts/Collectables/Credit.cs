using UnityEngine;
public class Credit : Collectable
{
    private void OnTriggerEnter(Collider other)
    {
        // play collect animation + sound
        if(other.gameObject.tag == "Player")
        {
            Destroy(this.gameObject);            
            _playerStats.UpdateCredits(1);
        }
   }
}
