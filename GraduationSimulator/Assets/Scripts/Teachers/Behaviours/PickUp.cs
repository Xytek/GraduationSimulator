using UnityEngine;
using UnityEngine.AI;

public class PickUp : StateMachineBehaviour
{
    private Transform _npc;
    private NavMeshAgent _agent;
    private Teacher _teacher;
    private Transform _target;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        InitializeVariables(animator);

        // Make the teacher stop and face the target until we exit the state
        StopAndFaceTarget();
        animator.ResetTrigger("panic");
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // Destroy the target and start moving again
        _teacher.DestroyTarget();
        _agent.isStopped = false;
    }

    private void InitializeVariables(Animator animator)
    {
        if (_npc == null)
        {
            _npc = animator.gameObject.transform;
            _agent = _npc.GetComponent<NavMeshAgent>();
            _teacher = _npc.GetComponent<Teacher>();
            if (_agent == null) Debug.LogError("No agent found");
            if (_teacher == null) Debug.LogError("No teacher found");
        }
        if (_teacher != null) _target = _teacher.target;
    }

    private void StopAndFaceTarget()
    {
        _agent.isStopped = true;
        Vector3 lookPos = _target.position - _npc.position;
        lookPos.y = 0;
        _npc.rotation = Quaternion.Slerp(_npc.rotation, Quaternion.LookRotation(lookPos), 1f);
    }
}
