using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Patrol : MonoBehaviour
{
    [SerializeField] Transform[] _checkpoints = default;    // An array holding the checkpoints the teacher will go to
    private NavMeshAgent _agent;                            // Used for AI commands and initiated in Start()
    private int _nextCheckpoint;                            // Holds the next checkpoint the teacher should go to
    private Animator _anim;
    private float _chasingSpeed = 5f;
    private float _baseSpeed = 1f;
    private void Awake()
    {
        _anim = GetComponent<Animator>();
        _agent = GetComponent<NavMeshAgent>();

        // Check that you found all required components
        if (_anim == null) Debug.LogError("Couldn't find animator");
        if (_agent == null) Debug.LogError("Couldn't find agent");
    }

    private void Update()
    {
        // Ensures you only change destination when you're not already on a path, and you're close to your goal
        if (!_agent.pathPending && _agent.remainingDistance < 0.5f)
            GoToNextCheckpoint();

        // Changes the teachers speed based on their state
        if (_anim.GetBool("isChasing") && _agent.speed != _chasingSpeed)
            _agent.speed = _chasingSpeed;
        if (_anim.GetBool("isPatrolling") && _agent.speed != _baseSpeed)
            _agent.speed = _baseSpeed;
    }

    private void GoToNextCheckpoint()
    {
        // If no checkpoints have been added to the array it will exit the function
        if (_checkpoints.Length == 0 || isPanicking())
        {
            Debug.LogError("No checkpoints");
            return;
        }

        StatePatrol();

        // Set the agent destination to the next checkpoint in the array
        _agent.destination = _checkpoints[_nextCheckpoint].position;

        // After the last checkpoint in the array comes the first one, so make sure they're close together.
        _nextCheckpoint = (_nextCheckpoint + 1) % _checkpoints.Length;
    }

    public void ChaseVial(Transform vial)
    {
        if (isPanicking())
            return;

        StateChase();
        _agent.SetDestination(vial.position);
        float distance = Vector3.Distance(this.gameObject.transform.position, vial.position);
        if (distance < 1f)
           StartCoroutine(WaitAtVial(vial));
    }



    private IEnumerator WaitAtVial(Transform vial)
    {
        StatePanick();
        _agent.isStopped = true;
        FaceTarget(vial.position);
        yield return new WaitForSeconds(5f);
        if (vial != null)
            vial.GetComponent<TriggeredVial>().DestroyVial();
        _agent.isStopped = false;
        StatePatrol();
    }

    private void FaceTarget(Vector3 destination)
    {
        Vector3 lookPos = destination - transform.position;
        lookPos.y = 0;
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(lookPos), 1f);
    }

    public void ChaseTarget(List<Transform> visibleTargets)
    {
        if (isPanicking())
            return;

        StateChase();

        Transform target;
        // If there's more than one target, find the highest priority one
        if (visibleTargets.Count > 1)
            target = PrioritizeTarget(visibleTargets);
        else
            target = visibleTargets[0];

        if(target.tag != "Vial")
           _agent.SetDestination(target.position);

        // Game over comes here
        if (_agent.remainingDistance < 2f)
        {
            Debug.Log("You got caught");
        }
    }

    private Transform PrioritizeTarget(List<Transform> visibleTargets)
    {
        Transform priorityTarget = visibleTargets[0];
        int priorityValue = 10;

        // This code goes through each target by tag and checks whether a higher priority target is already selected, and if not sets the current one
        for (int i = 0; i<visibleTargets.Count; i++)
        {
            switch (visibleTargets[i].tag)
            {
                case "Vial":
                    priorityTarget = visibleTargets[i];
                    priorityValue = 0;
                    break;
                case "Player":
                    priorityTarget = (priorityValue > 1) ? visibleTargets[i] : priorityTarget;
                    priorityValue = 1;
                    break;
                case "Apple":
                    priorityTarget = (priorityValue > 2) ? visibleTargets[i] : priorityTarget;
                    priorityValue = 2;
                    break;
                default:
                    Debug.LogError("Unknown tag " + visibleTargets[i].tag + ". Add it to PrioritizeTarget() in Patrol");
                    break;
            }
        }

        return priorityTarget; 
    }

 

    #region Animator states
    private void StateChase()
    {
        _anim.SetBool("isChasing", true);
        _anim.SetBool("isPanicking", false);
        _anim.SetBool("isPatrolling", false);
        _anim.SetBool("isIdle", false);
    }

    private void StatePatrol()
    {
        _anim.SetBool("isPatrolling", true);
        _anim.SetBool("isPanicking", false);
        _anim.SetBool("isChasing", false);
        _anim.SetBool("isIdle", false);
    }

    private void StatePanick()
    {
        _anim.SetBool("isPanicking", true);
        _anim.SetBool("isPatrolling", false);
        _anim.SetBool("isChasing", false);
        _anim.SetBool("isIdle", false);
    }
    private void StateIdle()
    {
        _anim.SetBool("isIdle", true);
        _anim.SetBool("isPanicking", false);
        _anim.SetBool("isPatrolling", false);
        _anim.SetBool("isChasing", false);
    }

    private void StateDoor()
    {
        _anim.SetTrigger("openDoor");
    }

    private void StateDetention()
    {
        _anim.SetTrigger("giveDetention");
    }

    private bool isPanicking()
    {
        return _anim.GetBool("isPanicking");
    }

    #endregion

    #region Freeze and resume
    bool wasIdling;
    public void Freeze()
    {
        wasIdling = _anim.GetBool("isIdling");
        _anim.SetBool("isIdling", true);
        _agent.isStopped = true;
    }
    public void Resume()
    {
        if (!wasIdling)
            _anim.SetBool("isIdling", false);
        _agent.isStopped = false;
    }
    #endregion
}
