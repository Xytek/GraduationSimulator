using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Chase : StateMachineBehaviour
{
    private Transform _npc;
    private NavMeshAgent _agent;
    private Teacher _teacher;
    private Transform _target;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        InitializeVariables(animator);

        _agent.speed = _teacher.ChaseSpeed;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // As long as the teacher isn't paused (in a menu)
        if (_teacher.Pause)
            return;

        // Set destination to target
        if (_target != null)
        {
            _agent.SetDestination(_target.position);
            float distance = Vector3.Distance(_npc.position, _target.position);

            // If you reached your target then change state accordingly to the type of target
            if (distance < 2f)
                switch (_target.tag)
                {
                    case "Vial":
                        animator.SetTrigger("panic");
                        break;
                    case "Player":
                        animator.SetTrigger("giveDetention");
                        break;
                    case "Apple":
                        animator.SetTrigger("pickUp");
                        break;
                    default:
                        animator.SetBool("isChasing", false);
                        break;
                }
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
        if (_teacher != null) _target = _teacher.target;
    }
}
