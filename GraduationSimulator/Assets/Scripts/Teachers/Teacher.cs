using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Teacher : MonoBehaviour
{
    public float PatrolSpeed { get; set; } = 1f;
    public float ChaseSpeed { get; set; } = 1f;
    public bool Pause { get; set; }
    public Transform target;
    public string previousState;

    private NavMeshAgent _agent;                            // Used for AI commands and initiated in Start()
    private Animator _anim;
    private bool _caught;

    private void Awake()
    {
        _anim = GetComponent<Animator>();
        _agent = GetComponent<NavMeshAgent>();
        // Check that you found all required components
        if (_anim == null) Debug.LogError("Couldn't find animator");
        if (_agent == null) Debug.LogError("Couldn't find agent");
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
        _anim.SetBool(previousState, true);
    }

    public void GetDazed()
    {
        _anim.SetTrigger("gotDazed");
    }

    private Transform PrioritizeTarget(List<Transform> visibleTargets)
    {
        Transform priorityTarget = visibleTargets[0];
        int priorityValue = 10;

        // This code goes through each target by tag and checks whether a higher priority target is already selected, and if not sets the current one
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


    public void DestroyTarget()
    {
        Destroy(target.gameObject);
    }

    #region Freeze and resume
    float prevSpeed;
    public void Freeze()
    {
        Pause = true;
        _agent.isStopped = true;
        prevSpeed = _anim.speed;
        _anim.speed = 0;
    }
    public void Resume()
    {
        Pause = false;
        _agent.isStopped = false;
        _anim.speed = prevSpeed;
    }
    #endregion
}