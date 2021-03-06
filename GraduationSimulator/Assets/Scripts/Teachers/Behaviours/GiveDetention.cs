﻿using UnityEngine;
using UnityEngine.AI;

public class GiveDetention : StateMachineBehaviour
{
    private Transform _npc;
    private NavMeshAgent _agent;
    private Teacher _teacher;
    private Transform _target;
    private Player _player;
    private bool _caught;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        InitializeVariables(animator);

        animator.SetBool("isChasing", false);

        if (_target != null && _target.tag == "Player")
        {
            _player = _target.GetComponent<Player>();
            // Make the teacher stop and face the target until we exit the state
            _player.StopAndLookAt(_npc);
            StopAndFaceTarget();

            // Check on entry if the player has already been caught by another teacher
            _caught = _player.Caught;
            if (_caught == false)
                _player.Caught = true;
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _teacher.target = null;
        _agent.isStopped = false;

        if (_caught == false)
        {
            _player.Detention();
            _player.Caught = false;
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

    private void StopAndFaceTarget()
    {
        _agent.isStopped = true;
        Vector3 lookPos = _target.position - _npc.position;
        lookPos.y = 0;
        _npc.rotation = Quaternion.Slerp(_npc.rotation, Quaternion.LookRotation(lookPos), 1f);
    }
}
