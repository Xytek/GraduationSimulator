using UnityEngine;

public class Door : MonoBehaviour
{
    protected bool _fromOutside; // is opened from the outside
    protected bool _fromInside; // is opened from the inside

    [SerializeField] protected Animator _animator;

    public Door()
    {
        _fromOutside = false;
        _fromInside = false;
    }

    public bool FromOutside
    {
        get { return _fromOutside; }
        set { _fromOutside = value; }
    }

    public bool FromInside
    {
        get { return _fromInside; }
        set { _fromInside = value; }
    }

    public virtual void OpenDoor(string animatorBool)
    {
        _animator.SetBool(animatorBool, true);
    }

    public virtual void CloseDoor(string animatorBool)
    {
        _animator.SetBool(animatorBool, false);
    }
}
