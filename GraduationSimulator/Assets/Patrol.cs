using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Patrol : StateMachineBehaviour
{
    List<Transform> _checkpoints = new List<Transform>();    // An array holding the checkpoints the teacher will go to
    private int _nextCheckpoint;                            // Holds the next checkpoint the teacher should go to
    private Transform _npc;
    private NavMeshAgent _agent;
    private Teacher _teacher;
    private float _timeElapsed;         // Used to counter a bug where the teacher might update too quickly after an action
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        InitializeVariables(animator);

        _agent.speed = _teacher.PatrolSpeed;

        _timeElapsed = Time.time;
    }


    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // Normal patrolling
        if (!_agent.pathPending && _agent.remainingDistance < 0.5f)
            GoToNextCheckpoint();
        // If the teacher gains a target, start chasing
        if (_teacher.target != null && _timeElapsed < (Time.time - 2f))
            animator.SetBool("isChasing", true);
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetBool("isPatrolling", false);
        _teacher.previousState = "isPatrolling";
    }

    #region Other functions
    private void GoToNextCheckpoint()
    {
        // Set the agent destination to the next checkpoint in the array
        _agent.destination = _checkpoints[_nextCheckpoint].position;

        // After the last checkpoint in the array comes the first one, so make sure they're close together.
        _nextCheckpoint = (_nextCheckpoint + 1) % _checkpoints.Count;
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

            _checkpoints.Clear();   // Ensure checkpoints is empty before running
            // Get the first sibling (Checkpoints) and add all its children to the _checkpoints list.
            foreach (Transform child in _npc.parent.GetChild(_npc.GetSiblingIndex() + 1))
                _checkpoints.Add(child.transform);
        }
    }
    #endregion


}
