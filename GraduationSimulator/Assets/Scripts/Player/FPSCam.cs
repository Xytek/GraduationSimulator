using UnityEngine;

public class FPSCam : MonoBehaviour
{
    [SerializeField] private float _sensitivity = 2f;   // Camera sensitivity
    private Vector2 _mouseLook;                         // 
    private GameObject _player;

    void Start()
    {
        _player = this.transform.parent.gameObject;
        if (_player == null) Debug.LogError("No player could be found");
    }

    // Update is called once per frame
    void Update()
    {
        float horizontal = Input.GetAxis("Mouse X");
        float vertical = Input.GetAxis("Mouse Y");
        Vector2 look = new Vector2(horizontal, vertical);       // Where are you looking
        _mouseLook += look * _sensitivity;                      // How fast should the camera move
        _mouseLook.y = Mathf.Clamp(_mouseLook.y, -80f, 80);     // Limit how far up/down you can look (no snapped necks)
        transform.localRotation = Quaternion.AngleAxis(-_mouseLook.y, Vector3.right);
        _player.transform.localRotation = Quaternion.AngleAxis(_mouseLook.x, _player.transform.up);
    }
}
