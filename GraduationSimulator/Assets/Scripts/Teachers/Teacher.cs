using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Teacher : MonoBehaviour
{
    public enum Type
    {
        patrol,
        idle
    }
    public float PatrolSpeed { get; set; } = 1f;            // NPC speed when patrolling
    public float ChaseSpeed { get; set; } = 3f;             // NPC speed when chasing  
    public bool Pause { get; private set; }                 // NPC state of being paused
    public Transform target;                                // The prioritized npc target, defaulting to null
    public bool restart;                                    // Resets patrols
    public Type type;
    public List<Transform> checkpoints = new List<Transform>();    // An array holding the checkpoints the teacher will go to

    private NavMeshAgent _agent;                            // The npc agent
    private Animator _anim;                                 // The npc state machine
    private FieldOfView _fow;                               // The npc field of view
    private float _prevSpeed;                               // Holds animation speed when pausing

    private void Awake()
    {
        // Get components
        _anim = GetComponent<Animator>();
        _agent = GetComponent<NavMeshAgent>();
        _fow = GetComponent<FieldOfView>();
        // Check that you found all required components
        if (_anim == null) Debug.LogError("Couldn't find animator");
        if (_agent == null) Debug.LogError("Couldn't find agent");
        if (_fow == null) Debug.LogError("Couldn't find field of view");

        InstantiateCheckpoints();
    }

    // Gets a list of targets from field of view and decide which one to go for
    public void SetTarget(List<Transform> visibleTargets)
    {
        Transform t = visibleTargets[0];
        // If there's more than one target, find the highest priority one
        if (visibleTargets.Count > 1)
            t = PrioritizeTarget(visibleTargets);
        target = t;
    }
    #region SetTarget overloads
    public void SetTarget(Transform t)
    {
        target = t;
    }
    public void SetTarget()
    {
        target = null;
        ChangeBackState();
    }
    #endregion

    private void ChangeBackState()
    {
        _anim.SetBool("isChasing", false);
        _anim.SetBool("isPatrolling", true);
    }
    
    public void GetDazed()
    {
        _anim.SetTrigger("gotDazed");
    }

    private Transform PrioritizeTarget(List<Transform> visibleTargets)
    {
        Transform priorityTarget = visibleTargets[0];   // Instantiate it with the first alternative
        int priorityValue = 10;                         // Lower value = higher priority

        // This code goes through each target by tag and checks whether a higher priority target ...
        // is already selected and if not sets the current one
        for (int i = 0; i < visibleTargets.Count; i++)
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

        return priorityTarget;
    }

    private void InstantiateCheckpoints()
    {
        checkpoints.Clear();   // Ensure checkpoints are empty before running
                                        // Get the first sibling (Checkpoints) and add all its children to the _checkpoints list.
        foreach (Transform child in transform.parent.GetChild(transform.GetSiblingIndex() + 1))
            checkpoints.Add(child.transform);
        // Set the type of teacher
        if (checkpoints.Count == 1)
            type = Type.idle;
        else
            type = Type.patrol;
    }


    public void DestroyTarget()
    {
        if(target.gameObject != null)
            Destroy(target.gameObject);
    }

    public void PowerUp(float patrolMultiplier, float chaseMultiplier, float fowAngleMultiplier)
    {
        PatrolSpeed *= patrolMultiplier;
        ChaseSpeed *= chaseMultiplier;
        _fow.viewAngle *= fowAngleMultiplier;
    }

    #region Freeze and resume
    public void Freeze()
    {
        Pause = true;
        _agent.isStopped = true;
        _prevSpeed = _anim.speed;
        _anim.speed = 0;
    }
    public void Resume()
    {
        Pause = false;
        _agent.isStopped = false;
        _anim.speed = _prevSpeed;
    }
    #endregion
}