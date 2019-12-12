using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Daze : StateMachineBehaviour
{
    private Transform _npc;
    private NavMeshAgent _agent;
    private FieldOfView _fow;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        InitializeVariables(animator);

        _agent.isStopped = true;
        _fow.enabled = false;

    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _agent.isStopped = false;
        _fow.enabled = true;
    }

    private void InitializeVariables(Animator animator)
    {
        if (_npc == null)
        {
            _npc = animator.gameObject.transform;
            _agent = _npc.GetComponent<NavMeshAgent>();
            _fow = _npc.GetComponent<FieldOfView>();
            if (_agent == null) Debug.LogError("No agent found");
            if (_fow == null) Debug.LogError("No fow found");
        }
    }
}
