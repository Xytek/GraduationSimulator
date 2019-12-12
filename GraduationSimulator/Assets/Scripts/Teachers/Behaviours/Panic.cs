using UnityEngine;
using UnityEngine.AI;

public class Panic : StateMachineBehaviour
{
    private Transform _npc;
    private NavMeshAgent _agent;
    private Teacher _teacher;
    private Transform _target = default;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        InitializeVariables(animator);

        // Make the teacher stop and face the target until we exit the state
        StopAndFaceTarget();

        animator.SetTrigger("pickUp");
        animator.SetBool("isChasing", false);

    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // Turn off the fire and have the agent pick it up
        if (_target != null)
        {
            TriggeredVial vialScript = _target.GetComponent<TriggeredVial>();
            vialScript.TurnOffFire();
        }
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
    }

    private void StopAndFaceTarget()
    {
        _agent.isStopped = true;
        if (_target != null)
        {
            Vector3 lookPos = _target.position - _npc.position;
            lookPos.y = 0;
            _npc.rotation = Quaternion.Slerp(_npc.rotation, Quaternion.LookRotation(lookPos), 1f);
        }
    }
}
