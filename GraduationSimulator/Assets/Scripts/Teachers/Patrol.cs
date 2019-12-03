using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Patrol : MonoBehaviour
{
    [SerializeField] Transform[] _checkpoints = default;    // An array holding the checkpoints the teacher will go to
    private NavMeshAgent _agent;                            // Used for AI commands and initiated in Start()
    [SerializeField] private int _nextCheckpoint;                            // Holds the next checkpoint the teacher should go to
    private Animator _anim;
    private float _chasingSpeed = 5f;
    private float _baseSpeed = 1f;
    private bool _pause;
    private bool _caught;
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

        SetStateBool("isPatrolling");

        // Set the agent destination to the next checkpoint in the array
        _agent.destination = _checkpoints[_nextCheckpoint].position;

        // After the last checkpoint in the array comes the first one, so make sure they're close together.
        _nextCheckpoint = (_nextCheckpoint + 1) % _checkpoints.Length;
    }

    public void ChaseTarget(List<Transform> visibleTargets)
    {
        if (isPanicking())
            return;

        Transform target = visibleTargets[0];
        // If there's more than one target, find the highest priority one
        if (visibleTargets.Count > 1)
            target = PrioritizeTarget(visibleTargets);

        //_agent.SetDestination(target.position);

        switch (target.tag)
        {
            case "Vial":
                ChaseVial(target);
                break;
            case "Player":
                ChasePlayer(target);
                break;
            case "Apple":
                ChaseApple(target);
                break;
            default:
                Debug.LogError("Unknown target");
                break;
        }
    }

    private void ChasePlayer(Transform target)
    {
        if (_pause)
            return;
        SetStateBool("isChasing");
        _agent.SetDestination(target.position);
        float distance = Vector3.Distance(transform.position, target.position);
        if (distance < 3f && !_caught)
            StartCoroutine(GotCaught(target));
    }
  
    IEnumerator GotCaught(Transform player)
    {
        _caught = true;
        Player playerScript = player.GetComponent<Player>();

        // Have both the patrol and player stop and look at each other
        playerScript.StopAndLookAt(this.gameObject.transform);
        StopAndLook(player);
        // Play the give detention animation and wait for it to finish
        SetStateTrigger("giveDetention");
        yield return new WaitForSeconds(5f);
        playerScript.Detention();
        _agent.isStopped = false;
        _caught = false;
    }

    #region Help functions
    private void FaceTarget(Vector3 destination)
    {
        Vector3 lookPos = destination - transform.position;
        lookPos.y = 0;
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(lookPos), 1f);
    }

    private void StopAndLook(Transform t)
    {
        _agent.isStopped = true;
        FaceTarget(t.position);
    }

    private Transform PrioritizeTarget(List<Transform> visibleTargets)
    {
        Transform priorityTarget = visibleTargets[0];
        int priorityValue = 10;

        // This code goes through each target by tag and checks whether a higher priority target is already selected, and if not sets the current one
        for (int i = 0; i < visibleTargets.Count; i++)
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
    #endregion

    #region Apple Skill
    private void ChaseApple(Transform apple)
    {
        SetStateBool("isPatrolling");
        _agent.SetDestination(apple.position);
        float distance = Vector3.Distance(transform.position, apple.position);
        if (distance < 1f)
            StartCoroutine(WaitAtApple(apple));
    }

    private IEnumerator WaitAtApple(Transform apple)
    {
        SetStateBool("isIdling");
        StopAndLook(apple);
        yield return new WaitForSeconds(5f);
        SetStateTrigger("pickUp");
        yield return new WaitForSeconds(0.5f);
        if (apple != null)
            Destroy(apple.gameObject);//.GetComponent<Apple>().DestroyApple();
        _agent.isStopped = false;
    }
    #endregion

    #region Science skill vial with explosion
    public void ChaseVial(Transform vial)
    {
        if (isPanicking())
            return;

        SetStateBool("isChasing");
        _agent.SetDestination(vial.position);
        float distance = Vector3.Distance(this.gameObject.transform.position, vial.position);
        if (distance < 1f)
            StartCoroutine(WaitAtVial(vial));
    }

    private IEnumerator WaitAtVial(Transform vial)
    {
        TriggeredVial vialScript = vial.GetComponent<TriggeredVial>();
        // Have the agent stop and look at the fire while panicking for 4 seconds
        SetStateBool("isPanicking");
        StopAndLook(vial);
        yield return new WaitForSeconds(4f);
        // Turn off the fire and have the agent pick it up
        vialScript.TurnOffFire();
        SetStateTrigger("pickUp");
        yield return new WaitForSeconds(0.5f); // Time this one with the pick up animation so the vial goes away when he bends down to it
        if (vial != null)
            vialScript.DestroyVial();
        _agent.isStopped = false;              // Let the agent move again and start patrolling
        SetStateBool("isPatrolling");
    }
    #endregion

    #region Animator states
    private void SetStateBool(string state)
    {
        foreach (AnimatorControllerParameter p in _anim.parameters)
            _anim.SetBool(p.name, false);
        _anim.SetBool(state, true);
    }
    
    private void SetStateTrigger(string state)
    {
        _anim.SetTrigger(state);
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
        _pause = true;
        _agent.isStopped = true;
        wasIdling = _anim.GetBool("isIdling");
        _anim.SetBool("isIdling", true);
    }
    public void Resume()
    {
        _pause = false;
        _agent.isStopped = false;
        if (!wasIdling)
            _anim.SetBool("isIdling", false);
    }
    #endregion
}
