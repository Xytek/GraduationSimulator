using UnityEngine;
public class Credit : Collectable
{
    private void OnTriggerEnter(Collider other)
    {
        // play collect animation + sound
        Destroy(this.gameObject);
        _player.ResetLastLookAtObject();
        _playerStats.UpdateCredits();
    }
}
